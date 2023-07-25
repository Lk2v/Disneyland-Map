using System;
using System.Data.Common;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace DisneylandMap.Components;

public partial class NavButton : ContentControl
{
    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
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

            this.SetAndRaise(CommandProperty, ref _command, value);
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


    public static readonly StyledProperty<string> TitleProperty = AvaloniaProperty.Register<NavButton, string>(
        nameof(Title), "<Tab-Name>");

    public static readonly DirectProperty<NavButton, ICommand?> CommandProperty =
        AvaloniaProperty.RegisterDirect<NavButton, ICommand?>(
            nameof(Command),
            (NavButton button) => button.Command,
            delegate (NavButton button, ICommand? c) {
                button.Command = c;
            }, defaultBindingMode: BindingMode.OneWay, enableDataValidation: true);

    public static readonly StyledProperty<object> CommandParameterProperty = AvaloniaProperty.Register<NavButton, object>(nameof(CommandParameter));
}