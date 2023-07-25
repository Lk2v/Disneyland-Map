using System;
namespace DisneylandMap.src.MessageBus
{

    public class MessageBusViewSwitcher
    {
        public MessageBusViewSwitcher(ViewSwitcherMessageBus viewType)
        {
            ViewType = viewType;
        }

        public ViewSwitcherMessageBus ViewType { get; }
    }

    public enum ViewSwitcherMessageBus
	{
		Home,
		Map,
        Densite,
    }
}

