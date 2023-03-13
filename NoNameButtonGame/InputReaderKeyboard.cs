using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace NoNameButtonGame;

public static class InputReaderKeyboard
{
    private static Dictionary<Keys, bool> _currentlyPressedKeys = new();
    private static bool _anyKeyPressed;

    /// <summary>
    /// Check if searched key is being pressed.
    /// </summary>
    /// <param name="search"></param>
    /// <param name="onlyOnces"> if true, stores searched key if pressed. Otherwise search is not stored</param>
    /// <returns>Returns if search is being pressed. If <paramref name="onlyOnces"> is true, only returns true if search is not stored./></returns>
    public static bool CheckKey(Keys search, bool onlyOnces = false)
    {
        var keyboardState = Keyboard.GetState();
        var isKeyDown = keyboardState.IsKeyDown(search);

        if (!onlyOnces)
            return isKeyDown;
        
        if (!_currentlyPressedKeys.ContainsKey(search))
            _currentlyPressedKeys.Add(search, isKeyDown);

        var returnValue = !_currentlyPressedKeys[search] && isKeyDown;
        _currentlyPressedKeys[search] = isKeyDown;
        
        return returnValue;
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