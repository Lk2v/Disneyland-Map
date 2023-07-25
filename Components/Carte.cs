
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Media;
using DisneylandMap.src.Graphe;


namespace DisneylandMap.Components
{
    public class Carte : UserControl
    {
        public static readonly StyledProperty<HashSet<Sommet>> SommetsProperty =
            AvaloniaProperty.Register<Carte, HashSet<Sommet>>(nameof(Sommets), new HashSet<Sommet>());

        public static readonly StyledProperty<HashSet<Chemin>> ItineraireCheminsProperty =
            AvaloniaProperty.Register<Carte, HashSet<Chemin>>(nameof(ItineraireChemins), new HashSet<Chemin>());

        public static readonly StyledProperty<HashSet<Chemin>> DensiteCheminsProperty =
            AvaloniaProperty.Register<Carte, HashSet<Chemin>>(nameof(DensiteChemins), new HashSet<Chemin>());


        public static readonly StyledProperty<Sommet> DepartProperty =
            AvaloniaProperty.Register<Carte, Sommet>(nameof(Depart));

        public static readonly StyledProperty<Sommet> ArriveeProperty =
            AvaloniaProperty.Register<Carte, Sommet>(nameof(Arrivee));

        // Evenements

        public HashSet<Sommet> Sommets
        {
            get { return GetValue(SommetsProperty); }
            set { SetValue(SommetsProperty, value); }
        }

        public HashSet<Chemin> ItineraireChemins
        {
            get { return GetValue(ItineraireCheminsProperty); }
            set { SetValue(ItineraireCheminsProperty, value); }
        }

        public HashSet<Chemin> DensiteChemins
        {
            get { return GetValue(DensiteCheminsProperty); }
            set { SetValue(DensiteCheminsProperty, value); }
        }

        public Sommet Depart
        {
            get { return GetValue(DepartProperty); }
            set { SetValue(DepartProperty, value); }
        }

        public Sommet Arrivee
        {
            get { return GetValue(ArriveeProperty); }
            set { SetValue(ArriveeProperty, value); }
        }

        private ICommand? _attractionCommand;

        public ICommand AttractionCommand
        {
            get
            {
                return _attractionCommand!;
            }
            set
            {
                this.SetAndRaise(AttractionCommandProperty, ref _attractionCommand!, value);
            }
        }

        public static readonly DirectProperty<Carte, ICommand> AttractionCommandProperty =
        AvaloniaProperty.RegisterDirect<Carte, ICommand>(
            nameof(AttractionCommand),
            (Carte c) => c.AttractionCommand,
            delegate (Carte c, ICommand cmd) {
                c.AttractionCommand = cmd;
            }, defaultBindingMode: BindingMode.OneWay);

        private ICommand? _cheminCommand;

        public ICommand CheminCommand
        {
            get
            {
                return _cheminCommand!;
            }
            set
            {
                this.SetAndRaise(CheminCommandProperty, ref _cheminCommand!, value);
            }
        }

        public static readonly DirectProperty<Carte, ICommand> CheminCommandProperty =
        AvaloniaProperty.RegisterDirect<Carte, ICommand>(
            nameof(CheminCommand),
            (Carte c) => c.CheminCommand,
            delegate (Carte c, ICommand cmd) {
                c.CheminCommand = cmd;
            }, defaultBindingMode: BindingMode.OneWay);

        public Carte()
        {
            this.GetObservable(SommetsProperty).Subscribe(_ => InvalidateVisual());
            this.GetObservable(ItineraireCheminsProperty).Subscribe(_ => InvalidateVisual());

            this.GetObservable(DensiteCheminsProperty).Subscribe(_ => InvalidateVisual());
            this.GetObservable(DepartProperty).Subscribe(_ => InvalidateVisual());
            this.GetObservable(ArriveeProperty).Subscribe(_ => InvalidateVisual());
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);

            if (e.GetCurrentPoint(this).Properties.PointerUpdateKind == PointerUpdateKind.LeftButtonPressed)
            {
                Point clickPoint = e.GetPosition(this);

                // Vérifier si le clic se situe sur une arête
                Chemin? cheminClick = null;
                foreach (Chemin r in DensiteChemins)
                {
                    // Calculer la distance entre le clic et l'arête
                    double distance = r.DistancePoint(clickPoint);

                    // Si la distance est inférieure à un seuil, l'arête a été cliquée
                    if (distance < 15)
                    {
                        
                        if (cheminClick == null || distance < cheminClick.DistancePoint(clickPoint)) cheminClick = r;
                    }
                }


                if (cheminClick != null && CheminCommand != null && CheminCommand.CanExecute(null))
                {
                    Console.WriteLine(cheminClick);
                    CheminCommand.Execute(cheminClick);
                }

                // Click sommets
                foreach (Sommet v in Sommets)
                {
                    if (!v.EstAttraction) continue;

                    double distance = Math.Sqrt(Math.Pow(clickPoint.X - v.X, 2) + Math.Pow(clickPoint.Y - v.Y, 2)); // Pythagore on cherche (hypothenus=distance)

                    if (distance <= 20) // Rayon de l'ellipse
                    {
                        if (AttractionCommand != null && AttractionCommand.CanExecute(null))
                        {
                            AttractionCommand.Execute(v);
                        }
                    }
                }
            }
        }

        

