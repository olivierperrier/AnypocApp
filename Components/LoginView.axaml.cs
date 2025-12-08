using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;

namespace AnypocApp.Components;

public partial class LoginView : UserControl
{
    private TextBox? _activeTextBox;

    public LoginView()
    {
        InitializeComponent();
        SetupKeyboard();
    }

    private void SetupKeyboard()
    {
        var keyboard = this.FindControl<VirtualKeyboard>("Keyboard");
        if (keyboard != null)
        {
            keyboard.DoneClicked += (s, e) => HideKeyboard();
        }
    }

    private void TextBox_GotFocus(object? sender, RoutedEventArgs e)
    {
        if (sender is TextBox textBox)
        {
            _activeTextBox = textBox;
            ShowKeyboard(textBox);
        }
    }

    private void TextBox_LostFocus(object? sender, RoutedEventArgs e)
    {
        // Don't hide keyboard immediately - let the Done button handle it
    }

    private void ShowKeyboard(TextBox targetTextBox)
    {
        var keyboard = this.FindControl<VirtualKeyboard>("Keyboard");
        if (keyboard != null)
        {
            keyboard.TargetControl = targetTextBox;
            keyboard.IsVisible = true;
        }
    }

    private void HideKeyboard()
    {
        var keyboard = this.FindControl<VirtualKeyboard>("Keyboard");
        if (keyboard != null)
        {
            keyboard.IsVisible = false;
            keyboard.TargetControl = null;
        }
        _activeTextBox = null;
    }

    private void Background_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        // Hide keyboard when clicking outside
        if (_activeTextBox != null)
        {
            var keyboard = this.FindControl<VirtualKeyboard>("Keyboard");
            if (keyboard != null && !IsPointerOverControl(e, keyboard))
            {
                HideKeyboard();
            }
        }
    }

    private bool IsPointerOverControl(PointerPressedEventArgs e, Control control)
    {
        var point = e.GetPosition(control);
        return point.X >= 0 && point.Y >= 0 &&
               point.X <= control.Bounds.Width && point.Y <= control.Bounds.Height;
    }

    private void EyeButton_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var passwordBox = this.FindControl<TextBox>("PasswordTextBox");
        if (passwordBox != null)
        {
            passwordBox.PasswordChar = '\0';
        }
    }

    private void EyeButton_PointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        var passwordBox = this.FindControl<TextBox>("PasswordTextBox");
        if (passwordBox != null)
        {
            passwordBox.PasswordChar = '•';
        }
    }

    private void EyeButton_PointerCaptureLost(object? sender, PointerCaptureLostEventArgs e)
    {
        var passwordBox = this.FindControl<TextBox>("PasswordTextBox");
        if (passwordBox != null)
        {
            passwordBox.PasswordChar = '•';
        }
    }
}
