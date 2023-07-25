using System;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using DisneylandMap.Views;
using DisneylandMap.src.MessageBus;
using System.Reactive.Linq;

namespace DisneylandMap
{
	public class MainWindowViewModel: ReactiveObject, IScreen
    {
        public RoutingState Router { get; } = new RoutingState();

        MessageBus ViewMessagesBus;

        public MainWindowViewModel()
		{
            ViewMessagesBus = new MessageBus();

            MessageSubscribe(ViewSwitcherMessageBus.Home);

            ViewMessagesBus.Listen<MessageBusViewSwitcher>().Subscribe((MessageBusViewSwitcher m) => MessageSubscribe(m.ViewType));
        }

        void MessageSubscribe(ViewSwitcherMessageBus m)
        {
            IRoutableViewModel? _view = null;
            switch(m)
            {
                case ViewSwitcherMessageBus.Home:
                    _view = new HomeViewModel(this, ViewMessagesBus);
                    break;

                case ViewSwitcherMessageBus.Map:
                    _view = new PlanViewModel(this, ViewMessagesBus);
                    break;

                case ViewSwitcherMessageBus.Densite:
                    _view = new DensiteViewModel(this, ViewMessagesBus);
                    break;
            }

            if(_view != null)
            {
                Router.Navigate.Execute(_view);
            }
            
        }

    }
}

