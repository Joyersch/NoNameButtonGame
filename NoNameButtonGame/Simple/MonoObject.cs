﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Raigy.Obj
{
    abstract class MonoObject
    {
        public abstract void Update(GameTime gt);
        public abstract void Draw(SpriteBatch sp);
    }
}
