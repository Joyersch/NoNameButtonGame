using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace NoNameButtonGame;

public static class InputReaderKeyboard
{
    private static List<Keys> _currentlyPressedKeys = new();
    private static bool _anyKeyPressed;

    /// <summary>
    /// Check if searched key is being pressed.
    /// </summary>
    /// <param name="search"></param>
    /// <param name="onlyOnces"> if true, stores searched key if pressed. Otherwise search is not stored</param>
    /// <returns>Returns if search is being pressed. If <paramref name="onlyOnces" is true, only returns true if search is not stored./></returns>
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

        if (!Keyboard.GetState().IsKeyDown(search))
            return false;
        _currentlyPressedKeys.Add(search);
        return true;
    }

    /// <summary>
    /// check if any key is pressed.
    /// </summary>
    /// <param name="onlyOnces"></param>
    /// <returns>true if any key is pressed, else false</returns>
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