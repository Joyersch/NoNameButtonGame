using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logic;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Objects;
using MonoUtils.Objects.TextSystem;
using MonoUtils.Ui.TextSystem;
using NoNameButtonGame.GameObjects.Buttons.TexturedButtons;

namespace NoNameButtonGame.LevelSystem.Settings;

public class ValueSelection : GameObject, IMoveable, IInteractable
{
    private readonly Text _display;
    private readonly SquareTextButton _decreaseButton;
    private readonly SquareTextButton _increaseButton;

    private readonly string _left = Letter.ReverseParse(Letter.Character.Left).ToString();
    private readonly string _right = Letter.ReverseParse(Letter.Character.Right).ToString();

    public event Action<string> ValueChanged;

    public List<string> ValidValues { get; private set; }

    public string Value => ValidValues[_pointer];

    public bool LoopOverValues = false;

    private int _pointer;
    
    public ValueSelection(Vector2 position, float scale, List<string> validValues, int startValueIndex) : base(
        position, SquareTextButton.DefaultSize * scale, DefaultTexture, DefaultMapping)
    {
        ValidValues = validValues;
        _pointer = startValueIndex;
        _decreaseButton = new SquareTextButton(position, scale, _left, _left);
        _decreaseButton.Click += DecreaseClicked;
        _display = new Text(validValues[_pointer], Vector2.One, scale);

        _increaseButton = new SquareTextButton(Vector2.Zero, scale, _right, _right);
        _increaseButton.Click += IncreaseClicked;
        Move(Position);
    }

    private void IncreaseClicked(object obj)
    {
        _pointer++;
        if (_pointer > ValidValues.Count - 1)
            _pointer = LoopOverValues ? 0 : ValidValues.Count - 1;
        UpdateTextValue();
    }

    private void DecreaseClicked(object obj)
    {
        _pointer--;
        if (_pointer < 0)
            _pointer = LoopOverValues ? ValidValues.Count - 1 : 0;
        UpdateTextValue();
    }

    private void UpdateTextValue()
    {
        _display.ChangeText(Value);
        Move(Position);
        ValueChanged?.Invoke(_display.Value);
    }

    public Vector2 GetPosition()
        => Position;

    public Vector2 GetSize()
        => Size;

    public void Move(Vector2 newPosition)
    {
        _decreaseButton.Move(newPosition);

        _display.Move(_decreaseButton.Position + new Vector2(_decreaseButton.Rectangle.Width + 4,
            _decreaseButton.Rectangle.Height / 2 - _display.Rectangle.Height / 2));

        _increaseButton.Move(_display.Position + new Vector2(_display.Rectangle.Width + 4,
            _display.Rectangle.Height / 2 - _increaseButton.Rectangle.Height / 2));
    }

    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        _increaseButton.UpdateInteraction(gameTime, toCheck);
        _decreaseButton.UpdateInteraction(gameTime, toCheck);
    }

    public override void Update(GameTime gameTime)
    {
        _display.Update(gameTime);
        _increaseButton.Update(gameTime);
        _decreaseButton.Update(gameTime);
        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _display.Draw(spriteBatch);
        _increaseButton.Draw(spriteBatch);
        _decreaseButton.Draw(spriteBatch);
    }

    protected override void UpdateRectangle()
    {
        Rectangle = Rectangle.Union(_display.Rectangle,
            Rectangle.Union(_decreaseButton.Rectangle, _increaseButton.Rectangle));
    }
}