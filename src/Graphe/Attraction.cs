using System;
namespace DisneylandMap.src.Graphe
{
	public class Attraction
	{
        public int Num { get; set; }
        public string Nom { get; set; } = "";

        public AttractionCategorie Categorie { get; set; }

        public override string ToString()
        {
            return $"({Num}) {Nom}";
        }

        public Attraction Clone()
        {
            return new Attraction
            {
                Num = Num,
                Nom = Nom,
                Categorie = Categorie,
            };
        }
    }

    public enum AttractionCategorie
    {
        MainStreet = 0,
        FrontierLand = 1,
        AdventureLand = 2,
        FantasyLand = 3,
        DiscoveryLand = 4,
    }
}

