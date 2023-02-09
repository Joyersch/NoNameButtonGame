using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.Hitboxes;
using NoNameButtonGame.Interfaces;
using NoNameButtonGame.Text;

namespace NoNameButtonGame.GameObjects.Groups;

public class ValueSelection : GameObject, IMoveable
{
    private readonly TextBuilder _display;
    private readonly SquareTextButton _decreaseButton;
    private readonly SquareTextButton _increaseButton;
    
    private readonly string left = Letter.ReverseParse(Letter.Character.Left).ToString();
    private readonly string right = Letter.ReverseParse(Letter.Character.Right).ToString();

    public List<string> ValidValues { get; private set; }

    public string Value => ValidValues[_pointer];
    
    private int _pointer;
    
    public ValueSelection(Vector2 position, Vector2 size,List<string> validValues, int startValueIndex) : base(position, size)
    {
        ValidValues = validValues;
        _pointer = startValueIndex;
        _decreaseButton = new SquareTextButton(position, left, left);
        _decreaseButton.ClickEventHandler += DecreaseClicked;
        _display = new TextBuilder(validValues[_pointer], Vector2.One);
        
        _increaseButton = new SquareTextButton(Vector2.Zero, right, right);
        _increaseButton.ClickEventHandler += IncreaseClicked;
        Move(Position);
    }

    private void IncreaseClicked(object obj)
    {
        _pointer++;
        if (_pointer > ValidValues.Count - 1)
            _pointer = 0;
        _display.ChangeText(Value);
        Move(Position);
    }

    private void DecreaseClicked(object obj)
    {
        _pointer--;
        if (_pointer < 0)
            _pointer = ValidValues.Count - 1;
        _display.ChangeText(Value);
        Move(Position);
    }

    public Vector2 GetPosition()
        => Position;

    public bool Move(Vector2 newPosition)
    {
        var cache1 = _display.Position;
        var cache2 = _decreaseButton.Position;
        var cache3 = _increaseButton.Position;
        
        var x1 = _decreaseButton.Move(newPosition);
        
        var x2 = _display.Move(_decreaseButton.Position + new Vector2(_decreaseButton.Rectangle.Width + 4,
            _decreaseButton.Rectangle.Height / 2 - _display.Rectangle.Height / 2));
        
        var x3 = _increaseButton.Move(_display.Position + new Vector2(_display.Rectangle.Width + 4,
            _display.Rectangle.Height / 2 - _increaseButton.Rectangle.Height / 2));
        
        if (!(x1 && x2 && x3))
        {
            _display.Position = cache1;
            _decreaseButton.Position = cache2;
            _increaseButton.Position = cache3;
        }
        return x1 && x2 && x3;
    }
    
    public override void Initialize()
    {
        _textureHitboxMapping = Mapping.GetMappingFromCache<ValueSelection>();
    }

    public void Update(GameTime gameTime, Rectangle mousePointer)
    {
        _display.Update(gameTime);
        _increaseButton.Update(gameTime, mousePointer);
        _decreaseButton.Update(gameTime, mousePointer);
        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _display.Draw(spriteBatch);
        _increaseButton.Draw(spriteBatch);
        _decreaseButton.Draw(spriteBatch);
    }
}