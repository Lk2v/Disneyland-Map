using System.Windows.Input;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Data;

namespace DisneylandMap.Components;
public partial class AttractionNum : TemplatedControl
{
    public string Num
    {
        get => GetValue(NumProperty);
        set => SetValue(NumProperty, value);
    }


    // Property
    public static readonly StyledProperty<string> NumProperty = AvaloniaProperty.Register<AttractionNum, string>(
        nameof(Num), "");
}