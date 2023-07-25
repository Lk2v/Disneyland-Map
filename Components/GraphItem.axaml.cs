using System.Windows.Input;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Media;

namespace DisneylandMap.Components;
public partial class GraphItem : TemplatedControl
{
    public string Nom
    {
        get => GetValue(NomProperty);
        set => SetValue(NomProperty, value);
    }

    public IBrush Couleur
    {
        get => GetValue(CouleurProperty);
        set => SetValue(CouleurProperty, value);
    }

    public bool Selectionner
    {
        get => GetValue(SelectionnerProperty);
        set => SetValue(SelectionnerProperty, value);
    }

    // Property

    public static readonly StyledProperty<string> NomProperty = AvaloniaProperty.Register<GraphItem, string>(
        nameof(Nom), "Sous-graphe");

    public static readonly StyledProperty<IBrush> CouleurProperty = AvaloniaProperty.Register<GraphItem, IBrush>(
        nameof(Couleur), Brushes.White);

    public static readonly StyledProperty<bool> SelectionnerProperty = AvaloniaProperty.Register<GraphItem, bool>(
        nameof(Selectionner), false);
}