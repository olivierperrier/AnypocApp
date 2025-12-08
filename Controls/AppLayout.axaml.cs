using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AnypocApp.Controls;

public partial class AppLayout : UserControl
{
    public static readonly StyledProperty<object?> PageContentProperty =
        AvaloniaProperty.Register<AppLayout, object?>(nameof(PageContent));

    public static readonly StyledProperty<string> PageTitleProperty =
        AvaloniaProperty.Register<AppLayout, string>(nameof(PageTitle), defaultValue: "Home");

    public static readonly StyledProperty<bool> IsWifiConnectedProperty =
        AvaloniaProperty.Register<AppLayout, bool>(nameof(IsWifiConnected), defaultValue: true);

    public static readonly StyledProperty<bool> IsEthernetConnectedProperty =
        AvaloniaProperty.Register<AppLayout, bool>(nameof(IsEthernetConnected), defaultValue: false);

    public static readonly StyledProperty<string> WifiSignalStrengthProperty =
        AvaloniaProperty.Register<AppLayout, string>(nameof(WifiSignalStrength), defaultValue: "75%");

    public static readonly StyledProperty<string> BottomLeftInfoProperty =
        AvaloniaProperty.Register<AppLayout, string>(nameof(BottomLeftInfo), defaultValue: "");

    public static readonly StyledProperty<string> BottomCenterInfoProperty =
        AvaloniaProperty.Register<AppLayout, string>(nameof(BottomCenterInfo), defaultValue: "");

    public static readonly StyledProperty<string> BottomRightInfoProperty =
        AvaloniaProperty.Register<AppLayout, string>(nameof(BottomRightInfo), defaultValue: "");

    public object? PageContent
    {
        get => GetValue(PageContentProperty);
        set => SetValue(PageContentProperty, value);
    }

    public string PageTitle
    {
        get => GetValue(PageTitleProperty);
        set => SetValue(PageTitleProperty, value);
    }

    public bool IsWifiConnected
    {
        get => GetValue(IsWifiConnectedProperty);
        set => SetValue(IsWifiConnectedProperty, value);
    }

    public bool IsEthernetConnected
    {
        get => GetValue(IsEthernetConnectedProperty);
        set => SetValue(IsEthernetConnectedProperty, value);
    }

    public string WifiSignalStrength
    {
        get => GetValue(WifiSignalStrengthProperty);
        set => SetValue(WifiSignalStrengthProperty, value);
    }

    public string BottomLeftInfo
    {
        get => GetValue(BottomLeftInfoProperty);
        set => SetValue(BottomLeftInfoProperty, value);
    }

    public string BottomCenterInfo
    {
        get => GetValue(BottomCenterInfoProperty);
        set => SetValue(BottomCenterInfoProperty, value);
    }

    public string BottomRightInfo
    {
        get => GetValue(BottomRightInfoProperty);
        set => SetValue(BottomRightInfoProperty, value);
    }

    public event EventHandler<RoutedEventArgs>? MenuButtonClicked;

    public AppLayout()
    {
        InitializeComponent();

        var topBar = this.FindControl<TopBar>("TopBar");
        if (topBar != null)
        {
            topBar.MenuButtonClicked += (s, e) => MenuButtonClicked?.Invoke(this, e);
        }
    }
}
