using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Collections;
using System.Linq;
using Avalonia.Media;
using System.ComponentModel;
using Avalonia.Controls;

namespace DisneylandMap.src.Graphe
{
    public class Graphe
    {
        static HashSet<Sommet> _listeSommets = new HashSet<Sommet>();
        static HashSet<Chemin> _listeChemins = new HashSet<Chemin>();

        public static HashSet<Sommet> ListeSommets
        {
            get => _listeSommets;
        }

        public static int Ordre
        {
            get => _listeSommets.Count;
        }

        public static HashSet<Sommet> ListeSommetsAttractions
        {
            get
            {
                HashSet<Sommet> listeAttractions = new HashSet<Sommet>();
                foreach(Sommet s in _listeSommets)
                {
                    if(s.EstAttraction)
                    {
                        listeAttractions.Add(s);
                    }
                }

                return listeAttractions;
            }
        }

        public static HashSet<Chemin> ListeChemins
        {
            get => _listeChemins;
        }

        public static HashSet<Chemin> ListeCheminsDesaturer
        {
            get
            {
                HashSet<Chemin> list = new HashSet<Chemin>();
                foreach(Chemin c in _listeChemins)
                {
                    if(c.Densite < 0.9) list.Add(c);
                }
                return list;
            }
        }

        public static GraphItem[] ListeSousGraphes
        {
            get
            {
                GraphItem[] liste = new GraphItem[Enum.GetNames(typeof(AttractionCategorie)).Length];

                for(int i = 0; i < liste.Length;i++)
                {
                    liste[i] = new GraphItem
                    {
                        Type=(AttractionCategorie)i,
                        Selectionner=true,
                    };
                }

                return liste;
            }
        }

        private static double[,] matriceAdj = new double[0, 0];
        public static double[,] MatriceAdj
        {
            get { return matriceAdj; }
            set { matriceAdj = value; }
        }

        public static void Initialiser()
        {
            Console.WriteLine("Chargement des données...");
            _listeSommets = Donnees.ChargerListeSommets();
            _listeChemins = Donnees.ChargerChemins(_listeSommets);
            Console.WriteLine("Donnée chargée !");



            matriceAdj = CreationMatriceAdj();
        }


        public static HashSet<Chemin> GenererDensiteAlea()
        {
            for(int i = 0; i < _listeChemins.Count; i++)
            {
                Random r = new Random();
                _listeChemins.ElementAt(i).Densite = Math.Round(r.NextDouble(), 2);
            }

            Donnees.MisAJourDensite(_listeChemins); // sauvegarde

            return _listeChemins;
        }

        // fonctionne : NON
        public static double[,] CreationMatriceAdj(bool modeAntiSaturation = true)
        {
            
            double[,] matAdj = new double[_listeSommets.Count, _listeSommets.Count];

            int[] sommets = new int[_listeSommets.Count];

            for (int k = 0; k < _listeSommets.Count; k++)
            {
                sommets[k] = _listeSommets.ElementAt(k).Id;
            }

            for (int i = 0; i < _listeSommets.Count; i++)
            {
                for (int j = 0; j < _listeSommets.Count; j++)
                {
                    if (i == j) matAdj[i, j] = 0;
                    else
                    {
                        foreach (Chemin c in _listeChemins)
                        {
                            if (c.S1.Id == sommets[i] || c.S2.Id == sommets[i])
                            {
                                if (c.S1.Id == sommets[j] || c.S2.Id == sommets[j])
                                {
                                    matAdj[i, j] = c.LongueurCalc;
                                }
                            }
                        }
                    }
                }
            }

            return matAdj;
        }

