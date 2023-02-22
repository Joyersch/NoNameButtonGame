using System;
using NoNameButtonGame.Cache;
using NoNameButtonGame.LogicObjects.Listener;

namespace NoNameButtonGame;

internal static class Globals
{
    // Directory save files
    public static readonly string SaveDirectory =
        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/NoNameButtonGame/";

    public static readonly SoundEffectsCache SoundEffects = new();

    public static SoundSettingsListener SoundSettingsListener;
}