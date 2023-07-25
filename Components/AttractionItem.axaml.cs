using System.Windows.Input;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Data;

namespace DisneylandMap.Components;
public partial class AttractionItem : TemplatedControl
{
    public string Num
    {
        get => GetValue(NumProperty);
        set => SetValue(NumProperty, value);
    }

    public string Nom
    {
        get => GetValue(NomProperty);
        set => SetValue(NomProperty, value);
    }

    public bool Selectionner
    {
        get => GetValue(SelectionnerProperty);
        set => SetValue(SelectionnerProperty, value);
    }

    // Property
    public static readonly StyledProperty<string> NumProperty = AvaloniaProperty.Register<AttractionItem, string>(
        nameof(Num), "");

    public static readonly StyledProperty<string> NomProperty = AvaloniaProperty.Register<AttractionItem, string>(
        nameof(Nom), "Selectionner...");

    public static readonly StyledProperty<bool> SelectionnerProperty = AvaloniaProperty.Register<AttractionItem, bool>(
        nameof(Selectionner), false);
}