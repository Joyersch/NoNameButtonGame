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
        public Vector2 ImageSize;
        public Rectangle[] Hitboxes;
        public int AnimationsFrames;
        public bool? AnimationFromTop;
        
    }
    public static class Mapping
    {

        // I dont now if this is a good way to do this, it works which is fine my me. ~2020
        // 
        // After doing a little touch up, I still don't know if it is a good way but it looks more right ~2023
        public static HitboxMap GetHitboxMapping(this ContentManager contentManager, string textureName) {
            
            var map = new HitboxMap {
                Texture = contentManager.Load<Texture2D>(textureName)
            };
            switch (textureName) {
                
                case "awesomebutton":
                    map.Hitboxes = new Rectangle[2] { new Rectangle(2,1, 28,14),
                new Rectangle(1,2,30,12) };
                    map.ImageSize = new Vector2(32, 16);
                    break;
                
                case "startbutton":
                    map.Hitboxes = new Rectangle[2] { new Rectangle(2,1, 44,14),
                new Rectangle(1,2,46,12)  };
                    map.ImageSize = new Vector2(48, 16);
                    break;
                
                case "emptybutton":
                case "failbutton":
                    map.Hitboxes = new Rectangle[2] { new Rectangle(2,1, 28,14),
                new Rectangle(1,2,30,12) };
                    map.ImageSize = new Vector2(32, 16);
                    break;
                
                case "cursor":
                    map.ImageSize = new Vector2(map.Texture.Width, map.Texture.Height);
                    map.Hitboxes = new Rectangle[1] { new Rectangle(0, 0, 2, 2) };
                    break;
                
                case "zone":
                    map.ImageSize = new Vector2(8, 8);
                    map.Hitboxes = new Rectangle[1] { new Rectangle(0, 0, 8, 8) };
                    map.AnimationsFrames = 16;
                    map.AnimationFromTop = true;
                    break;
                
                case "zonenew":
                    map.ImageSize = new Vector2(8, 8);
                    map.Hitboxes = new Rectangle[1] { new Rectangle(0, 0, 8, 8) };
                    map.AnimationsFrames = 32;
                    map.AnimationFromTop = true;
                    break;
                
                case "minibutton":
                    map.ImageSize = new Vector2(16, 8);
                    map.Hitboxes = new Rectangle[1] { new Rectangle(0, 0, 16, 8) };
                    break;
                
                case "settingsbutton":
                    map.ImageSize = new Vector2(73, 16);
                    map.Hitboxes = new Rectangle[2] { new Rectangle(1, 2, 71, 12), new Rectangle(2, 1, 69, 14) };
                    break;
                case "selectbutton":
                    map.ImageSize = new Vector2(54, 16);
                    map.Hitboxes = new Rectangle[2] { new Rectangle(1, 2, 52, 12), new Rectangle(2, 1, 50, 14) };
                    break;
                
                case "exitbutton":
                    map.ImageSize = new Vector2(34, 16);
                    map.Hitboxes = new Rectangle[2] { new Rectangle(1, 2, 32, 12), new Rectangle(2, 1, 30, 14) };
                    break;
                
                default:
                    map.ImageSize = new Vector2(map.Texture.Width, map.Texture.Height);
                    map.Hitboxes = new Rectangle[1] { new Rectangle(0, 0, map.Texture.Width, map.Texture.Height) };
                    break;
            }
            return map;
        }

    }
}
