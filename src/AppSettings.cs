using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RxT;

internal enum AppTheme
{
    System,
    Light,
    Dark,
}

/// <summary>Benutzereinstellungen, gespeichert als JSON neben der EXE (portabel).</summary>
internal sealed class AppSettings
{
    private static readonly string FilePath = Path.Combine(AppContext.BaseDirectory, "RxT.settings.json");

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter(null, allowIntegerValues: false) },
    };

    public AppTheme Theme { get; set; } = AppTheme.System;

    public Language Language { get; set; } = Language.English;

    public static AppSettings Load()
    {
        try
        {
            return JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(FilePath), JsonOptions) ?? new AppSettings();
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or JsonException)
        {
            return new AppSettings();
        }
    }

    public void Save()
    {
        try
        {
            File.WriteAllText(FilePath, JsonSerializer.Serialize(this, JsonOptions));
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            // Einstellungen sind nicht kritisch (z. B. schreibgeschütztes Verzeichnis).
        }
    }
}
