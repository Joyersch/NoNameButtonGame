using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.GameObjects.Text;
using NoNameButtonGame.Input;
using NoNameButtonGame.Text;

namespace NoNameButtonGame.GameObjects.Buttons;

internal class HoldButton : EmptyButton
{
    private float _holdTime;
    public float EndHoldTime = 10000F;
    private bool _pressed;
    private readonly TextBuilder textContainer;


    public HoldButton(Vector2 position, Vector2 size) : base(position, size)
    {
        textContainer = new TextBuilder("test", new Vector2(float.MinValue, float.MinValue), new Vector2(16, 16),
            0);
    }

    public override void Update(GameTime gameTime, Rectangle mousePosition)
    {
        textContainer.ChangeText((((EndHoldTime - _holdTime) / 1000).ToString("0.0") + "s").Replace(',', '.'));

        textContainer.Position = Rectangle.Center.ToVector2() - textContainer.Rectangle.Size.ToVector2() / 2;
        textContainer.Position.Y -= 32;
        textContainer.Update(gameTime);
        bool isMouseHovering = HitboxCheck(mousePosition);
        if (isMouseHovering)
        {
            if (!hover)
                InvokeEnterEventHandler();

            if (InputReaderMouse.CheckKey(InputReaderMouse.MouseKeys.Left, false) && !_pressed)
            {
                _holdTime += (float) gameTime.ElapsedGameTime.TotalMilliseconds;
                if (_holdTime > EndHoldTime)
                {
                    InvokeClickEventHandler();
                    EndHoldTime = 0;
                    _holdTime = 0;
                    _pressed = true;
                }
            }
            else if (!_pressed)
                _holdTime -= (float) gameTime.ElapsedGameTime.TotalMilliseconds / 2;
        }
        else
        {
            if (hover)
                InvokeLeaveEventHandler();
            if (!_pressed)
                _holdTime -= (float) gameTime.ElapsedGameTime.TotalMilliseconds / 2;
        }

        if (_holdTime < 0) _holdTime = 0;
        
        ImageLocation = new Rectangle(isMouseHovering ? (int) FrameSize.X : 0, 0, (int) FrameSize.X, (int) FrameSize.Y);
        hover = isMouseHovering;
        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        textContainer.Draw(spriteBatch);
    }
}