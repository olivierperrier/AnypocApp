using Avalonia.Controls;
using Avalonia.Input;

namespace AnypocApp.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void EyeButton_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        PasswordTextBox.PasswordChar = '\0';
    }

    private void EyeButton_PointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        PasswordTextBox.PasswordChar = '•';
    }

    private void EyeButton_PointerCaptureLost(object? sender, PointerCaptureLostEventArgs e)
    {
        PasswordTextBox.PasswordChar = '•';
    }
}