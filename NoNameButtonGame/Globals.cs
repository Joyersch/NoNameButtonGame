using System;

namespace NoNameButtonGame;

internal static class Globals
{
    // Directory save files
    public static readonly string SaveDirectory =
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/NoNameButtonGame/";
    
}