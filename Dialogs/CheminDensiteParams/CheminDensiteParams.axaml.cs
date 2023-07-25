using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using ReactiveUI;
using System;

using Avalonia;
using System.Reactive.Linq;
using DisneylandMap.src.Graphe;

namespace DisneylandMap.Dialogs
{
    public partial class CheminDensiteParamsWindow : ReactiveWindow<CheminDensiteParamsViewModel>
    {
        public CheminDensiteParamsWindow()
        {
            InitializeComponent();

            #if DEBUG
            this.AttachDevTools();
#endif
        }

        public CheminDensiteParamsWindow(Chemin c) : this()
        {
            ViewModel = new CheminDensiteParamsViewModel(c);
            this.WhenActivated(d => {
                d(ViewModel.Submit.Subscribe(Close));
            });
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
