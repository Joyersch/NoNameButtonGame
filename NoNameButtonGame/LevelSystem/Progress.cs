using MonoUtils.Settings;

namespace NoNameButtonGame.LevelSystem;

public class Progress : ISave
{
    public int MaxLevel { get; set; }

    public bool FinishedLevels { get; set; }

    public bool FinishedSelect { get; set; }

    public bool FinishedEndless { get; set; }

    public void Reset()
    {
        MaxLevel = 0;
        FinishedEndless = false;
        FinishedLevels = false;
        FinishedSelect = false;
    }
}