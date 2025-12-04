using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using System.Linq;

namespace AnypocApp.Views;

public partial class MainWindow : Window
{
    private TextBox? _activeTextBox;
    private bool _isShiftActive = false;

    public MainWindow()
    {
        InitializeComponent();
        AttachKeyboardHandlers();
    }

    private void AttachKeyboardHandlers()
    {
        // Find all keyboard buttons and attach click handlers
        var keyButtons = KeyboardPanel.GetLogicalDescendants()
            .OfType<Button>()
            .Where(b => b.Classes.Contains("key"));

        foreach (var button in keyButtons)
        {
            button.Click += KeyButton_Click;
        }
    }

    private void TextBox_GotFocus(object? sender, RoutedEventArgs e)
    {
        _activeTextBox = sender as TextBox;
        KeyboardPanel.IsVisible = true;
    }

    private void TextBox_LostFocus(object? sender, RoutedEventArgs e)
    {
        // Use a small delay to allow keyboard button clicks to register first
        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            // Check if focus went to another textbox
            if (this.FocusManager?.GetFocusedElement() is TextBox)
                return;

            // Check if focus went to a keyboard button
            if (this.FocusManager?.GetFocusedElement() is Button btn && btn.Classes.Contains("key"))
                return;

            // Otherwise hide the keyboard
            KeyboardPanel.IsVisible = false;
            _activeTextBox = null;
        }, Avalonia.Threading.DispatcherPriority.Background);
    }

    private void KeyButton_Click(object? sender, RoutedEventArgs e)
    {
        if (_activeTextBox == null || sender is not Button button)
            return;

        var tag = button.Tag?.ToString();
        var content = button.Content?.ToString();

        if (tag == "done")
        {
            KeyboardPanel.IsVisible = false;
            _activeTextBox = null;
            // Remove focus from textbox
            this.Focus();
            return;
        }

        if (tag == "backspace")
        {
            var text = _activeTextBox.Text ?? "";
            if (text.Length > 0)
            {
                _activeTextBox.Text = text.Substring(0, text.Length - 1);
                _activeTextBox.CaretIndex = _activeTextBox.Text.Length;
            }
            return;
        }

        if (tag == "shift")
        {
            _isShiftActive = !_isShiftActive;
            return;
        }

        if (tag == "space")
        {
            _activeTextBox.Text += " ";
            _activeTextBox.CaretIndex = _activeTextBox.Text.Length;
            return;
        }

        if (!string.IsNullOrEmpty(content))
        {
            var textToAdd = _isShiftActive ? content : content.ToLower();
            _activeTextBox.Text += textToAdd;
            _activeTextBox.CaretIndex = _activeTextBox.Text.Length;

            if (_isShiftActive)
                _isShiftActive = false;
        }
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

    private void Background_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        // If keyboard is visible and click is not on a textbox or keyboard button, hide keyboard
        if (KeyboardPanel.IsVisible)
        {
            var clickedElement = e.Source;

            // Check if we clicked on a TextBox or keyboard button
            if (clickedElement is TextBox ||
                (clickedElement is Button btn && btn.Classes.Contains("key")))
            {
                return;
            }

            // Hide keyboard and remove focus
            KeyboardPanel.IsVisible = false;
            _activeTextBox = null;
            this.Focus();
        }
    }
}