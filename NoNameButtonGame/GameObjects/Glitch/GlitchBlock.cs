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

    public new static Vector2 DefaultSize = DefaultMapping.ImageSize * 2;

    public new static Texture2D DefaultTexture;

    public new static TextureHitboxMapping DefaultMapping => new TextureHitboxMapping()
    {
        ImageSize = new Vector2(16, 16),
        Hitboxes = new[]
        {
            new Rectangle(0, 0, 16, 16)
        },
        AnimationFrames = 64,
        AnimationFromTop = true,
        AnimationSpeed = 31
    };

    public GlitchBlock(Vector2 position) : this(position, DefaultSize,
        new Rectangle(Vector2.Zero.ToPoint(), DefaultMapping.ImageSize.ToPoint()))
    {
    }

    public GlitchBlock(Vector2 position, Rectangle framePosition) : this(position, 1F, framePosition)
    {
    }

    public GlitchBlock(Vector2 position, float scale, Rectangle framePosition) : this(position, DefaultSize * scale,
        framePosition)
    {
    }

    public GlitchBlock(Vector2 position, Vector2 size, Rectangle framePosition) : base(position, size, DefaultTexture,
        DefaultMapping)
    {
        FramePosition = framePosition;
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

    public int ColorLength() => 1;
}