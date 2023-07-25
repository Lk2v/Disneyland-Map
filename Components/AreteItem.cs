
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
    public class AreteItem : UserControl
    {
        public static readonly StyledProperty<Chemin> CheminProperty =
            AvaloniaProperty.Register<AreteItem, Chemin>(nameof(Chemin), new Chemin());

        public Chemin Chemin
        {
            get { return GetValue(CheminProperty); }
            set { SetValue(CheminProperty, value); }
        }


        public static readonly StyledProperty<double> TailleProperty =
            AvaloniaProperty.Register<AreteItem, double>(nameof(Taille), 25.0);

        public double Taille
        {
            get { return GetValue(TailleProperty); }
            set {
                SetValue(TailleProperty, value);
                UpdateSize();
            }
        }
        

        public AreteItem()
        {
            UpdateSize();
        }

        private void UpdateSize()
        {
            Width = Taille;
            Height = Taille;
        }

        public override void Render(DrawingContext context)
        {
            // Récupérer l'angle de la ligne en radians
            double angleRadians = Math.Atan2(Chemin.S2.Y - Chemin.S1.Y, Chemin.S2.X - Chemin.S1.X);

            double length = Math.Min(Bounds.Width, Bounds.Height)*0.8;

            // Calculer les coordonnées du centre du Canvas
            double centerX = Bounds.Width / 2;
            double centerY = Bounds.Height / 2;

            // Calculer les coordonnées du premier point de la ligne
            double x1 = centerX - (length / 2) * Math.Cos(angleRadians);
            double y1 = centerY - (length / 2) * Math.Sin(angleRadians);

            // Calculer les coordonnées du deuxième point de la ligne
            double x2 = centerX + (length / 2) * Math.Cos(angleRadians);
            double y2 = centerY + (length / 2) * Math.Sin(angleRadians);

            // Dessiner la ligne
            Pen pen = new Pen(Brushes.White, 4);
            context.DrawLine(pen, new Point(x1, y1), new Point(x2, y2));

        }

    }
}