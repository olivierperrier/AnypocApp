using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AnypocApp.Controls;

public partial class TopBar : UserControl
{
    public static readonly StyledProperty<string> TitleProperty =
        AvaloniaProperty.Register<TopBar, string>(nameof(Title), defaultValue: "AnypocApp");

    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public event EventHandler<RoutedEventArgs>? MenuButtonClicked;

    public TopBar()
    {
        InitializeComponent();

        var menuButton = this.FindControl<Button>("BurgerMenuButton");
        if (menuButton != null)
        {
            menuButton.Click += (s, e) => MenuButtonClicked?.Invoke(this, e);
        }
    }
}
