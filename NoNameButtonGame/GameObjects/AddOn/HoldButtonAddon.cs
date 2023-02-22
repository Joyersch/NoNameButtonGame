using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.Text;

namespace NoNameButtonGame.GameObjects.AddOn;

public class HoldButtonAddon : GameObject
{
    public event Action TimerReachedZero;

    private float _startTime;
    private readonly EmptyButton _button;
    private readonly TextBuilder _timer;
    private bool _isHover;
    private float _time;

    public HoldButtonAddon(EmptyButton button, float startTime) : base(
        button.Position, button.Size, DefaultTexture, DefaultMapping)
    {
        _button = button;
        _startTime = startTime;
        _time = _startTime;
        button.Enter += o => _isHover = true; 
        button.Leave += o => _isHover = false; 
        _timer = new TextBuilder((_startTime / 1000F).ToString("0.0"),
            button.Position);
    }

    public void Update(GameTime gameTime, Rectangle mousePosition)
    {
        base.Update(gameTime);;
        _button.Update(gameTime, mousePosition);
        float passedGameTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        if (_isHover && InputReaderMouse.CheckKey(InputReaderMouse.MouseKeys.Left, false))
        {
            _time -= passedGameTime;
            if (_time <= 0)
            {
                _time = 0;
                TimerReachedZero?.Invoke();
            }
        }
        else
        {
            _time += passedGameTime / 2;
            if (_time > _startTime)
                _time = _startTime;
        }

        _timer.ChangeText((_time / 1000F).ToString("0.0"));
        _timer.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _button.Draw(spriteBatch);
        _timer.Draw(spriteBatch);
    }
}