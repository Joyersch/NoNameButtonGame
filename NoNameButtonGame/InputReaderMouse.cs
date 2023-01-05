using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Raigy.Input
{
    public static class InputReaderMouse
    {

        public enum MouseKeys
        {
            Left,
            Middle,
            Right
        }
        static List<MouseKeys> CRPKeys = new List<MouseKeys>();
        public static bool CheckKey(MouseKeys search, bool OnlyOnces) {

            MouseState ms = Mouse.GetState();
            if (OnlyOnces) {
                for (int i = 0; i < CRPKeys.Count; i++) {
                    switch (CRPKeys[i]) {
                        case MouseKeys.Left:
                            if (ms.LeftButton == ButtonState.Released) {
                                CRPKeys.Remove(CRPKeys[i]);
                            }
                            break;
                        case MouseKeys.Middle:
                            if (ms.MiddleButton == ButtonState.Released) {
                                CRPKeys.Remove(CRPKeys[i]);
                            }
                            break;
                        case MouseKeys.Right:
                            if (ms.RightButton == ButtonState.Released) {
                                CRPKeys.Remove(CRPKeys[i]);
                            }
                            break;
                    }
                }

                if (CRPKeys.Count == 0) {
                    switch (search) {
                        case MouseKeys.Left:
                            if (ms.LeftButton == ButtonState.Pressed) {
                                CRPKeys.Add(search);
                                return true;
                            }
                            break;
                        case MouseKeys.Middle:
                            if (ms.MiddleButton == ButtonState.Pressed) {
                                CRPKeys.Add(search);
                                return true;
                            }
                            break;
                        case MouseKeys.Right:
                            if (ms.RightButton == ButtonState.Pressed) {
                                CRPKeys.Add(search);
                                return true;
                            }
                            break;
                    }
                    
                }
            } else {
                switch (search) {
                    case MouseKeys.Left:
                        if (ms.LeftButton == ButtonState.Pressed) {
                            return true;
                        }
                        break;
                    case MouseKeys.Middle:
                        if (ms.MiddleButton == ButtonState.Pressed) {
                            return true;
                        }
                        break;
                    case MouseKeys.Right:
                        if (ms.RightButton == ButtonState.Pressed) {
                            return true;
                        }
                        break;
                }
            }
            return false;
        }
    }
}
