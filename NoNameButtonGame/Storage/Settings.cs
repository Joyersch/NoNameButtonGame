namespace NoNameButtonGame;
public class Settings
{
    public bool IsFixedStep { get; set; }
    public bool IsFullscreen { get; set; }

    public Settings()
    {
        Resolution = new Resolution();
    }

    public Resolution Resolution { get; set; }
}