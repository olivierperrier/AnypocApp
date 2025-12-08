using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using AnypocApp.Models;
using AnypocApp.Services;

namespace AnypocApp.Components;

public partial class VirtualKeyboard : UserControl
{
    public static readonly StyledProperty<Control?> TargetControlProperty =
        AvaloniaProperty.Register<VirtualKeyboard, Control?>(nameof(TargetControl));

    public static readonly StyledProperty<string> LanguageCodeProperty =
        AvaloniaProperty.Register<VirtualKeyboard, string>(nameof(LanguageCode), defaultValue: "en-US");

    public static readonly StyledProperty<bool> ShowKeyPressPopupProperty =
        AvaloniaProperty.Register<VirtualKeyboard, bool>(nameof(ShowKeyPressPopup), defaultValue: true);

    public Control? TargetControl
    {
        get => GetValue(TargetControlProperty);
        set => SetValue(TargetControlProperty, value);
    }

    public string LanguageCode
    {
        get => GetValue(LanguageCodeProperty);
        set => SetValue(LanguageCodeProperty, value);
    }

    public bool ShowKeyPressPopup
    {
        get => GetValue(ShowKeyPressPopupProperty);
        set => SetValue(ShowKeyPressPopupProperty, value);
    }

    public event EventHandler<RoutedEventArgs>? DoneClicked;

    private bool _isShiftActive = false;
    private KeyboardLayout? _currentLayout;
    private Button? _shiftButton;

    public VirtualKeyboard()
    {
        InitializeComponent();
        PropertyChanged += OnPropertyChanged;
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        LoadKeyboardLayout();
    }

