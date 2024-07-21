using MonoUtils.Settings;

namespace NoNameButtonGame.LevelSystem.Selection;

public class SelectionState : ISave
{
    public Difficulty Difficulty { get; set; } = Difficulty.Easy;
    public LevelFactory.LevelType Level { get; set; } = LevelFactory.LevelType.Tutorial;
    public void Reset()
    {
        Difficulty = Difficulty.Easy;
        Level = LevelFactory.LevelType.Tutorial;
    }
}