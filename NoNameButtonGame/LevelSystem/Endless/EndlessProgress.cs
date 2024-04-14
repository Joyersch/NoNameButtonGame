using MonoUtils.Settings;

namespace NoNameButtonGame.LevelSystem.Endless;

public class EndlessProgress : ISave
{
    public int HighestLevel { get; set; }

    public void Reset()
    {
        HighestLevel = 0;
    }
}