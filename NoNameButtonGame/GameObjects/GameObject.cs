using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Xna.Framework.Content;
using NoNameButtonGame.Hitboxes;

namespace NoNameButtonGame.GameObjects
{
    public class GameObject : MonoObject
    {
        public Texture2D Texture;
        public Vector2 Position;
        public Vector2 Size;
        public Vector2 FrameSize;
        public Rectangle ImageLocation;
        public Color DrawColor;
        public Rectangle rec;

        public override void Update(GameTime gameTime)
        {
            rec = new Rectangle(Position.ToPoint(), Size.ToPoint());
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (ImageLocation == new Rectangle(0, 0, 0, 0))
            {
                spriteBatch.Draw(Texture, rec, DrawColor);
            }
            else
            {
                spriteBatch.Draw(Texture, rec, ImageLocation, DrawColor);
            }
        }
    }
}