using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.Interfaces;

namespace NoNameButtonGame.GameObjects.Text;

public class TextBuilder : IColorable, IMoveable
{
    private Letter[] _letters;
    private readonly int _spacing;
    private string _represent;
    public Vector2 Position;
    public Vector2 Size;
    public Rectangle Rectangle;
    public string Text => _represent;

    public Letter this[int i] => _letters[i];

    public int Length => _letters.Length;

    public static Vector2 DefaultLetterSize => new Vector2(16, 16);

    public TextBuilder(string text, Vector2 position) : this(text, position, DefaultLetterSize, 1)
    {
    }

    public TextBuilder(string text, Vector2 position, float scale) : this(text, position, DefaultLetterSize * scale, 1)
    {
    }

    public TextBuilder(string text, Vector2 position, float scale, int spacing) : this(text, position,
        DefaultLetterSize * scale, spacing)
    {
    }

    public TextBuilder(string text, Vector2 position, Vector2 letterSize, int spacing)
    {
        _spacing = spacing;
        Size = letterSize;
        Position = position;

        ChangeText(text);
    }

    public void ChangePosition(Vector2 position)
    {
        foreach (Letter letter in _letters)
            letter.Position += position - Position;
        Position = position;
    }

    public void ChangeColor(Color[] color)
    {
        for (int i = 0; i < color.Length; i++)
        {
            if (_letters.Length > i)
                _letters[i].ChangeColor(color[i]);
        }
    }

    public int ColorLength()
        => Length;

    public void ChangeColor(Color color)
    {
        for (int i = 0; i < _letters.Length; i++)
        {
            _letters[i].ChangeColor(color);
        }
    }

    public void ChangeText(string text)
    {
        _represent = text;
        CreateLetters(ParseArray(text.ToCharArray()));
    }

    private void CreateLetters(Letter.Character[] characters)
    {
        var letters = new List<Letter>();

        int length = 0;
        float sizeScale = Size.X / 8;
        foreach (Letter.Character character in characters)
        {
            var letter = new Letter(new Vector2(length, 0) + Position, Size, character);
            letter.Position += new Vector2(0, 8F * letter.InitialScale.Y) - new Vector2(0, letter.Rectangle.Height);
            length += (int) ((letter.FrameSpacing.Width + _spacing) * sizeScale);
            letters.Add(letter);
        }

        _letters = letters.ToArray();
        UpdateRectangle();
    }

    private Letter.Character[] ParseArray(char[] text)
        => text.Select(Letter.Parse).ToArray();

    public virtual void Update(GameTime gameTime)
    {
        _represent = BuildString(_letters);

        System.Array.ForEach(_letters, c => c.Update(gameTime));

        UpdateRectangle();
    }

    private void UpdateRectangle()
    {
        Rectangle combination = Rectangle.Empty;
        foreach (Letter l in _letters)
        {
            if (combination.IsEmpty)
                combination = l.Rectangle;
            else
                Rectangle.Union(ref combination, ref l.Rectangle, out combination);
        }

        Rectangle = combination;
    }

    public override string ToString()
        => BuildString(_letters);

    public virtual void Draw(SpriteBatch spriteBatch)
    => System.Array.ForEach(_letters, c => c.Draw(spriteBatch));

    public static string BuildString(Letter[] letters)
    {
        string build = string.Empty;
        foreach (var letter in letters)
        {
            build += Letter.ReverseParse(letter.RepresentingCharacter);
        }

        return build;
    }

    public Vector2 GetPosition()
        => Position;

    public bool Move(Vector2 newPosition)
    {
        ChangePosition(newPosition);
        return true;
    }
}