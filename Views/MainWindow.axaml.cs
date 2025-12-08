using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Media;
using Avalonia.Animation;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Styling;
using System;

namespace AnypocApp.Views;

public partial class MainWindow : Window
{
    private TextBox? _activeTextBox;
    private bool _isShiftActive = false;
    private System.Timers.Timer? _popupTimer;

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
            button.AddHandler(Button.PointerPressedEvent, KeyButton_PointerPressed, handledEventsToo: true);
        }
    }

    private void KeyButton_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (sender is not Button button)
            return;

        ShowKeyPopup(button);
    }

    private void ShowKeyPopup(Button button)
    {
        // Get the button content
        var content = button.Content?.ToString();
        var tag = button.Tag?.ToString();

        // Determine what to display
        string displayText = content ?? "";
        if (tag == "backspace")
            displayText = "⌫";
        else if (tag == "shift")
            displayText = "⇧";
        else if (tag == "space")
            displayText = "Space";
        else if (tag == "done")
            displayText = "✓";

        // Set the popup text
        KeyPressText.Text = displayText;

        // Calculate position above the button
        var buttonBounds = button.Bounds;
        var buttonPosition = button.TranslatePoint(new Point(0, 0), this);

        if (buttonPosition.HasValue)
        {
            // Center the popup above the button
            var popupWidth = 60.0;
            var popupHeight = 70.0;

            var left = buttonPosition.Value.X + (buttonBounds.Width / 2) - (popupWidth / 2);
            var top = buttonPosition.Value.Y - popupHeight - 10;

            // Ensure popup stays within window bounds
            left = Math.Max(5, Math.Min(left, this.Bounds.Width - popupWidth - 5));
            top = Math.Max(5, top);

            Canvas.SetLeft(KeyPressPopup, left);
            Canvas.SetTop(KeyPressPopup, top);
        }

        // Show popup
        KeyPressPopup.IsVisible = true;

        // Hide after 150ms
        _popupTimer?.Stop();
        _popupTimer = new System.Timers.Timer(150);
        _popupTimer.Elapsed += (s, e) =>
        {
            Avalonia.Threading.Dispatcher.UIThread.Post(() =>
            {
                KeyPressPopup.IsVisible = false;
                _popupTimer?.Dispose();
                _popupTimer = null;
            });
        };
        _popupTimer.AutoReset = false;
        _popupTimer.Start();
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