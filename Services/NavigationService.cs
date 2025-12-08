using System;
using CommunityToolkit.Mvvm.ComponentModel;
using AnypocApp.ViewModels;

namespace AnypocApp.Services;

public class NavigationService : ObservableObject
{
    private ViewModelBase? _currentView;

    public ViewModelBase? CurrentView
    {
        get => _currentView;
        private set => SetProperty(ref _currentView, value);
    }

    public void NavigateTo<T>() where T : ViewModelBase, new()
    {
        CurrentView = new T();
    }

    public void NavigateTo(ViewModelBase viewModel)
    {
        CurrentView = viewModel;
    }
}
