using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NoNameButtonGame.Input;

public static class InputReaderMouse
{
    public enum MouseKeys
    {
        Left,
        Middle,
        Right
    }

    private static Dictionary<MouseKeys, ButtonState> _lastMouseKeys = new();

    public static void UpdateLast()
    {
        MouseState mouseState = Mouse.GetState();
        _lastMouseKeys = new()
        {
            {MouseKeys.Left, mouseState.LeftButton},
            {MouseKeys.Middle, mouseState.MiddleButton},
            {MouseKeys.Right, mouseState.RightButton}
        };
    }

    public static bool CheckKey(MouseKeys search, bool onlyOnces)
        => !(onlyOnces && _lastMouseKeys.Any(s => s.Key == search && s.Value == ButtonState.Pressed))
           && search switch
           {
               MouseKeys.Left => Mouse.GetState().LeftButton == ButtonState.Pressed,
               MouseKeys.Middle => Mouse.GetState().MiddleButton == ButtonState.Pressed,
               MouseKeys.Right => Mouse.GetState().RightButton == ButtonState.Pressed,
               _ => false
           };
}