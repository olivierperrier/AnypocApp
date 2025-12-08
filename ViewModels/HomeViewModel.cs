using CommunityToolkit.Mvvm.ComponentModel;

namespace AnypocApp.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _welcomeMessage = "Welcome to AnypocApp";

    [ObservableProperty]
    private string _userInfo = "User logged in successfully";
}
