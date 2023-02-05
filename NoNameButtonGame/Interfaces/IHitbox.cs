using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace NoNameButtonGame.Interfaces
{
    internal interface IHitbox
    {
        public Rectangle[] Hitbox { get; }
    }
}
