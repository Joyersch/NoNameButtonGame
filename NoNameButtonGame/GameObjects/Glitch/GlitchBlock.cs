using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoUtils.Logic;
using MonoUtils.Ui.Objects;
using MonoUtils;
using MonoUtils.Logging;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Logic.Management;
using MonoUtils.Ui.Color;
using MonoUtils.Ui.Logic;

namespace NoNameButtonGame.GameObjects.Glitch;

internal class GlitchBlock : GameObject, IColorable, IInteractable, IMouseActions
{
    public event Action<object> Leave;
    public event Action<object> Enter;
    public event Action<object> Click;

    private MouseActionsMat _mouseActionsMat;

    public new static Vector2 DefaultSize = DefaultMapping.ImageSize * 4;

    public new static Texture2D DefaultTexture;

    public static Color Color = new Color(181, 54, 54);

    public new static TextureHitboxMapping DefaultMapping => new TextureHitboxMapping()
    {
        ImageSize = new Vector2(8, 8),
        Hitboxes = new[]
        {
            new Rectangle(0, 0, 8, 8)
        },
        AnimationFrames = 32,
        AnimationFromTop = true,
        AnimationSpeed = 128
    };


    public GlitchBlock(Vector2 position) : this(position, 1F)
    {
    }

    public GlitchBlock(Vector2 position, float scale) : this(position, DefaultSize * scale)
    {
    }

    public GlitchBlock(Vector2 position, Vector2 size) : base(position, size, DefaultTexture,
        DefaultMapping)
    {
        var scale = size / TextureHitboxMapping.ImageSize;
        var max = Math.Max(scale.X, scale.Y);
        FramePosition = new Rectangle(Vector2.Zero.ToPoint(), (size / max).ToPoint());
        _mouseActionsMat = new MouseActionsMat(this);
        _mouseActionsMat.Leave += delegate { Leave?.Invoke(this); };
        _mouseActionsMat.Enter += delegate { Enter?.Invoke(this); };
        _mouseActionsMat.Click += delegate { Click?.Invoke(this); };
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        _mouseActionsMat.UpdateInteraction(gameTime, toCheck);
    }

    public void ChangeColor(Color[] input)
        => DrawColor = input[0];

    public void ChangeColor(Color input)
        => DrawColor = input;

    public int ColorLength() => 1;
}