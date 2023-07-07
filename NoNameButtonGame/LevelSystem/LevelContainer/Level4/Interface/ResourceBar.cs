using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logic;
using MonoUtils.Logic.Management;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level4.Interface;

public class ResourceBar : IManageable, IMoveable
{
    private readonly ResourceManager _resourceManager;
    private readonly Vector2 _position;
    private readonly Vector2 _size;
    private readonly Rectangle _rectangle;
    private readonly ResourcePair[] _resources;

    public ResourceBar(ResourceManager resourceManager, Vector2 position, float scale)
    {
        _resourceManager = resourceManager;
        _position = position;
        // _rectangle = new Rectangle(position.ToPoint(), size.ToPoint());
        _resources = new ResourcePair[8];
        for (int i = 0; i < 8; i++)
            _resources[i] = new ResourcePair((Resource.Type) i, position, scale);
    }

    public Rectangle Rectangle => _rectangle;
    public void Update(GameTime gameTime)
    {
        for (int i = 0; i < 8; i++)
            _resources[i].Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        for (int i = 0; i < 8; i++)
            _resources[i].Draw(spriteBatch);
    }

    public Vector2 GetPosition()
        => _position;

    public Vector2 GetSize()
        => _size;

    public void Move(Vector2 newPosition)
    {
        var offset = newPosition - _position;
        for (int i = 0; i < 8; i++)
            _resources[i].Move(_resources[i].GetPosition() + offset);
    }
}