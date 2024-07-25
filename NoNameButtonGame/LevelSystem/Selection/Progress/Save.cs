using MonoUtils.Settings;

namespace NoNameButtonGame.LevelSystem.Selection.Progress;

public class Save : ISave
{
    public Level[] Levels { get; set; } =
    {
        new Level(),
        new Level(),
        new Level(),
        new Level(),
        new Level(),
        new Level(),
        new Level(),
        new Level(),
        new Level(),
        new Level(),
    };

    public void Reset()
    {
        Levels = new[]
        {
            new Level(),
            new Level(),
            new Level(),
            new Level(),
            new Level(),
            new Level(),
            new Level(),
            new Level(),
            new Level(),
            new Level(),
        };
    }
}