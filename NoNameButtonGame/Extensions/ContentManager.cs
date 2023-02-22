using Audio = Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace NoNameButtonGame.Extensions;

public static class ContentManager
{
    public static Texture2D GetTexture(
        this Microsoft.Xna.Framework.Content.ContentManager contentManager, string textureName)
        => contentManager.Load<Texture2D>("Textures/" + textureName);

    public static Audio.SoundEffect GetMusic(this Microsoft.Xna.Framework.Content.ContentManager contentManager, string name)
        => contentManager.Load<Audio.SoundEffect>("Music/" + name);

    public static Audio.SoundEffect GetSfx(this Microsoft.Xna.Framework.Content.ContentManager contentManager, string name)
        => contentManager.Load<Audio.SoundEffect>("SFX/" + name);
}