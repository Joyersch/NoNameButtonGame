using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;

namespace NoNameButtonGame.Hitboxes
{
    public struct HitboxMap
    {
        public Texture2D Texture;
        public Vector2 Imagesize;
        public Rectangle[] Hitbox;
        public int Aniframes;
        public bool? AniFromTop;
        
    }
    public static class Mapping
    {
        
        public static HitboxMap GetTHBox(this ContentManager contentManager, string textureName) {
            //I dont now if this is a good way to do this, it works which is fine my me.
            HitboxMap map = new HitboxMap {
                Texture = contentManager.Load<Texture2D>(textureName)
            };
            switch (textureName) {
                case "awesomebutton":
                    map.Hitbox = new Rectangle[2] { new Rectangle(2,1, 28,14),
                new Rectangle(1,2,30,12) };
                    map.Imagesize = new Vector2(32, 16);
                    break;
                case "startbutton":
                    map.Hitbox = new Rectangle[2] { new Rectangle(2,1, 44,14),
                new Rectangle(1,2,46,12)  };
                    map.Imagesize = new Vector2(48, 16);
                    break;
                case "emptybutton":
                case "failbutton":
                    map.Hitbox = new Rectangle[2] { new Rectangle(2,1, 28,14),
                new Rectangle(1,2,30,12) };
                    map.Imagesize = new Vector2(32, 16);
                    break;
                case "cursor":
                    map.Imagesize = new Vector2(map.Texture.Width, map.Texture.Height);
                    map.Hitbox = new Rectangle[1] { new Rectangle(0, 0, 2, 2) };
                    break;
                case "zone":
                    map.Imagesize = new Vector2(8, 8);
                    map.Hitbox = new Rectangle[1] { new Rectangle(0, 0, 8, 8) };
                    map.Aniframes = 16;
                    map.AniFromTop = true;
                    break;
                case "zonenew":
                    map.Imagesize = new Vector2(8, 8);
                    map.Hitbox = new Rectangle[1] { new Rectangle(0, 0, 8, 8) };
                    map.Aniframes = 32;
                    map.AniFromTop = true;
                    break;
                case "minibutton":
                    map.Imagesize = new Vector2(16, 8);
                    map.Hitbox = new Rectangle[1] { new Rectangle(0, 0, 16, 8) };
                    break;
                case "settingsbutton":
                    map.Imagesize = new Vector2(73, 16);
                    map.Hitbox = new Rectangle[2] { new Rectangle(1, 2, 71, 12), new Rectangle(2, 1, 69, 14) };
                    break;
                case "selectbutton":
                    map.Imagesize = new Vector2(54, 16);
                    map.Hitbox = new Rectangle[2] { new Rectangle(1, 2, 52, 12), new Rectangle(2, 1, 50, 14) };
                    break;
                case "exitbutton":
                    map.Imagesize = new Vector2(34, 16);
                    map.Hitbox = new Rectangle[2] { new Rectangle(1, 2, 32, 12), new Rectangle(2, 1, 30, 14) };
                    break;
                default:
                    map.Imagesize = new Vector2(map.Texture.Width, map.Texture.Height);
                    map.Hitbox = new Rectangle[1] { new Rectangle(0, 0, map.Texture.Width, map.Texture.Height) };
                    break;
            }
            return map;
        }

    }
}
