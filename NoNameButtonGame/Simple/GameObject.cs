using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joyersch.Obj
{
     class GameObject : MonoObject
    {
        public Texture2D Texture;
        public Vector2 Position;
        public Vector2 Size;
        public Vector2 FrameSize;
        public Rectangle ImageLocation;
        public Color DrawColor;
        public Rectangle rec;


        public override void Update(GameTime gt) {
            rec = new Rectangle(Position.ToPoint(), Size.ToPoint());
        }
        public override void Draw(SpriteBatch sp) {
            if (ImageLocation == new Rectangle(0,0,0,0)) {
                sp.Draw(Texture, rec, DrawColor);
            } else {
                sp.Draw(Texture, rec, ImageLocation, DrawColor);
            }
            
        }
    }
}
