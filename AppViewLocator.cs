using System;
using ReactiveUI;
using DisneylandMap.Views;
using Microsoft.Win32;

namespace DisneylandMap
{
    public class AppViewLocator : IViewLocator
    {
        public IViewFor ResolveView<T>(T viewModel, string? contract = null) => viewModel switch
        {
            HomeViewModel context => new HomeView { DataContext = context },
            PlanViewModel context => new PlanView { DataContext = context },
            DensiteViewModel context => new DensiteView { DataContext = context },

            _ => throw new ArgumentOutOfRangeException(nameof(viewModel))
        };
    }
}

