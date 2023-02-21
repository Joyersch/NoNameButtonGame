using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace NoNameButtonGame;

public static class InputReaderKeyboard
{
    private static List<Keys> _currentlyPressedKeys = new();
    private static bool _anyKeyPressed;
        
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