using System;
using System.IO;
using System.Linq;
using System.Reflection;
using NoNameButtonGame.Cache;
using NoNameButtonGame.LogicObjects.Listener;

namespace NoNameButtonGame;

internal static class Globals
{
    // Directory save files
    public static readonly string SaveDirectory =
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/NoNameButtonGame/";

    public static readonly SoundEffectsCache SoundEffects = new();

    public static SoundSettingsListener SoundSettingsListener;

    public static string ReadFromResources(string file)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var cache = assembly.GetManifestResourceNames();
        if (!assembly.GetManifestResourceNames().Contains(file))
            throw new ArgumentException("Resource does not exists!");
        using Stream stream = assembly.GetManifestResourceStream(file);
        using StreamReader reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}