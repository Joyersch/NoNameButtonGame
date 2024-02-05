using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Logic.Text;
using MonoUtils.Ui.Logic;

namespace NoNameButtonGame.LevelSystem.Settings;

public class Flag : GameObject, IInteractable
{
    public readonly TextProvider.Language Language;
    public new static Vector2 DefaultSize => DefaultMapping.ImageSize;
    public new static Texture2D DefaultTexture;

    public new static TextureHitboxMapping DefaultMapping => new TextureHitboxMapping()
    {
        ImageSize = new Vector2(64, 32),
        Hitboxes = new[]
        {
            new Rectangle(0, 0, 64, 32)
        },
    };

    public event Action<object> Click;

    private MouseActionsMat _mouseActionsMat;

    public Flag(TextProvider.Language language) : this(language, Vector2.Zero, 1F)
    {

    }

    public Flag(TextProvider.Language language, float scale) : this(language, Vector2.Zero, scale)
    {

    }

    public Flag(TextProvider.Language language, Vector2 position, float scale) : base(position, DefaultSize * scale,
        DefaultTexture,
        DefaultMapping)
    {
        Language = language;
        var imageLocation = new Rectangle(Vector2.Zero.ToPoint(), DefaultMapping.ImageSize.ToPoint());
        imageLocation.X = (int)DefaultSize.X * (int)language;
        ImageLocation = imageLocation;
        _mouseActionsMat = new MouseActionsMat(this);
        _mouseActionsMat.Click += delegate
        {
            Click?.Invoke(this);
        };
    }

    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        _mouseActionsMat.UpdateInteraction(gameTime, toCheck);
    }
}