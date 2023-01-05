using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
namespace NoNameButtonGame
{
    static class Globals
    {
        public static ContentManager Content { get; set; }

        public static class Settings
        {
            public static bool IsFixedStep { get; set; }
            public static bool IsFullscreen { get; set; }
        }
        
        public static class GameData
        {
            public static int MaxLevel { get; set; }
        }
    }
}
