using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace NoNameButtonGame
{
    internal static class Globals
    {
        public static readonly string SaveDirectory =
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/NoNameButtonGame/";
    }
}