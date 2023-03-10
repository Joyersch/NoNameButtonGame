using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.TextSystem;
using NoNameButtonGame.Interfaces;

namespace NoNameButtonGame.GameObjects.AddOn;

public class HoldButtonAddon : GameObject, IInteractable
{
    public event Action TimerReachedZero;
    
    private readonly EmptyButton _button;
    private readonly TextSystem.Text _timer;
    private readonly float _startTime;
    private bool _isHover;
    private float _time;
    private bool hasReachedZero;
    private bool pressStartOnObject;

    public HoldButtonAddon(EmptyButton button, float startTime) : base(
        button.Position, button.Size, DefaultTexture, DefaultMapping)
    {
        _button = button;
        _startTime = startTime;
        _time = _startTime;
        button.Enter += o => _isHover = true; 
        button.Leave += o => _isHover = false; 
        _timer = new TextSystem.Text((_startTime / 1000F).ToString("0.0"),
            button.Position);
        pressStartOnObject = !InputReaderMouse.CheckKey(InputReaderMouse.MouseKeys.Left, false);
    }

    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        _button.UpdateInteraction(gameTime, toCheck);

        
        float passedGameTime = 0F;
        if (!hasReachedZero)
            passedGameTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        if (_isHover && InputReaderMouse.CheckKey(InputReaderMouse.MouseKeys.Left, false))
        {
            if (!pressStartOnObject)
            {
                pressStartOnObject = InputReaderMouse.CheckKey(InputReaderMouse.MouseKeys.Left, true);
                return;
            }
            
            _time -= passedGameTime;
            if (_time <= 0)
            {
                _time = 0;
                hasReachedZero = true;
                TimerReachedZero?.Invoke();
                
            }
        }
        else
        {
            _time += passedGameTime / 2;
            if (_time > _startTime)
                _time = _startTime;
        }

        
        
        string newText = string.Empty;
        
        // If _time has no value after decimal point there is no need to print the value after the decimal point
        if (Math.Floor(_time) == _time)
            newText = (_time / 1000).ToString("0");
        else
            newText = (_time / 1000).ToString("0.0");
        _timer.ChangeText(newText);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        _timer.Update(gameTime);
        _button.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _button.Draw(spriteBatch);
        _timer.Draw(spriteBatch);
    }
}