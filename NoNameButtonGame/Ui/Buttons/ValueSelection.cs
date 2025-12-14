using System;
using System.Collections.Generic;
using Joyersch.Monogame;
using Joyersch.Monogame.Ui.Buttons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.Ui.Text;

namespace NoNameButtonGame.Ui.Buttons;

public class ValueSelection<T> : IManageable, IMoveable, IInteractable, IScaleable
{
    private readonly BasicText _display;
    private readonly SquareTextButton _decreaseButton;
    private readonly SquareTextButton _increaseButton;

    public Rectangle[] Hitbox
    {
        get
        {
            if (_hitbox.Length > 0)
                return _hitbox;

            List<Rectangle> recs = [];
            recs.AddRange(_decreaseButton.Hitbox);
            recs.AddRange(_increaseButton.Hitbox);
            _hitbox = recs.ToArray();
            return _hitbox;
        }
}
    private Rectangle[] _hitbox = [];

    private Vector2 _position;
    private Vector2 _size;
    private Rectangle _rectangle;

    private readonly float _initialScale;
    private float _extendedScale = 1f;
    public float Scale => _extendedScale * _initialScale;
    public Rectangle Rectangle => _rectangle;

    private readonly string _left = "[left]";
    private readonly string _right = "[right]";

    public event Action<object> ValueChanged;

    public List<T> ValidValues { get; private set; }

    public string Value => ValidValues[_pointer].ToString();

    private int _pointer;
    private int _longestValidValue;

    public bool LoopOverValues;

    private float _maxTextLength = 0f;

    public ValueSelection(Vector2 position, float initialScale, List<T> validValues, int startValueIndex)
    {
        ValidValues = validValues;
        _position = position;
        _initialScale = initialScale;
        _pointer = startValueIndex;

        _decreaseButton = new SquareTextButton(_left, position, initialScale * 4f);
        _decreaseButton.Click += DecreaseClicked;

        _increaseButton = new SquareTextButton(_right, Vector2.Zero, initialScale * 4f);
        _increaseButton.Click += IncreaseClicked;

        _display = new BasicText(validValues[_pointer].ToString(), Vector2.Zero, initialScale * 2f);

        foreach (var value in validValues)
        {
            var display = new BasicText(value.ToString(), Vector2.Zero, 2f);
            var length = display.Rectangle.Size.X;
            if (length > _maxTextLength)
                _maxTextLength = length;
        }
        
        UpdateTextValue();
    }

    private void IncreaseClicked(object obj)
    {
        _pointer++;
        if (_pointer > ValidValues.Count - 1)
            _pointer = LoopOverValues ? 0 : ValidValues.Count - 1;
        UpdateTextValue();
        ValueChanged?.Invoke(ValidValues[_pointer]);
    }

    private void DecreaseClicked(object obj)
    {
        _pointer--;
        if (_pointer < 0)
            _pointer = LoopOverValues ? ValidValues.Count - 1 : 0;
        UpdateTextValue();
        ValueChanged?.Invoke(ValidValues[_pointer]);
    }

    private void UpdateTextValue()
    {
        _display.ChangeText(Value);
        float textLength = _maxTextLength * Scale + _decreaseButton.Rectangle.Size.X * 1.25f * 2f;

        _size = new Vector2(textLength, _decreaseButton.Rectangle.Size.Y);
        _rectangle = new Rectangle(_position.ToPoint(), _size.ToPoint());

        _display.InRectangle(this)
            .OnCenter()
            .Centered()
            .Apply();

        _decreaseButton.GetAnchor(this)
            .SetMainAnchor(AnchorCalculator.Anchor.Left)
            .SetSubAnchor(AnchorCalculator.Anchor.Left)
            .Apply();

        _increaseButton.GetAnchor(this)
            .SetMainAnchor(AnchorCalculator.Anchor.Right)
            .SetSubAnchor(AnchorCalculator.Anchor.Right)
            .Apply();
    }

    public bool UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        bool @return = false;
        @return |= _increaseButton.UpdateInteraction(gameTime, toCheck);
        @return |= _decreaseButton.UpdateInteraction(gameTime, toCheck);
        return @return;
    }

    public void Update(GameTime gameTime)
    {
        _display.Update(gameTime);
        _increaseButton.Update(gameTime);
        _decreaseButton.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _display.Draw(spriteBatch);
        _increaseButton.Draw(spriteBatch);
        _decreaseButton.Draw(spriteBatch);
    }

    public void SetScale(ScaleProvider provider)
    {
        _display.SetScale(provider);
        _increaseButton.SetScale(provider);
        _decreaseButton.SetScale(provider);
        _extendedScale = provider.Scale;
        UpdateTextValue();
    }

    public Vector2 GetPosition()
        => _position;

    public Vector2 GetSize()
        => _size;

    public void Move(Vector2 newPosition)
    {
        _position = newPosition;
        UpdateTextValue();
    }
}