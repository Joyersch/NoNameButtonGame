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
    public static class InputReaderKeyboard
    {
        static List<Keys> CRPKeys = new List<Keys>();
        static int KeyPressed = 0;

        public static bool CheckKey(Keys search, bool OnlyOnces) {
            if (OnlyOnces) {
                for (int i = 0; i < CRPKeys.Count; i++) {

                    if (Keyboard.GetState().IsKeyUp(CRPKeys[i])) {
                        CRPKeys.Remove(CRPKeys[i]);
                    }
                }

                if (CRPKeys.Count == 0) {

                    if (Keyboard.GetState().IsKeyDown(search)) {
                        CRPKeys.Add(search);
                        return true;
                    }
                }
            } else {
                if (Keyboard.GetState().IsKeyDown(search))
                    return true;
            }
            return false;
        }

        public static bool AnyKeyPress(bool OnlyOnces) {
            if (Keyboard.GetState().GetPressedKeys().Length > 0) {
                if (OnlyOnces) {
                    if (KeyPressed == 0) {
                        KeyPressed = Keyboard.GetState().GetPressedKeys().Length;
                        return true;
                    } else {
                        return false;
                    }
                } else {
                    return true;
                }

            }
            KeyPressed = 0;
            return false;
        }

    }
}
