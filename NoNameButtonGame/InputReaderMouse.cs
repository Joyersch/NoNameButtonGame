﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace NoNameButtonGame;

public static class InputReaderMouse
{
    public enum MouseKeys
    {
        Left,
        Middle,
        Right
    }

    private static Dictionary<MouseKeys, ButtonState> _storedMouseStates = new();

    /// <summary>
    /// Stores the current key-states. Call this at the end of update
    /// </summary>
    public static void StoreButtonStates()
    {
        MouseState mouseState = Mouse.GetState();
        if (!_storedMouseStates.Any())
            _storedMouseStates = new()
            {
                {MouseKeys.Left, mouseState.LeftButton},
                {MouseKeys.Middle, mouseState.MiddleButton},
                {MouseKeys.Right, mouseState.RightButton}
            };
        else
        {
            _storedMouseStates[MouseKeys.Left] = mouseState.LeftButton;
            _storedMouseStates[MouseKeys.Middle] = mouseState.MiddleButton;
            _storedMouseStates[MouseKeys.Right] = mouseState.RightButton;
        }
    }

    /// <summary>
    /// Check if a key is pressed.
    /// </summary>
    /// <param name="search">Key to be checked</param>
    /// <param name="onlyOnces">If true, will return false if stored button states contain search</param>
    /// <returns>if <paramref name="search"/> is beeing pressed, returns true else false. </returns>
    public static bool CheckKey(MouseKeys search, bool onlyOnces)
        => !(onlyOnces && _storedMouseStates.Any(s => s.Key == search && s.Value == ButtonState.Pressed))
           && search switch
           {
               MouseKeys.Left => Mouse.GetState().LeftButton == ButtonState.Pressed,
               MouseKeys.Middle => Mouse.GetState().MiddleButton == ButtonState.Pressed,
               MouseKeys.Right => Mouse.GetState().RightButton == ButtonState.Pressed,
               _ => false
           };
}