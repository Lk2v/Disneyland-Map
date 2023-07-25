using System.Windows.Input;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using DisneylandMap.src.Graphe;

namespace DisneylandMap.Components;
public partial class ItineraireInfo : TemplatedControl
{
    public string Num1
    {
        get => GetValue(TempsProperty);
        set => SetValue(TempsProperty, value);
    }

    public string Num2
    {
        get => GetValue(TempsProperty);
        set => SetValue(TempsProperty, value);
    }

    public string Temps
    {
        get => GetValue(TempsProperty);
        set => SetValue(TempsProperty, value);
    }

    public string Longueur
    {
        get => GetValue(LongueurProperty);
        set => SetValue(LongueurProperty, value);
    }

    public string FlotMax
    {
        get => GetValue(FlotMaxProperty);
        set => SetValue(FlotMaxProperty, value);
    }

    // Property
    public static readonly StyledProperty<string> Num1Property = AvaloniaProperty.Register<ItineraireInfo, string>(
        nameof(Num1), "0");

    public static readonly StyledProperty<string> Num2Property = AvaloniaProperty.Register<ItineraireInfo, string>(
        nameof(Num2), "0");

    public static readonly StyledProperty<string> TempsProperty = AvaloniaProperty.Register<ItineraireInfo, string>(
        nameof(Temps), "0");

    public static readonly StyledProperty<string> LongueurProperty = AvaloniaProperty.Register<ItineraireInfo, string>(
        nameof(Longueur), "0");

    public static readonly StyledProperty<string> FlotMaxProperty = AvaloniaProperty.Register<ItineraireInfo, string>(
        nameof(FlotMax), "0");

}