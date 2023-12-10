namespace NoNameButtonGame.LevelSystem.Settings;

public class Volume
{
    public float Value { get; }

    public Volume(float value)
    {
        if (value > 1F)
            value = 1F;
        if (value < 0F)
            value = 0F;

        Value = value;
    }

    public override string ToString()
    {
        return $"{Value * 100:0}%";
    }
}