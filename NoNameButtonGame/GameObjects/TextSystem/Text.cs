using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.Interfaces;

namespace NoNameButtonGame.GameObjects.TextSystem;

public class Text : IColorable, IMoveable, IManageable
{
    private List<Letter> _letters;
    protected readonly int Spacing;
    private string _represent;
    public Vector2 Position;
    public Vector2 Size;
    public Rectangle Rectangle;
    public string Value => _represent;

    public bool IsStatic;

    public Letter this[int i] => _letters[i];

    public int Length => _letters.Count;

    public static Vector2 DefaultLetterSize => new Vector2(16, 16);

    public Text(string text) : this(text, Vector2.Zero, 1, 1)
    {
    }
    
    public Text(string text, float scale) : this(text, Vector2.Zero, DefaultLetterSize * scale, 1)
    {
    }

    public Text(string text, Vector2 position) : this(text, position, DefaultLetterSize, 1)
    {
    }

    public Text(string text, Vector2 position, float scale) : this(text, position, DefaultLetterSize * scale, 1)
    {
    }

    public Text(string text, Vector2 position, float scale, int spacing) : this(text, position,
        DefaultLetterSize * scale, spacing)
    {
    }

    public Text(string text, Vector2 position, Vector2 letterSize, int spacing)
    {
        Spacing = spacing;
        Size = letterSize;
        Position = position;

        ChangeText(text);
    }

    private void ChangePosition(Vector2 newPosition)
    {
        foreach (Letter letter in _letters)
            letter.Move(letter.Position + (newPosition - Position));
        Position = newPosition;
    }

    public void ChangeColor(Color[] color)
    {
        for (int i = 0; i < color.Length; i++)
        {
            if (_letters.Count > i)
                _letters[i].ChangeColor(color[i]);
        }
    }

    public int ColorLength()
        => Length;

    public void ChangeColor(Color color)
    {
        for (int i = 0; i < _letters.Count; i++)
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
            length += (int) ((letter.FrameSpacing.Width + Spacing) * sizeScale);
            letters.Add(letter);
        }

        _letters = letters;
        UpdateRectangle();
    }

    private Letter.Character[] ParseArray(char[] text)
        => text.Select(Letter.Parse).ToArray();

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
            if (combination.IsEmpty)
                combination = letter.Rectangle;
            else
                Rectangle.Union(ref combination, ref letter.Rectangle, out combination);
        }

        Rectangle = combination;
    }

    public override string ToString()
        => BuildString();

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        if (IsStatic)
            return;
        GeneralDraw(spriteBatch);
    }
    
    public virtual void DrawStatic(SpriteBatch spriteBatch)
    {
        if (!IsStatic)
            return;
        GeneralDraw(spriteBatch);
    }
    
    protected virtual void GeneralDraw(SpriteBatch spriteBatch)
    {
        foreach (var letter in _letters)
        {
            letter.Draw(spriteBatch);
        }
    } 

    private string BuildString()
    {
        string build = string.Empty;

        foreach (var letter in _letters)
        {
            build += Letter.ReverseParse(letter.RepresentingCharacter);
        }

        return build;
    }

    public Vector2 GetPosition()
        => Position;

    public Vector2 GetSize()
        => Rectangle.Size.ToVector2();

    public void Move(Vector2 newPosition)
    {
        ChangePosition(newPosition);
        UpdateRectangle();
    }
}