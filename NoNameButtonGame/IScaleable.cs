namespace NoNameButtonGame;

public interface IScaleable
{
    public float Scale { get; }
    public void SetScale(ScaleProvider provider);
}