    private void OnPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property == LanguageCodeProperty)
        {
            LoadKeyboardLayout();
        }
    }

    private void LoadKeyboardLayout()
    {
        try
        {
            KeyboardLayoutService.Initialize();
            _currentLayout = KeyboardLayoutService.GetLayout(LanguageCode);
            BuildKeyboard();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading keyboard layout: {ex.Message}");
        }
    }

    private void BuildKeyboard()
    {
        var container = this.FindControl<StackPanel>("KeyboardContainer");
        if (container == null || _currentLayout == null) return;

        container.Children.Clear();

        foreach (var row in _currentLayout.Rows)
        {
            var rowGrid = new Grid { HorizontalAlignment = HorizontalAlignment.Stretch };

            // Build column definitions based on key widths
            var columnDefs = new ColumnDefinitions();
            foreach (var key in row.Keys)
            {
                columnDefs.Add(new ColumnDefinition(key.Width, GridUnitType.Star));
            }
            rowGrid.ColumnDefinitions = columnDefs;

            // Add keys to the row
            for (int i = 0; i < row.Keys.Count; i++)
            {
                var key = row.Keys[i];
                var button = CreateKeyButton(key);
                Grid.SetColumn(button, i);
                rowGrid.Children.Add(button);
            }

            container.Children.Add(rowGrid);
        }
    }

    private Button CreateKeyButton(KeyboardKey key)
    {
        var button = new Button
        {
            Content = key.Display,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Classes = { "key" }
        };

        // Add type-specific classes
        switch (key.Type.ToLower())
        {
            case "special":
            case "shift":
            case "backspace":
                button.Classes.Add("special");
                break;
            case "done":
                button.Classes.Add("done");
                break;
            case "space":
                button.Classes.Add("space");
                break;
            case "empty":
                button.Classes.Add("empty");
                break;
        }

        // Store key data
        button.Tag = key;

        // Track shift button reference
        if (key.Type == "shift")
        {
            _shiftButton = button;
        }

        button.Click += OnKeyButtonClick;
        button.AddHandler(Button.PointerPressedEvent, OnKeyButtonPointerPressed, handledEventsToo: true);
        button.AddHandler(Button.PointerReleasedEvent, OnKeyButtonPointerReleased, handledEventsToo: true);

        return button;
    }

    private void OnKeyButtonPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (!ShowKeyPressPopup) return;
        if (sender is not Button button || button.Tag is not KeyboardKey key) return;

        ShowPopup(button, key);
    }

    private void OnKeyButtonPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        HidePopup();
    }

    private void ShowPopup(Button button, KeyboardKey key)
    {
        var popup = this.FindControl<Border>("KeyPressPopup");
        var popupText = this.FindControl<TextBlock>("KeyPressText");

        if (popup == null || popupText == null) return;

        // Determine what to display
        string displayText = key.Display;

        if (key.Type == "backspace")
            displayText = "⌫";
        else if (key.Type == "shift")
            displayText = "⇧";
        else if (key.Type == "space")
            displayText = "Space";
        else if (key.Type == "done")
            displayText = "✓";
        else if (_isShiftActive && !string.IsNullOrEmpty(key.ShiftDisplay))
            displayText = key.ShiftDisplay;

        popupText.Text = displayText;

        // Calculate position above the button
        var buttonBounds = button.Bounds;
        var buttonPosition = button.TranslatePoint(new Point(0, 0), this);

        if (buttonPosition.HasValue)
        {
            var popupWidth = 60.0;
            var popupHeight = 70.0;

            var left = buttonPosition.Value.X + (buttonBounds.Width / 2) - (popupWidth / 2);
            var top = buttonPosition.Value.Y - popupHeight - 8;

            // Ensure popup stays within control bounds horizontally
            left = Math.Max(5, Math.Min(left, this.Bounds.Width - popupWidth - 5));

            // Allow negative top values so popup can go above the keyboard control
            // The Canvas has ClipToBounds="False" so it can render outside

            Canvas.SetLeft(popup, left);
            Canvas.SetTop(popup, top);
        }

        popup.IsVisible = true;
    }

    private void HidePopup()
    {
        var popup = this.FindControl<Border>("KeyPressPopup");
        if (popup != null)
        {
            popup.IsVisible = false;
        }
    }

    private void OnKeyButtonClick(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button button || button.Tag is not KeyboardKey key) return;

        switch (key.Type.ToLower())
        {
            case "shift":
                ToggleShift();
                break;
            case "backspace":
                HandleBackspace();
                break;
            case "space":
                InsertText(key.Value);
                break;
            case "done":
                DoneClicked?.Invoke(this, e);
                break;
            case "empty":
                // Do nothing
                break;
            default:
                HandleNormalKey(key);
                break;
        }

        e.Handled = true;
    }

    private void HandleNormalKey(KeyboardKey key)
    {
        string textToInsert;

        if (_isShiftActive && !string.IsNullOrEmpty(key.ShiftValue))
        {
            textToInsert = key.ShiftValue;
        }
        else
        {
            textToInsert = key.Value;
        }

        InsertText(textToInsert);

        if (_isShiftActive)
        {
            _isShiftActive = false;
            UpdateShiftButton();
        }
    }

    private void ToggleShift()
    {
        _isShiftActive = !_isShiftActive;
        UpdateShiftButton();
    }

    private void UpdateShiftButton()
    {
        if (_shiftButton == null) return;

        if (_isShiftActive)
        {
            _shiftButton.Classes.Add("shift-active");
        }
        else
        {
            _shiftButton.Classes.Remove("shift-active");
        }
    }

    private void HandleBackspace()
    {
        if (TargetControl is TextBox textBox)
        {
            if (!string.IsNullOrEmpty(textBox.Text) && textBox.CaretIndex > 0)
            {
                var caretIndex = textBox.CaretIndex;
                textBox.Text = textBox.Text.Remove(caretIndex - 1, 1);
                textBox.CaretIndex = caretIndex - 1;
            }
        }
    }

    private void InsertText(string text)
    {
        if (TargetControl is TextBox textBox)
        {
            var caretIndex = textBox.CaretIndex;
            var currentText = textBox.Text ?? string.Empty;
            textBox.Text = currentText.Insert(caretIndex, text);
            textBox.CaretIndex = caretIndex + text.Length;
        }
    }

    public void SetLanguage(string languageCode)
    {
        LanguageCode = languageCode;
    }
}
