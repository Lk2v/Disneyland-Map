using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DisneylandMap.src.Graphe
{
	public class Donnees
	{
        static string dossierProjet = Path.Combine(Directory.GetParent(Environment.CurrentDirectory!)!.Parent!.Parent!.FullName, "Assets");

        /*
            En utilisant la syntaxe "using", les objets StreamReader seront automatiquement fermés et libérés de la mémoire
            dès que le bloc de code sera terminé, même en cas d'exception.
            Cela peut donc améliorer la gestion des ressources et la performance
         */

        public static HashSet<Sommet> ChargerListeSommets()
        {
            HashSet<Attraction> attractions = ChargerAttractions();
            HashSet<Sommet> sommets = ChargerSommets(attractions);

            return sommets;
        }

        static HashSet<Attraction> ChargerAttractions()
        {
            HashSet<Attraction> ls_attractions = new HashSet<Attraction>();
            using (StreamReader sr = new StreamReader(Path.Combine(dossierProjet, "attractions.csv")))
            {
                int i = 0;
                string ligne = "";
                string[] champs = new string[3];
                while (sr.Peek() > 0)
                {
                    ligne = sr.ReadLine()!;
                    if (ligne.StartsWith("#") || ligne.Trim() == "") continue; // La ligne est vide ou en commentaire 

                    champs = ligne.Split(';');

                    Attraction v = new Attraction
                    {
                        Num = Int32.Parse(champs[0]),
                        Nom = champs[1],
                        Categorie = (AttractionCategorie)Int32.Parse(champs[2]),
                    };

                    ls_attractions.Add(v);
                    i++;
                }
            }

            return ls_attractions;
        }

        static HashSet<Sommet> ChargerSommets(HashSet<Attraction> ls_attractions)
        {
            HashSet<Sommet> ls_sommets = new HashSet<Sommet>();
            using (StreamReader sr = new StreamReader(Path.Combine(dossierProjet, "sommets.csv")))
            {
                int i = 0;
                string ligne = "";
                string[] champs = new string[3];
                while (sr.Peek() > 0)
                {
                    ligne = sr.ReadLine()!;
                    if (ligne.StartsWith("#")) continue; // La ligne est en commentaire

                    champs = ligne.Split(';');

                    Attraction? attraction;
                    

                    if (champs[3] == "-")
                    {
                        attraction = null;
                    } else
                    {
                        int attractionNum = Int32.Parse(champs[3]);
                        attraction = ls_attractions.First((attr) => attr.Num == attractionNum);
                    }

                    Sommet v = new Sommet
                    {
                        Id = Int32.Parse(champs[0]),
                        I = i,

                        X = Convert.ToDouble(champs[1]),
                        Y = Convert.ToDouble(champs[2]),
                        Attraction = attraction,
                    };

                    ls_sommets.Add(v);
                    i++;
                }
            }

            return ls_sommets;
        }

        public static HashSet<Chemin> ChargerChemins(HashSet<Sommet> sommets)
        {
            HashSet<Chemin> ls_Chemins = new HashSet<Chemin>();

            bool sv = false;

            using (StreamReader sr = new StreamReader(Path.Combine(dossierProjet, "chemins.csv")))
            {
                string ligne = "";

                string[] info = new string[4];

                int i = 1;
                while (sr.Peek() > 0)
                {
                    ligne = sr.ReadLine()!;
                    if (ligne.StartsWith("#") || ligne.Trim() == "") continue; // La ligne est en commentaire

                    info = ligne.Split(';');

                    int id1 = Int32.Parse(info[0]);
                    int id2 = Int32.Parse(info[1]);

                    Sommet s1 = sommets.First(item => item.Id == id1);
                    Sommet s2 = sommets.First(item => item.Id == id2);

                    double longueur = Math.Sqrt(Math.Pow(s1.X - s2.X, 2) + Math.Pow(s1.Y - s2.Y, 2)) / 1.2; // reequilibrage rapport echelle 1.2 

                    double densite;
                    if(info.Length >= 3)
                    {
                        densite = double.Parse(info[2]);
                    } else
                    {
                        Random r = new Random();
                        densite = Math.Round(r.NextDouble(), 2);
                        sv = true;
                    }

                    Chemin rd = new Chemin
                    {
                        I = i,
                        S1 = s1,
                        S2 = s2,
                        Longueur = longueur,
                        Densite = densite,
                    };

                    ls_Chemins.Add(rd);
                    i++;
                }
            }

            if (sv) MisAJourDensite(ls_Chemins);
            return ls_Chemins;
        }

        public static void MisAJourDensite(HashSet<Chemin> chemins)
        {
            using (StreamWriter writer = new StreamWriter(Path.Combine(dossierProjet, "chemins.csv")))
            {
                foreach (Chemin c in chemins)
                { 
                    writer.WriteLine(c.ToCSV()); // écrire une ligne dans le fichier
                }
            }
        }
    }
}

