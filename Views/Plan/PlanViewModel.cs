using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Media;
using DisneylandMap.src.Graphe;
using DisneylandMap.src.MessageBus;
using ReactiveUI;


namespace DisneylandMap.Views
{
    public class PlanViewModel : ReactiveObject, IRoutableViewModel, INotifyPropertyChanged
    {
        // Reference to IScreen that owns the routable view model.
        public IScreen HostScreen { get; }

        // Unique identifier for the routable view model.
        public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

        public ReactiveCommand<Unit, Unit> GoHome { get; }

        public ReactiveCommand<Sommet, Unit> SelectionAttraction { get; }

        Itineraire? _itineraireInfo;
        public Itineraire? ItineraireInfo {
            get => _itineraireInfo;
            set
            {
                this.RaiseAndSetIfChanged(ref _itineraireInfo, value);
                NotifyPropertyChanged(nameof(ItineraireInfo));
                NotifyPropertyChanged(nameof(ItineraireAffichee));
            }
        }

        public bool ItineraireAffichee
        {
            get => ItineraireInfo != null;
        }

        HashSet<Sommet> _listeAttractions;
        public HashSet<Sommet> ListeAttractions
        {
            get {
                HashSet<Sommet> attractionsAccesible = new HashSet<Sommet>();
                for(int i = 0; i < _listeAttractions.Count;i++)
                {
                    if(AttractionEstAccessible(_listeAttractions.ElementAt(i)))
                    {
                        attractionsAccesible.Add(_listeAttractions.ElementAt(i));
                    }
                }

                return attractionsAccesible;
            }
            set => this.RaiseAndSetIfChanged(ref _listeAttractions, value);
        }

        HashSet<Sommet> _listeSommets;
        public HashSet<Sommet> ListeSommets
        {
            get => _listeSommets;
            set => this.RaiseAndSetIfChanged(ref _listeSommets, value);
        }

        Sommet? _attractionSelectionnerDepart;
        public Sommet? AttractionSelectionnerDepart
        {
            get => _attractionSelectionnerDepart;
            set
            {
                this.RaiseAndSetIfChanged(ref _attractionSelectionnerDepart, value);
                NotifyPropertyChanged(nameof(AttractionSelectionnerDepart));
                Itineraire();
            }
        }

        Sommet? _attractionSelectionnerArrivee;
        public Sommet? AttractionSelectionnerArrivee
        {
            get => _attractionSelectionnerArrivee;
            set
            {
                this.RaiseAndSetIfChanged(ref _attractionSelectionnerArrivee, value);
                NotifyPropertyChanged(nameof(AttractionSelectionnerArrivee));
                Itineraire();
            }
        }


        ObservableCollection<GraphItem> _listeSousGraphes;

        public ObservableCollection<GraphItem> ListeSousGraphes
        {
            get => _listeSousGraphes;
            set => this.RaiseAndSetIfChanged(ref _listeSousGraphes, value);
        }


        public ReactiveCommand<GraphItem, Unit> SelectionGraph { get; }

        IMessageBus Bus;
        public PlanViewModel(IScreen screen, IMessageBus b)
        {
            HostScreen = screen;
            Bus = b;

            SelectionGraph = ReactiveCommand.Create<GraphItem>(SelecGraphe);
            SelectionAttraction = ReactiveCommand.Create<Sommet>(SelecAttraction);

            List<Sommet> sortedList = new List<Sommet>(Graphe.ListeSommetsAttractions);

            sortedList.Sort((x, y) =>
            {
                if (x.Attraction!.Num == y.Attraction!.Num)
                {
                    return 0;
                }

                if (x.Attraction!.Num == 0)
                {
                    return 1;
                }

                if (y.Attraction!.Num == 0)
                {
                    return -1;
                }

                return x.Attraction!.Num.CompareTo(y.Attraction!.Num);
            });

            _listeSommets = Graphe.ListeSommets;
            _listeAttractions = sortedList.ToHashSet();

            _listeSousGraphes = new ObservableCollection<GraphItem>(Graphe.ListeSousGraphes);

            GoHome = ReactiveCommand.Create(BackHome);

        }

        void BackHome()
        {
            Bus.SendMessage(new MessageBusViewSwitcher(ViewSwitcherMessageBus.Home));
        }

        private void Itineraire()
        {
            Sommet? a = AttractionSelectionnerDepart;
            Sommet? b = AttractionSelectionnerArrivee;

            if (a == null || b == null)
            {
                return;
            }

            Console.WriteLine("Construction de l'itinéraire...");
            if (a == b)
            {
                Console.WriteLine("Vous êtes déjà arrivée a desination");
                return;
            }

            // C'est partit
            Console.WriteLine($"{a} <-> {b}");


            // Dijkastra
            ItineraireInfo = Graphe.Dijkstra2(a, b);
            if(ItineraireInfo.Accessible == false) Console.WriteLine("Attraction innaccessible en raison des mesures de securitées (chemins saturés)");
        }

        void ResetItineraire()
        {
            AttractionSelectionnerDepart = null;
            AttractionSelectionnerArrivee = null;

            ItineraireInfo = null;
        }

        void SelecGraphe(GraphItem gi)
        {

            gi.Selectionner = !gi.Selectionner;
            
            if (!(AttractionEstAccessible(_attractionSelectionnerDepart!) && AttractionEstAccessible(_attractionSelectionnerArrivee!)))
            {
                ResetItineraire();
            }

            NotifyPropertyChanged(nameof(ListeAttractions));
        }

        void SelecAttraction(Sommet s)
        {
            if(_attractionSelectionnerDepart != null && _attractionSelectionnerArrivee != null)
            {
                ResetItineraire();
            }

            if(_attractionSelectionnerDepart == null)
            {
                ResetItineraire();
                AttractionSelectionnerDepart = s;
            } else if(_attractionSelectionnerArrivee == null)
            {
                AttractionSelectionnerArrivee = s;
            }
        }

        private bool AttractionEstAccessible(Sommet s)
        {
            if (s == null) return false;

            bool res = false;
            foreach (GraphItem g in _listeSousGraphes)
            {
                if (g.Selectionner && g.Type == s.Attraction!.Categorie)
                {
                    res = true;
                    break;
                }
            }
            return res;
        }


        public new event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([System.Runtime.CompilerServices.CallerMemberName]
        string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

