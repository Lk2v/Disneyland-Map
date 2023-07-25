using System;
using System.ComponentModel;
using System.Net.Sockets;

namespace DisneylandMap.src.Graphe
{
    /*
     * Un sommet peut être une attraction ou non :
     *      - Un sommet qui est une attraction a sa propriété Attraction attribué
     *      - Un sommet qui est juste un point de liaison entre arrête a la propriété Attraction à null
     */
    public class Sommet : INotifyPropertyChanged
    {
        public int Id { get; set; } // n° du sommet utile comme identifiant
        public int I { get; set; }

        public double X { get; set; }
        public double Y { get; set; }

        public double Distance { get; set; }

        public bool Visite { get; set; }
        public Sommet? Pred { get; set; } = null;

        public Attraction? Attraction { get; set; } = null;

        public bool EstAttraction
        {
            get => Attraction != null;
        }

        public override string ToString()
        {
            return $"Sommet n°{Id} - " + (EstAttraction ? Attraction!.ToString() : "Point de liaison");
        }

        public Sommet Clone()
        {
            return new Sommet
            {

                Id=Id,
                I=I,

                X=X,
                Y=Y,

                Distance=Distance,
                Pred = Pred,
                Visite=Visite,

                Attraction= EstAttraction ? Attraction!.Clone() : null,
            };
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

