using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils;
using MonoUtils.Logic.Management;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level4;

public class OverworldCollection : List<GameObject>, IManageable
{
    public Rectangle Rectangle => new Rectangle(-32000, -32000, 64000, 64000);
    public void Update(GameTime gameTime)
    {
        foreach (var obj in this)
            obj.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var obj in this)
            obj.Draw(spriteBatch);
    }
}