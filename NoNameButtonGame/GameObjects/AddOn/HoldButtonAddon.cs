using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.TextSystem;
using NoNameButtonGame.Interfaces;

namespace NoNameButtonGame.GameObjects.AddOn;

public class HoldButtonAddon : ButtonAddonBase
{
    private readonly ButtonAddonAdapter _button;
    private readonly TextSystem.Text _timer;
    private readonly float _startTime;
    private bool _isHover;
    private float _time;
    private bool hasReachedZero;
    private bool pressStartOnObject;
    private int _offset;

    public HoldButtonAddon(ButtonAddonAdapter button, float startTime) : base(button)
    {
        _button = button;
        _startTime = startTime;
        _time = _startTime;
        _timer = new TextSystem.Text((_startTime / 1000F).ToString("0.0"),
            button.Position);
        pressStartOnObject = !InputReaderMouse.CheckKey(InputReaderMouse.MouseKeys.Left, false);
    }
    
    public override int GetIndicatorOffset()
        => _button.GetIndicatorOffset() + Rectangle.Size.X;

    protected override void ButtonCallback(object sender, IButtonAddon.CallState state)
    {
        if (state == IButtonAddon.CallState.Enter)
            _isHover = true;
        if (state == IButtonAddon.CallState.Leave)
            _isHover = false;
     if (_time == 0)
         base.ButtonCallback(sender, state);
    }
    
    public override void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
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
                base.ButtonCallback(_button, IButtonAddon.CallState.Click);
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
    
    public override Vector2 GetPosition()
        => _button.GetPosition();

    public override Vector2 GetSize()
        => _button.GetSize();

    public override void Move(Vector2 newPosition)
    {
        _button.Move(newPosition);
        _timer.Move(newPosition);
        Position = newPosition + new Vector2(_offset, 0);
    }

    public override void SetDrawColor(Color color)
        => _button.SetDrawColor(color);

    public override Rectangle GetRectangle()
        => _button.GetRectangle();
}