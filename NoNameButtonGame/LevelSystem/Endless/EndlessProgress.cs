using MonoUtils.Settings;

namespace NoNameButtonGame.LevelSystem.Endless;

public class EndlessProgress : ISave
{
    public int HighestLevel { get; set; }

    public double? BestTimeTo50 { get; set; }

    public void Reset()
    {
        HighestLevel = 0;
        BestTimeTo50 = null;
    }
}