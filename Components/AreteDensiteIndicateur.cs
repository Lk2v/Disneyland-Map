using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Media;
using DisneylandMap.src.Graphe;

namespace DisneylandMap.Components;
public partial class AreteDensiteIndicateur : UserControl
{
    public double Densite
    {
        get => GetValue(DensiteProperty);
        set =>SetValue(DensiteProperty, value);
    }

    public static readonly StyledProperty<double> TailleProperty =
            AvaloniaProperty.Register<AreteItem, double>(nameof(Taille), 25.0);

    public double Taille
    {
        get { return GetValue(TailleProperty); }
        set
        {
            SetValue(TailleProperty, value);
            UpdateSize();
        }
    }

    // Property

    public static readonly StyledProperty<double> DensiteProperty = AvaloniaProperty.Register<AttractionItem, double>(
        nameof(Densite), -1);

    public AreteDensiteIndicateur()
    {
        this.GetObservable(DensiteProperty).Subscribe(_ => InvalidateVisual());
        UpdateSize();
    }

    private void UpdateSize()
    {
        Width = Taille;
        Height = Taille;
    }

    public override void Render(DrawingContext ctx) {
        
       ctx.DrawEllipse(Chemin.CouleurDensite(Densite), null, new Point(Width/2, Height/2), Width/2, Height/2);

        var text = new FormattedText
        {
            Text = Math.Round(Densite * 100).ToString(),
            Typeface = new Typeface("Arial", FontStyle.Normal, FontWeight.Bold),
            TextAlignment = TextAlignment.Center,
            FontSize = Taille / 2,
        };


        ctx.DrawText(Brushes.White, new Point((Width - text.Bounds.Width) / 2, (Height - text.Bounds.Height) / 2), text);
        
    }
}