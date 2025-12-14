namespace NoNameButtonGame.Ui.Buttons
{
    public interface IButton : IUserInferface, IMoveable, IColorable, IMouseActions, ILayerable, IScaleable
    {
        public bool IsHover { get; }
    }
}