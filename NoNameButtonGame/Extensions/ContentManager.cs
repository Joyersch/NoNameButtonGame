using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.Cache;

namespace NoNameButtonGame.Extensions;

public static class ContentManager
{
    public static Texture2D GetTexture(
        this Microsoft.Xna.Framework.Content.ContentManager contentManager, string textureName)
        => contentManager.Load<Texture2D>("Textures/" + textureName);

    public static SoundEffect GetMusic(this Microsoft.Xna.Framework.Content.ContentManager contentManager, string name)
        => contentManager.Load<SoundEffect>("Music/" + name);

    public static SoundEffect GetSfx(this Microsoft.Xna.Framework.Content.ContentManager contentManager, string name)
        => contentManager.Load<SoundEffect>("SFX/" + name);
}