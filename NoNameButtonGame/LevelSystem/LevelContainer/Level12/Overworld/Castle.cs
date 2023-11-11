using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils;
using MonoUtils.Logic;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Ui.Logic;
using MonoUtils.Ui.Objects.TextSystem;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level12.Overworld;

public class Castle : GameObject, IInteractable, ILocation
{
    public new static Vector2 DefaultSize => DefaultMapping.ImageSize;
    public new static Texture2D DefaultTexture;
    
    private bool _isHover;
    private string _name;
    private Guid _guid;

    public event Action Interacted;

    public new static TextureHitboxMapping DefaultMapping => new TextureHitboxMapping()
    {
        ImageSize = new Vector2(64, 48),
        Hitboxes = new[]
        {
            new Rectangle(0, 0, 64, 48)
        }
    };

    public Castle(Vector2 position, float scale, string name) : base(position, DefaultSize * scale,
        DefaultTexture,
        DefaultMapping)
    {
        DrawColor = Color.Gray;
        _name = name;
        _guid = Guid.NewGuid();
    }
    
    public override void Update(GameTime gameTime)
    {
        if (_isHover && InputReaderMouse.CheckKey(InputReaderMouse.MouseKeys.Left, true))
            Interacted?.Invoke();
        base.Update(gameTime);
    }

    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        _isHover = toCheck.Hitbox.Any(c => c.Intersects(Rectangle.ExtendFromCenter(1.5F)));
    }

    public string GetName()
        => _name;

    public Guid GetGuid()
        => _guid;
}