using MonoUtils.Settings;

namespace NoNameButtonGame.LevelSystem.Endless;

public class Challenges : ISave
{
    public bool Score50 { get; set; }
    public bool Score25 { get; set; }
    public bool Score10 { get; set; }

    public bool Time1h { get; set; }
    public bool Time30min { get; set; }

    public void Reset()
    {
        Score50 = false;
        Score25 = false;
        Score10 = false;

        Time1h = false;
        Time30min = false;
    }
}