using MonoUtils.Settings;

namespace NoNameButtonGame.LevelSystem;

public class Progress : ISave
{
    public int MaxLevel { get; set; }

    public bool FinishedGame { get; set; }

    public bool FirstStart { get; set; } = true;

    public void Reset()
    {
        MaxLevel = 0;
    }
}