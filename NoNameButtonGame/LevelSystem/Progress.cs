using MonoUtils.Settings;

namespace NoNameButtonGame.LevelSystem;

public class Progress : ISave
{
    public int MaxLevel { get; set; }

    public void Reset()
    {
        MaxLevel = 0;
    }
}