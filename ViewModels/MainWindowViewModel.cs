using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AnypocApp.Services;

namespace AnypocApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly NavigationService _navigationService;

    [ObservableProperty]
    private string _username = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private bool _isLoggedIn = false;

    [ObservableProperty]
    private ViewModelBase? _currentView;

    [ObservableProperty]
    private string _searchText = string.Empty;

    public MainWindowViewModel()
    {
        _navigationService = new NavigationService();
        _navigationService.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(NavigationService.CurrentView))
            {
                CurrentView = _navigationService.CurrentView;

                // Sync SearchText with HomeViewModel if current view is HomeViewModel
                if (CurrentView is HomeViewModel homeViewModel)
                {
                    homeViewModel.PropertyChanged += (s, e) =>
                    {
                        if (e.PropertyName == nameof(HomeViewModel.SearchText))
                        {
                            SearchText = homeViewModel.SearchText;
                        }
                    };

                    this.PropertyChanged += (s, e) =>
                    {
                        if (e.PropertyName == nameof(SearchText) && CurrentView is HomeViewModel hvm)
                        {
                            hvm.SearchText = SearchText;
                        }
                    };
                }
            }
        };
    }

    [RelayCommand]
    private void Login()
    {
        // Simple login validation (replace with real authentication)
        if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
        {
            System.Diagnostics.Debug.WriteLine($"Login attempt - Username: {Username}");

            // Navigate to Home screen after successful login
            IsLoggedIn = true;
            _navigationService.NavigateTo<HomeViewModel>();
        }
    }
}
