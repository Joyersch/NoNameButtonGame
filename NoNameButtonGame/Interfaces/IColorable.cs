using Microsoft.Xna.Framework;

namespace NoNameButtonGame.Interfaces;

public interface IColorable
{
    public void ChangeColor(Color[] input);
    public int ColorLength();
}