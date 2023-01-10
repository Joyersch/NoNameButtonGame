using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace NoNameButtonGame
{
    static class Globals
    {
        public static ContentManager Content { get; set; }

        public static readonly string SaveDirectory =
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/NoNameButtonGame/";
    }
}