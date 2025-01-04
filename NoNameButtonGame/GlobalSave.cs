using Joyersch.Monogame.Storage;

namespace NoNameButtonGame;

public class GlobalSave : ISave
{
    public bool WasLaunched { get; set; } = false;

    public void Reset()
    {
        WasLaunched = false;
    }
}