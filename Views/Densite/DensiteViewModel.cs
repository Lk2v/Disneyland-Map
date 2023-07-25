using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
using DisneylandMap.Dialogs;
using DisneylandMap.src.Graphe;
using DisneylandMap.src.MessageBus;
using ReactiveUI;


namespace DisneylandMap.Views
{
    public class DensiteViewModel : ReactiveObject, IRoutableViewModel
    {
        // Reference to IScreen that owns the routable view model.
        public IScreen HostScreen { get; }

        // Unique identifier for the routable view model.
        public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

        public ReactiveCommand<Chemin, Unit> SelectionnerChemin { get; }
        public ReactiveCommand<Unit, Unit> GoHome { get; }
        public ReactiveCommand<Unit, Unit> AleatoireDensite { get; }

        public HashSet<Chemin> _listeChemins;

        public HashSet<Chemin> ListeChemins
        {
            get => _listeChemins;
            set => this.RaiseAndSetIfChanged(ref _listeChemins, value);
        }

        IMessageBus Bus;

        public DensiteViewModel(IScreen screen, IMessageBus b)
        {
            HostScreen = screen;
            Bus = b;

            SelectionnerChemin = ReactiveCommand.Create<Chemin>(SelecChemin);
            _listeChemins = Graphe.ListeChemins;
            GoHome = ReactiveCommand.Create(BackHome);
            AleatoireDensite = ReactiveCommand.Create(RegenererAleatoireDensite);
        }

        
        void BackHome()
        {
            Bus.SendMessage(new MessageBusViewSwitcher(ViewSwitcherMessageBus.Home));
        }

        void RegenererAleatoireDensite()
        {
            ListeChemins = new HashSet<Chemin>(Graphe.GenererDensiteAlea());
        }

        async void SelecChemin(Chemin c)
        {
            await CheminParamsDialog(c);
        }

        async Task CheminParamsDialog(Chemin chemin)
        {

            if (App.MainWindow != null)
            {
                var dialog = new CheminDensiteParamsWindow(chemin);
                DialogCheminDensiteParams res = await dialog.ShowDialog<DialogCheminDensiteParams>(App.MainWindow);

                if (res == null) return; // la fenetre d'ajout de recette a été fermée
                chemin.Densite = res.Valeur;
                Donnees.MisAJourDensite(Graphe.ListeChemins);
                ListeChemins = new HashSet<Chemin>(Graphe.ListeChemins); // mis a jour de l'UI
            }
        }

        void SelecAttraction(Sommet s)
        {
            Console.WriteLine(s);
        }
    }
}

