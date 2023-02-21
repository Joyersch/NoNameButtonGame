using System;
using NoNameButtonGame.Cache;
using NoNameButtonGame.LogicObjects.Linker;

namespace NoNameButtonGame
{
    internal static class Globals
    {
        public static readonly string SaveDirectory =
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/NoNameButtonGame/";

        public static readonly TextureCache Textures = new();

        public static readonly SoundEffectsCache SoundEffects = new();

        public static SoundSettingsLinker SoundSettingsLinker;
    }
}