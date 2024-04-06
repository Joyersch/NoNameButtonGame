using Microsoft.Xna.Framework;

namespace NoNameButtonGame.LevelSystem.LevelContainer.ButtonGridLevel;

public class ColorComponent
{
    public string Text { get; set; }
    public Color Color { get; set; }

    public int Difficulty { get; set; } = 5000;
}