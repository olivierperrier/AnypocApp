using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using AnypocApp.Models;

namespace AnypocApp.Services;

public class KeyboardLayoutService
{
    private static readonly Dictionary<string, KeyboardLayout> _layouts = new();
    private static bool _isInitialized = false;

    public static void Initialize()
    {
        if (_isInitialized) return;

        try
        {
            // First, try to load from file system
            var layoutsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Keyboards");

            if (Directory.Exists(layoutsPath))
            {
                var jsonFiles = Directory.GetFiles(layoutsPath, "*.json");

                foreach (var file in jsonFiles)
                {
                    try
                    {
                        var json = File.ReadAllText(file);
                        LoadLayout(json);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error loading keyboard layout from {file}: {ex.Message}");
                    }
                }
            }

            // If no layouts loaded, use embedded fallback
            if (_layouts.Count == 0)
            {
                LoadEmbeddedQwertyLayout();
            }

            _isInitialized = true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error initializing keyboard layouts: {ex.Message}");
            LoadEmbeddedQwertyLayout();
            _isInitialized = true;
        }
    }

    private static void LoadLayout(string json)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var layout = JsonSerializer.Deserialize<KeyboardLayout>(json, options);

        if (layout != null && !string.IsNullOrEmpty(layout.LanguageCode))
        {
            _layouts[layout.LanguageCode] = layout;
            System.Diagnostics.Debug.WriteLine($"Loaded keyboard layout: {layout.Name} ({layout.LanguageCode})");
        }
    }

    private static void LoadEmbeddedQwertyLayout()
    {
        try
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "AnypocApp.Assets.Keyboards.qwerty.json";

            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                throw new InvalidOperationException($"Embedded resource not found: {resourceName}");
            }

            using var reader = new StreamReader(stream);
            var json = reader.ReadToEnd();
            LoadLayout(json);

            System.Diagnostics.Debug.WriteLine("Loaded embedded QWERTY keyboard layout");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading embedded keyboard layout: {ex.Message}");
            throw;
        }
    }

    public static KeyboardLayout GetLayout(string languageCode)
    {
        if (!_isInitialized) Initialize();

        if (_layouts.TryGetValue(languageCode, out var layout))
        {
            return layout;
        }

        // Fallback to first available layout (should be at least QWERTY)
        return _layouts.Values.FirstOrDefault()
            ?? throw new InvalidOperationException("No keyboard layouts available");
    }

    public static List<string> GetAvailableLanguages()
    {
        if (!_isInitialized) Initialize();
        return _layouts.Keys.ToList();
    }

    public static Dictionary<string, string> GetAvailableLayoutsWithNames()
    {
        if (!_isInitialized) Initialize();
        return _layouts.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Name);
    }
}
