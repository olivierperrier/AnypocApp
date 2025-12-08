using Avalonia;
using Avalonia.Controls;

namespace AnypocApp.Controls;

public partial class BottomBar : UserControl
{
    public static readonly StyledProperty<string> LeftInfoProperty =
        AvaloniaProperty.Register<BottomBar, string>(nameof(LeftInfo), defaultValue: "");

    public static readonly StyledProperty<string> CenterInfoProperty =
        AvaloniaProperty.Register<BottomBar, string>(nameof(CenterInfo), defaultValue: "");

    public static readonly StyledProperty<string> RightInfoProperty =
        AvaloniaProperty.Register<BottomBar, string>(nameof(RightInfo), defaultValue: "");

    public string LeftInfo
    {
        get => GetValue(LeftInfoProperty);
        set => SetValue(LeftInfoProperty, value);
    }

    public string CenterInfo
    {
        get => GetValue(CenterInfoProperty);
        set => SetValue(CenterInfoProperty, value);
    }

    public string RightInfo
    {
        get => GetValue(RightInfoProperty);
        set => SetValue(RightInfoProperty, value);
    }

    public BottomBar()
    {
        InitializeComponent();
    }
}
