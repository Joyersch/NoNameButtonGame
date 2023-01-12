using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NoNameButtonGame.Input
{
    public static class InputReaderKeyboard
    {
        private static List<Keys> _currentlyPressedKeys = new();
        private static bool _anyKeyPressed = false;
        
        public static bool CheckKey(Keys search, bool onlyOnces)
        {
            if (!onlyOnces)
                return Keyboard.GetState().IsKeyDown(search);

            if (_currentlyPressedKeys.Any(k => k == search))
            {
                if (!Keyboard.GetState().IsKeyDown(search))
                    _currentlyPressedKeys.Remove(search);
                return false;
            }

            _currentlyPressedKeys.Add(search);
            return true;
        }

        public static bool AnyKeyPress(bool onlyOnces)
        {
            if (Keyboard.GetState().GetPressedKeys().Length == 0)
                return false;
            
            if (_anyKeyPressed)
                return false;
            
            if (!onlyOnces)
                return _anyKeyPressed = true;

            return true;
        }
    }
}