using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NoNameButtonGame.Ui.Text;

public class BasicText : IColorable, IMoveable, IManageable, IScaleable
{
    private List<Letter> _letters;
    private string _text;
    protected readonly int Spacing;
    private float _extendedScale = 1F;
    private readonly float _baseScale;
    public float Scale => _baseScale * _extendedScale;

    public Vector2 Position;
    public Vector2 Size;
    public Rectangle Rectangle { get; private set; }

    public Letter this[int i] => _letters[i];

    public List<Letter> Letters => _letters;

    public int Length => _letters.Count;

    private readonly static float DefaultLetterScale = 2F;

    public BasicText(float scale) : this(string.Empty, Vector2.Zero, scale, 1)
    {
    }

    public BasicText(string text) : this(text, Vector2.Zero, DefaultLetterScale, 1)
    {
    }

    public BasicText(string text, float scale) : this(text, Vector2.Zero, scale, 1)
    {
    }

    public BasicText(string text, Vector2 position) : this(text, position, DefaultLetterScale, 1)
    {
    }

    public BasicText(string text, Vector2 position, float scale) : this(text, position, scale, 1)
    {
    }

    public BasicText(string text, Vector2 position, float scale, int spacing)
    {
        _text = text;
        Spacing = spacing;
        _baseScale = scale;
        Position = position;
        ChangeText(text);
    }

    public void ChangeText(string text)
    {
        _text = text;
        var letters = Letter.Parse(text, Scale);

        int length = 0;
        int index = 0;
        foreach (var letter in letters)
        {
            var position = Position;
            position.X += length;
            letter.Move(position + new Vector2(0, letter.FullSize.Y) - new Vector2(0, letter.Rectangle.Height));
            if (_letters is not null && _letters.Count > index)
                letter.DrawColor = _letters[index++].DrawColor;
            length += (int)(letter.Size.X + Spacing * Scale);
        }

        _letters = letters;
        UpdateRectangle();
    }


    public virtual void Update(GameTime gameTime)
    {
        foreach (var letter in _letters)
        {
            letter.Update(gameTime);
        }

        UpdateRectangle();
    }

    private void UpdateRectangle()
    {
        Rectangle combination = Rectangle.Empty;
        foreach (Letter letter in _letters)
        {
            Rectangle rec = letter.Rectangle;
            if (combination.IsEmpty)
                combination = letter.Rectangle;
            else
                Rectangle.Union(ref combination, ref rec, out combination);
        }

        Rectangle = combination;
        Size = Rectangle.Size.ToVector2();
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        foreach (var letter in _letters)
        {
            letter.Draw(spriteBatch);
        }
    }

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();
        foreach (var letter in _letters)
        {
            builder.Append(letter);
        }

        return builder.ToString();
    }

    public virtual Vector2 GetPosition()
        => Position;

    public virtual Vector2 GetSize()
        => Size;

    public virtual void Move(Vector2 newPosition)
    {
        Vector2 offset = newPosition - Position;
        foreach (Letter letter in _letters)
            letter.Move(letter.Position + offset);
        Position = newPosition;
        UpdateRectangle();
    }

    public virtual void ChangeColor(Microsoft.Xna.Framework.Color[] color)
    {
        for (int i = 0; i < color.Length; i++)
        {
            if (_letters.Count > i)
                _letters[i].DrawColor = color[i];
        }
    }

    public int ColorLength()
        => Length;

    public Microsoft.Xna.Framework.Color[] GetColor()
        => _letters.Select(l => l.DrawColor).ToArray();

    public virtual void ChangeColor(Microsoft.Xna.Framework.Color color)
    {
        for (int i = 0; i < _letters.Count; i++)
        {
            _letters[i].DrawColor = color;
        }
    }

    public virtual void SetScale(ScaleProvider provider)
    {
        _extendedScale = provider.Scale;
        ChangeText(_text);
    }
}