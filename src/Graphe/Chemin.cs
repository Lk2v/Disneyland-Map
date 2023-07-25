using System;
using Avalonia;
using Avalonia.Media;

namespace DisneylandMap.src.Graphe
{
    public class Chemin
    {
        public int I { get; set; }
        public Sommet S1 { get; set; } = new Sommet();
        public Sommet S2 { get; set; } = new Sommet();

        public double Densite { get; set; }
        public double Longueur { get; set; } // en m

        public double LongueurCalc {
            get => EstSature ? double.MaxValue : Longueur;
        }

        public bool EstSature
        {
            get => Densite == 1;
        }

        public string LongueurFormat
        {
            get => Math.Round(Longueur) + "m"; // on considere que les visiteurs vont a 1,25m/s (4,5km/h moyenne)
        }

        // En s
        public double Temps
        {
            get => Longueur / 1.5; // on considere que les visiteurs vont a 1,5m/s (5,4km/h moyenne)
        }

        public string TempsParcoursFormat
        {
            get
            {
                int t = (int) Math.Round(Temps);
                int sec = t % 60;
                int min = t / 60;

                string format = "";

                if (min != 0) format += min + "min ";
                if (sec != 0) format += sec + "s";


                return (format == "") ? "(null)" : format;
            }
        }

        // Méthode pour calculer la distance entre un point et une ligne
        public double DistancePoint(Point point)
        {
            double dx = S2.X - S1.X;
            double dy = S2.Y - S1.Y;

            double u = ((point.X - S1.X) * dx + (point.Y - S1.Y) * dy) / (dx * dx + dy * dy);

            if (u > 1)
            {
                u = 1;
            }
            else if (u < 0)
            {
                u = 0;
            }

            double x = S1.X + u * dx;
            double y = S1.Y + u * dy;

            return Math.Sqrt((point.X - x) * (point.X - x) + (point.Y - y) * (point.Y - y));
        }

        public SolidColorBrush CouleurDensite()
        {
            return CouleurDensite(Densite);
        }

        public static SolidColorBrush CouleurDensite(double val)
        {
            Color color = new Color();

            if (val <= 0.5)
            {
                // Blanc vers Orange
                byte r = (byte)(255 * (2 * val));
                byte g = 255;
                byte b = 0;
                color = Color.FromRgb(r, g, b);
            }
            else
            {
                // Orange vers Rouge
                byte r = 255;
                byte g = (byte)(255 - 255 * (2 * (val - 0.5)));
                byte b = 0;
                color = Color.FromRgb(r, g, b);
            }

            return new SolidColorBrush(color);
        }

        public bool Contient(Sommet s)
        {
            return (s.Id == S1.Id || s.Id == S2.Id);
        }

        public bool Contient(Sommet s1, Sommet s2)
        {
            return (s1.Id == S1.Id && s2.Id == S2.Id) || (s2.Id == S1.Id && s1.Id == S2.Id);
        }

        public string ToCSV()
        {
            return $"{S1.Id};{S2.Id};{Densite}";
        }
        public override string ToString()
        {
            return $"Chemin ({Longueur}m)\n" +
                $"S1 = {S1}\n" +
                $"S2 = {S2}\n" +
                $"Densité = {Densite}";
        }
    }
}

