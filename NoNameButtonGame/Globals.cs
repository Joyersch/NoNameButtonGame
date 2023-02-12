using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using NoNameButtonGame.Cache;

namespace NoNameButtonGame
{
    internal static class Globals
    {
        public static readonly string SaveDirectory =
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/NoNameButtonGame/";

        public static readonly TextureCache Textures = new();

        public static readonly SoundEffectsCache SoundEffects = new();
    }
}