        public override void Render(DrawingContext context)
        {
            Console.WriteLine($"Mise à jour de la carte");

            // Créer le dégradé linéaire
            var gradientBrush = new Avalonia.Media.LinearGradientBrush
            {
                GradientStops = new GradientStops
            {
                new GradientStop(Colors.Blue, 0.0),
                new GradientStop(Colors.MediumPurple, 1.0)
            },
                StartPoint = new RelativePoint(Depart == null ? 0 : Depart.X, Depart == null ? 0 : Depart.X, RelativeUnit.Absolute),
                EndPoint = new RelativePoint(Arrivee == null ? this.Bounds.Width : Arrivee.X, Arrivee == null ? this.Bounds.Height : Arrivee.Y, RelativeUnit.Absolute),
                SpreadMethod = GradientSpreadMethod.Pad
            };

            // 
            Pen pen = new Pen(Brushes.Black, 5);
            

            Pen ligne = new Pen
            {
                LineJoin = PenLineJoin.Round,
                Thickness=10,
                Brush = gradientBrush,
                LineCap=PenLineCap.Flat,
            };

            Pen ligneBack = new Pen
            {
                LineJoin = PenLineJoin.Round,
                Thickness = 20,
                Brush = Brushes.White,
                LineCap = PenLineCap.Flat,
            };

            var pathGeometry = new PathGeometry();
            // Chemins

            ZIndex = 2;
            foreach (Chemin r in ItineraireChemins)
            {
                var pathFigure = new PathFigure { StartPoint = new Point(r.S1.X, r.S1.Y) };
                pathFigure.Segments!.Add(new LineSegment { Point = new Point(r.S2.X, r.S2.Y) });
                pathGeometry.Figures.Add(pathFigure);
            }

            context.DrawGeometry(null, ligneBack, pathGeometry);
            context.DrawGeometry(null, ligne, pathGeometry);

            foreach(Chemin c in DensiteChemins)
            {
                DessinerDensiteChemin(context, c);
            }

            // Sommets
            foreach (Sommet v in Sommets)
            {
                double rayon = (v.EstAttraction) ? 15 : 5;
                double taillePolice = 18;

                if (v == Depart || v == Arrivee)
                {
                    rayon = rayon*1.5;
                    taillePolice = taillePolice*1.5;
                }

                if(v.EstAttraction)
                {
                    context.DrawEllipse(Brushes.White, pen, new Point(v.X, v.Y), rayon, rayon);
                }
                
                if (v.EstAttraction)
                {
                    var formattedText = new FormattedText
                    {
                        Text = v.Attraction!.Num.ToString(),
                        Typeface = new Typeface("Arial", FontStyle.Normal, FontWeight.Bold),
                        TextAlignment = TextAlignment.Center,
                        FontSize = taillePolice,
                    };

                    context.DrawText(Brushes.Black, new Point(v.X - formattedText.Bounds.Width / 2, v.Y - formattedText.Bounds.Height / 2), formattedText);
                }

                //DEBUG
                //Debug(context, v);
                
            }
        }

        void DessinerDensiteChemin(DrawingContext ctx, Chemin c)
        {
            double px = (c.S1.X + c.S2.X) / 2;
            double py = (c.S1.Y + c.S2.Y) / 2;

            Brush brh = c.CouleurDensite();

            ctx.DrawLine(new Pen(brh, 10), new Point(c.S1.X, c.S1.Y), new Point(c.S2.X, c.S2.Y));

            ctx.DrawEllipse(brh, null, new Point(px, py), 12, 12);

            var text = new FormattedText
            {
                Text = Math.Round(c.Densite*100).ToString(),
                Typeface = new Typeface("Arial", FontStyle.Normal, FontWeight.Bold),
                TextAlignment = TextAlignment.Center,
                FontSize = 18,
            };

            ctx.DrawText(Brushes.White, new Point(px - text.Bounds.Width/2, py - text.Bounds.Height / 2), text);
        }

        void Debug(DrawingContext ctx, Sommet v)
        {
            var debugId = new FormattedText
            {
                Text = v.Id.ToString(),
                Typeface = new Typeface("Arial", FontStyle.Normal, FontWeight.Bold),
                TextAlignment = TextAlignment.Center,
                FontSize = 18,
            };

            ctx.DrawText(Brushes.Red, new Point(v.X - debugId.Bounds.Width / 2, v.Y + debugId.Bounds.Height / 2), debugId);
        }
    }
}