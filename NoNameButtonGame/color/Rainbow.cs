using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Raigy.Obj;
namespace NoNameButtonGame.color
{
    class Rainbow : BaseAniColor
    {
        
        public Rainbow() {
           
        }
        public override void Init() {
            Color = new Color[768];
            int c = 0;
            for (int i = 0; i < 256; i++) {
                Color[i + c * 256] = new Color(i, 255 - i, 255);
            }
            c++;
            for (int i = 0; i < 256; i++) {
                Color[i + c * 256] = new Color(255, i, 255 - i);
            }
            c++;
            for (int i = 0; i < 256; i++) {
                Color[i + c * 256] = new Color(255 - i, 255, i);
            }
        }
    }
}
