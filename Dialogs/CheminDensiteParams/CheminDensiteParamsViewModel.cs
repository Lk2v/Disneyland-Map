using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using DisneylandMap.src.Graphe;
using ReactiveUI;

namespace DisneylandMap.Dialogs
{
    public class CheminDensiteParamsViewModel : ReactiveObject
    {

        Chemin _chemin;
        public Chemin Chemin
        {
            get => _chemin;
            set => this.RaiseAndSetIfChanged(ref _chemin, value);
        }

        double _valeurDensite;
        public double ValeurDensite
        {
            get => _valeurDensite;
            set {
                this.RaiseAndSetIfChanged(ref _valeurDensite, value);
                NbVisiteurs = (int) Math.Round(value * (Chemin.Longueur * 2.5));
            }
        }

        int _nbVisiteurs;
        public int NbVisiteurs
        {
            get => _nbVisiteurs;
            set => this.RaiseAndSetIfChanged(ref _nbVisiteurs, value);
        }

        public ReactiveCommand<Unit, DialogCheminDensiteParams> Submit { get; }

        public CheminDensiteParamsViewModel(Chemin c)
        {
            _chemin = c;
            ValeurDensite = Chemin.Densite;
            Console.WriteLine(ValeurDensite);

            Submit = ReactiveCommand.Create(SubmitReq);
        }

        DialogCheminDensiteParams SubmitReq()
        {
            return new DialogCheminDensiteParams(ValeurDensite);
        }
    }

    public class DialogCheminDensiteParams
    {
        public double Valeur { get; set; }

        public DialogCheminDensiteParams(double v)
        {
            Valeur = v;
        }
    }
}