        /*
        public static HashSet<Sommet> Dijkstra(Sommet source)
        {
            HashSet<Sommet> listeSommetsDistance = ListeSommets;

            foreach (Sommet s in _listeSommets)
            {
                if (s.Id == source.Id) source.Id = s.Id;
            }
            double[] distances = new double[_listeSommets.Count]; // Tableau pour stocker les distances minimales
            bool[] tab = new bool[_listeSommets.Count]; // Tableau pour stocker les sommets visités
            int[,] mat = new int[_listeSommets.Count, _listeSommets.Count];
            string[,] matrice = new string[_listeSommets.Count, _listeSommets.Count];
            // Initialiser les distances à l'infini sauf pour la source

            for (int i = 0; i < _listeSommets.Count; i++)
            {
                distances[i] = double.MaxValue;
                tab[i] = false;
            }
            distances[source.Id] = 0; // La distance de la source à elle-même est de 0

            for (int count = 0; count < _listeSommets.Count - 1; count++)
            {
                int u = MinDistance(distances, tab);
                tab[u] = true;

                for (int v = 0; v < _listeSommets.Count; v++)
                {
                    if (!tab[v] && matriceAdj[u, v] != 0 && distances[u] != double.MaxValue && distances[u] + matriceAdj[u, v] < distances[v])
                    {
                        distances[v] = distances[u] + matriceAdj[u, v];
                    }
                }
            }

            for (int i = 0; i < listeSommetsDistance.Count; i++)
            {
                listeSommetsDistance.ElementAt(i).Distance = distances[i];
            }

            return listeSommetsDistance;
        }
        */

        private static int MinDistance(double[] distances, bool[] shortestPathSet)
        {
            double min = double.MaxValue;
            int minIndex = 0;
            for (int v = 0; v < _listeSommets.Count; v++)
            {
                if (!shortestPathSet[v] && distances[v] <= min)
                {
                    min = distances[v];
                    minIndex = v;
                }
            }
            return minIndex;
        }


        public static Itineraire Dijkstra2(Sommet source, Sommet arrive)
        {
            double[,] matAdjDst = CreationMatriceAdj();

            #region Initialisation
            foreach (Sommet v in _listeSommets)
            {
                if (source.Id == v.Id) source = v;
                if (arrive.Id == v.Id) arrive = v;
            }

            List<Sommet> lst = new List<Sommet>();
            for (int i = 0; i < _listeSommets.Count; i++)
            {
                Sommet s = _listeSommets.ElementAt(i).Clone();

                lst.Add(s);
            }
            #endregion

            double[] distances = new double[_listeSommets.Count]; // Tableau pour stocker les distances minimales
            bool[] tab = new bool[_listeSommets.Count]; // Tableau pour stocker les sommets visités
            int[,] mat = new int[_listeSommets.Count, _listeSommets.Count];

            int[,] matrice = new int[_listeSommets.Count, _listeSommets.Count];
            // Initialiser les distances à l'infini sauf pour la source
            for (int i = 0; i < _listeSommets.Count; i++)
            {
                distances[i] = double.MaxValue;
                tab[i] = false;
            }
            distances[source.I] = 0; // La distance de la source à elle-même est de 0
            for (int count = 0; count < _listeSommets.Count - 1; count++)
            {
                int u = MinDistance(distances, tab);
                tab[u] = true;
                for (int v = 0; v < _listeSommets.Count; v++)
                {
                    if (!tab[v] && matAdjDst[u, v] != 0 && distances[u] != double.MaxValue && distances[u] + matAdjDst[u, v] < distances[v])
                    {
                        distances[v] = distances[u] + matAdjDst[u, v];
                        lst[v].Pred = lst[u];
                    }
                }
            }

            HashSet<Sommet> tr = new HashSet<Sommet>();
            int dep = source.I;
            int f = arrive.I;
            tr.Add(arrive);

            bool parcourir = true;
            bool innaccesible = false;

            while (parcourir)
            {
                if(lst[f].Pred != null)
                {
                    if(lst[f].Pred!.I != dep)
                    {
                        tr.Add(lst[f].Pred!);
                        f = lst[f].Pred!.I;
                    } else
                    {
                        parcourir = false;
                    }
                } else
                {
                    parcourir = false;
                    innaccesible = true;
                }
            }
            if (!innaccesible)
            {
                tr.Add(source);


                HashSet<Chemin> trajet = new HashSet<Chemin>();
                for (int i = 0; i < tr.Count - 1; i++)
                {
                    for (int w = 0; w < _listeChemins.Count; w++)
                    {
                        if (_listeChemins.ElementAt(w).Contient(tr.ElementAt(i), tr.ElementAt(i + 1)))
                        {
                            trajet.Add(_listeChemins.ElementAt(w));
                            break;
                        }
                    }
                }

                return new Itineraire
                {
                    Parcours = trajet,
                    Depart = source,
                    Arrivee = arrive,
                    FlotMax = MaxFlot(source.I, arrive.I),
                };
            } else
            {
                return new Itineraire
                {
                    Depart = source,
                    Arrivee = arrive,
                    Accessible = false
                };
            }
        }


