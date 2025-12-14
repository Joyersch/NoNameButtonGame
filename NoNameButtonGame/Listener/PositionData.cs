using Microsoft.Xna.Framework;
using NoNameButtonGame;

namespace Joyersch.Monogame.Listener;

public sealed class PositionData
{
    public PositionData(IMoveable Main, Vector2 OldPosition, IMoveable Sub)
    {
        this.Main = Main;
        this.OldPosition = OldPosition;
        this.Sub = Sub;
    }

    public IMoveable Main { get; set; }
    public Vector2 OldPosition { get; set; }
    public IMoveable Sub { get; set; }

    public void Deconstruct(out IMoveable Main, out Vector2 OldPosition, out IMoveable Sub)
    {
        Main = this.Main;
        OldPosition = this.OldPosition;
        Sub = this.Sub;
    }
}