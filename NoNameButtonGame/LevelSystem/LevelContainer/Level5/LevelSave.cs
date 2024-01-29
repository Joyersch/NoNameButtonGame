using MonoUtils.Settings;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level5;

public class LevelSave : ISave
{
    public long Beans { get; set; }

    public int Upgrade1 { get; set; }
    public int Upgrade2 { get; set; }
    public int Upgrade3 { get; set; }
    public int Upgrade4 { get; set; }

    public bool CanSeeDistraction { get; set; }

    public void Reset()
    {
        Beans = 0;
        Upgrade1 = 0;
        Upgrade2 = 0;
        Upgrade3 = 0;
        Upgrade4 = 0;
        CanSeeDistraction = false;
    }
}