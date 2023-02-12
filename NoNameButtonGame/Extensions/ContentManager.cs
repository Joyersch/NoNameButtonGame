using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.Cache;

namespace NoNameButtonGame.Extensions;

public static class ContentManager
{
    public static TextureHitboxMapping GetHitboxMapping(this Microsoft.Xna.Framework.Content.ContentManager contentManager, string textureName)
    {
        var map = new TextureHitboxMapping
        {
            Texture = contentManager.Load<Texture2D>("Textures/" + textureName)
        };
        switch (textureName)
        {
            case "awesomebutton":
                map.Hitboxes = new Rectangle[2]
                {
                    new Rectangle(2, 1, 28, 14),
                    new Rectangle(1, 2, 30, 12)
                };
                map.ImageSize = new Vector2(32, 16);
                break;

            case "startbutton":
                map.Hitboxes = new Rectangle[2]
                {
                    new Rectangle(2, 1, 44, 14),
                    new Rectangle(1, 2, 46, 12)
                };
                map.ImageSize = new Vector2(48, 16);
                break;

            case "emptybutton":
            case "failbutton":
                map.Hitboxes = new Rectangle[2]
                {
                    new Rectangle(2, 1, 28, 14),
                    new Rectangle(1, 2, 30, 12)
                };
                map.ImageSize = new Vector2(32, 16);
                break;

            case "cursor":
                map.ImageSize = new Vector2(map.Texture.Width, map.Texture.Height);
                map.Hitboxes = new Rectangle[1] {new Rectangle(0, 0, 1, 1)};
                break;

            case "zone":
                map.ImageSize = new Vector2(8, 8);
                map.Hitboxes = new Rectangle[1] {new Rectangle(0, 0, 8, 8)};
                map.AnimationsFrames = 16;
                map.AnimationFromTop = true;
                break;

            case "zonenew":
                map.ImageSize = new Vector2(8, 8);
                map.Hitboxes = new Rectangle[1] {new Rectangle(0, 0, 8, 8)};
                map.AnimationsFrames = 32;
                map.AnimationFromTop = true;
                break;

            case "minibutton":
                map.ImageSize = new Vector2(16, 8);
                map.Hitboxes = new Rectangle[2] {new Rectangle(0, 1, 16, 6), new Rectangle(1, 0, 14, 8)};
                break;

            case "squarebutton":
                map.ImageSize = new Vector2(8, 8);
                map.Hitboxes = new Rectangle[2] {new Rectangle(1, 0, 6, 8), new Rectangle(0, 1, 8, 6)};
                break;

            case "settingsbutton":
                map.ImageSize = new Vector2(73, 16);
                map.Hitboxes = new Rectangle[2] {new Rectangle(1, 2, 71, 12), new Rectangle(2, 1, 69, 14)};
                break;

            case "selectbutton":
                map.ImageSize = new Vector2(54, 16);
                map.Hitboxes = new Rectangle[2] {new Rectangle(1, 2, 52, 12), new Rectangle(2, 1, 50, 14)};
                break;

            case "exitbutton":
                map.ImageSize = new Vector2(34, 16);
                map.Hitboxes = new Rectangle[2] {new Rectangle(1, 2, 32, 12), new Rectangle(2, 1, 30, 14)};
                break;
            case "font":
                map.ImageSize = new Vector2(8, 8);
                map.Hitboxes = Array.Empty<Rectangle>();
                break;
            default:
                map.ImageSize = new Vector2(map.Texture.Width, map.Texture.Height);
                map.Hitboxes = new Rectangle[1] {new Rectangle(0, 0, map.Texture.Width, map.Texture.Height)};
                break;
        }

        return map;
    }

    public static SoundEffect GetMusic(this Microsoft.Xna.Framework.Content.ContentManager contentManager, string name)
        => contentManager.Load<SoundEffect>("Music/" + name);
    
    public static SoundEffect GetSFX(this Microsoft.Xna.Framework.Content.ContentManager contentManager, string name)
        => contentManager.Load<SoundEffect>("SFX/" + name);
}