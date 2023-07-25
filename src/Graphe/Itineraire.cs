using System;
using System.Collections.Generic;

namespace DisneylandMap.src.Graphe
{
	public class Itineraire
	{
		public HashSet<Chemin> Parcours { get; set; } = new HashSet<Chemin>();

        public Sommet Depart { get; set; } = new Sommet();
        public Sommet Arrivee { get; set; } = new Sommet();

        public bool Accessible { get; set; } = true;

        public double FlotMax { get; set; }

        public double Distance
        {
            get
            {
                double d = 0;
                foreach (Chemin r in Parcours)
                {
                    d += r.Longueur;
                }
                return d;
            }
        }

        public string DistanceFormat
        {
            get
            {
                double d = Distance;
                int km = (int)(Distance / 1000);
                int m = (int)(Distance % 1000);

                if (km != 0)
                {
                    return Math.Round(Distance / 1000, 2) + "km";
                } else
                {
                    return m + "m";
                }
            }
        }

        public double Temps
        {
            get
            {
                double t = 0;
                foreach (Chemin r in Parcours)
                {
                    t += r.Temps;
                }
                return t;
            }
        }

        public string TempsFormat
        {
            get
            {
                int t = (int)Math.Round(Temps);
                int sec = t % 60;
                int min = t / 60;

                string format = "";

                if (min != 0) format += min + "min ";
                if (sec != 0) format += sec + "s";


                return (format == "") ? "(null)" : format;
            }
        }


        public override string ToString()
        {
            return $"{Depart} <-> {Arrivee}";
        }
    }
}

