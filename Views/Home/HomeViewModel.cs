using System;
using System.Reactive;
using ReactiveUI;
using DisneylandMap.src.MessageBus;
namespace DisneylandMap.Views
{
    public class HomeViewModel : ReactiveObject, IRoutableViewModel
    {
        // Reference to IScreen that owns the routable view model.
        public IScreen HostScreen { get; }

        // Unique identifier for the routable view model.
        public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

        public ReactiveCommand<ViewSwitcherMessageBus, Unit> Navigation { get; }

        IMessageBus Bus;

        public HomeViewModel(IScreen screen, IMessageBus b)
        {
            HostScreen = screen;
            Bus = b;

            Navigation = ReactiveCommand.Create<ViewSwitcherMessageBus>(Selection);
        }

        void Selection(ViewSwitcherMessageBus nav)
        {
            Bus.SendMessage(new MessageBusViewSwitcher(nav));
        }
    }
}

