using System.Windows.Input;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Data;

namespace DisneylandMap.Components;
public partial class TabChip : TemplatedControl
{
    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value.ToUpper());
    }

    private ICommand? _command;

    public ICommand? Command
    {
        get
        {
            return _command;
        }
        set
        {

            SetAndRaise(CommandProperty, ref _command, value);
        }
    }

    public object CommandParameter
    {
        get
        {
            return GetValue(CommandParameterProperty);
        }
        set
        {
            SetValue(CommandParameterProperty, value);
        }
    }

    // Property


    public static readonly StyledProperty<string> TitleProperty = AvaloniaProperty.Register<TabChip, string>(
        nameof(Title), "<Tab-Name>");

    public static readonly DirectProperty<TabChip, ICommand?> CommandProperty =
        AvaloniaProperty.RegisterDirect<TabChip, ICommand?>(
            nameof(Command),
            (TabChip button) => button.Command,
            delegate (TabChip button, ICommand? c) {
                button.Command = c;
            }, defaultBindingMode: BindingMode.OneWay, enableDataValidation: true);

    public static readonly StyledProperty<object> CommandParameterProperty = AvaloniaProperty.Register<TabChip, object>(nameof(CommandParameter));
}