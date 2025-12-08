using System.Collections.Generic;

namespace AnypocApp.Models;

public class KeyboardLayout
{
    public string Name { get; set; } = string.Empty;
    public string LanguageCode { get; set; } = string.Empty;
    public List<KeyboardRow> Rows { get; set; } = new();
}

public class KeyboardRow
{
    public List<KeyboardKey> Keys { get; set; } = new();
}

public class KeyboardKey
{
    public string Display { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string? ShiftDisplay { get; set; }
    public string? ShiftValue { get; set; }
    public string Type { get; set; } = "normal"; // normal, special, space, done
    public double Width { get; set; } = 1.0; // Relative width multiplier
}
