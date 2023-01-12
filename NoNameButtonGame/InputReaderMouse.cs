using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Joyersch.Input
{
    public static class InputReaderMouse
    {
        public enum MouseKeys
        {
            Left,
            Middle,
            Right
        }

        static List<MouseKeys> CurrentMouseKeys = new List<MouseKeys>();

        public static bool CheckKey(MouseKeys search, bool onlyOnces)
        {
            MouseState mouseState = Mouse.GetState();
            if (onlyOnces)
            {
                for (int i = 0; i < CurrentMouseKeys.Count; i++)
                {
                    var released = search switch
                    {
                        MouseKeys.Left => mouseState.LeftButton == ButtonState.Released,
                        MouseKeys.Middle => mouseState.MiddleButton == ButtonState.Released,
                        MouseKeys.Right => mouseState.RightButton == ButtonState.Released,
                        _ => false
                    };
                    
                    if (released)
                        CurrentMouseKeys.Remove(CurrentMouseKeys[i]);
                }
            }

            var pressed = search switch
            {
                MouseKeys.Left => mouseState.LeftButton == ButtonState.Pressed,
                MouseKeys.Middle => mouseState.MiddleButton == ButtonState.Pressed,
                MouseKeys.Right => mouseState.RightButton == ButtonState.Pressed,
                _ => false
            };
        
            if (onlyOnces)
                CurrentMouseKeys.Add(search);

            return pressed;
        }
    }
}