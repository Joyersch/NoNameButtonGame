namespace NoNameButtonGame.Ui;

public interface IColorable
{
    public void ChangeColor(Microsoft.Xna.Framework.Color[] input);
    public int ColorLength();

    public Microsoft.Xna.Framework.Color[] GetColor();
}