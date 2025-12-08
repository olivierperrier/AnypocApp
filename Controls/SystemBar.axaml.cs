using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;

namespace AnypocApp.Controls;

public partial class SystemBar : UserControl
{
    public static readonly StyledProperty<bool> IsWifiConnectedProperty =
        AvaloniaProperty.Register<SystemBar, bool>(nameof(IsWifiConnected), defaultValue: true);

    public static readonly StyledProperty<bool> IsEthernetConnectedProperty =
        AvaloniaProperty.Register<SystemBar, bool>(nameof(IsEthernetConnected), defaultValue: false);

    public static readonly StyledProperty<string> WifiSignalStrengthProperty =
        AvaloniaProperty.Register<SystemBar, string>(nameof(WifiSignalStrength), defaultValue: "75%");

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

    private DispatcherTimer? _timer;

    public SystemBar()
    {
        InitializeComponent();
        StartClock();
    }

    private void StartClock()
    {
        UpdateTime();
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        _timer.Tick += (s, e) => UpdateTime();
        _timer.Start();
    }

    private void UpdateTime()
    {
        var timeText = this.FindControl<TextBlock>("TimeText");
        var dateText = this.FindControl<TextBlock>("DateText");

        if (timeText != null)
        {
            timeText.Text = DateTime.Now.ToString("HH:mm");
        }

        if (dateText != null)
        {
            dateText.Text = DateTime.Now.ToString("ddd, MMM d");
        }
    }
}