        // Fonction qui calcule le flot maximal dans un graphe de flots à l'aide de l'algorithme de Ford-Fulkerson
        public static double MaxFlot(int source, int destination)
        {
            double[,] graphe = (double[,]) matriceAdj.Clone();

            int[] parent = new int[Ordre]; // Tableau pour stocker les parents des sommets lors de la recherche de chemin augmentant
            double maxFlot = 0; // Initialise le flot maximal à 0

            // Tant qu'il y a un chemin augmentant dans le graphe résiduel
            while (true)
            {
                bool[] visite = new bool[Ordre];
                visite[source] = true;

                // Trouve un chemin augmentant dans le graphe résiduel
                if (!DFS(graphe, source, destination, visite, parent))
                {
                    break;
                }

                // Calcule le flot maximal du chemin augmentant et met à jour le graphe résiduel
                double flotChemin = double.MaxValue;
                for (int v = destination; v != source; v = parent[v])
                {
                    int u = parent[v];
                    flotChemin = Math.Min(flotChemin, graphe[u, v]);
                }

                for (int v = destination; v != source; v = parent[v])
                {
                    int u = parent[v];
                    graphe[u, v] -= flotChemin;
                    graphe[v, u] += flotChemin;
                }

                maxFlot += flotChemin;
            }

            // Retourne le flot maximal
            return Math.Round(maxFlot, 2);
        }

        // Fonction récursive qui recherche un chemin augmentant dans le graphe résiduel
        static bool DFS(double[,] graphe, int u, int destination, bool[] visite, int[] parent)
        {
            if (u == destination)
            {
                return true;
            }

            for (int v = 0; v < Ordre; v++)
            {
                if (visite[v] == false && graphe[u, v] > 0)
                {
                    visite[v] = true;
                    parent[v] = u;

                    if (DFS(graphe, v, destination, visite, parent))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }

    public class GraphItem: INotifyPropertyChanged
    {
        public AttractionCategorie Type { get; set; }

        bool _selectionner;
        public bool Selectionner {
            get => _selectionner;
            set
            {
                _selectionner = value;
                NotifyPropertyChanged(nameof(Selectionner));
            }
        }

        public IBrush Couleur
        {
            get
            {
                switch(Type)
                {
                    case AttractionCategorie.MainStreet:
                        return new SolidColorBrush(Colors.White, 0.8);

                    case AttractionCategorie.AdventureLand:
                        return new SolidColorBrush(Color.FromRgb(249, 219, 84));

                    case AttractionCategorie.DiscoveryLand:
                        return new SolidColorBrush(Color.FromRgb(235, 81, 121));

                    case AttractionCategorie.FantasyLand:
                        return new SolidColorBrush(Color.FromRgb(198, 133, 222));

                    case AttractionCategorie.FrontierLand:
                        return new SolidColorBrush(Color.FromRgb(246, 164, 65));
                }

                return Brushes.White;
            }
        }

        #region UI
        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([System.Runtime.CompilerServices.CallerMemberName]
        string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

    }
}