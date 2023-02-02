﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.Hitboxes;

namespace NoNameButtonGame.Text;

public class TextBuilder
{
    private Letter[] _letters;
    private int spacing;
    private int _inGameLength;
    private string represent;
    public Vector2 Position;
    public Vector2 Size;
    public Rectangle rectangle;
    public string Text => represent;

    public Letter this[int i] => _letters[i];

    public int Length => _letters.Length;

    public TextBuilder(string text, Vector2 position, Vector2 letterSize, Color[] color, int spacing) : this(text,
        position, letterSize, spacing)
    {
    }

    public TextBuilder(string text, Vector2 position) : this(text, position, DefaultLetterSize, 0)
    {
    }

    public TextBuilder(string text, Vector2 position, Vector2 letterSize, int spacing)
    {
        this.spacing = spacing;
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

    public void ChangeText(string text)
    {
        represent = text;
        CreateLetters(ParseArray(text.ToCharArray()));
    }

    private void CreateLetters(Letter.Character[] characters)
    {
        _letters = new Letter[characters.Length];
        int currentStringLength = 0;
        for (int i = 0; i < characters.Length; i++)
        {
            _letters[i] = new Letter(new Vector2(Position.X + currentStringLength, Position.Y), Size,
                characters[i]);
            currentStringLength += _letters[i].frameSpace.Width * ((int) Size.X / 8)
                                   +
                                   _letters[i].frameSpace.X * ((int) Size.X / 8)
                                   +
                                   (spacing + 1) * ((int) Size.X / 8);
        }

        _inGameLength = currentStringLength;
    }

    private Letter.Character[] ParseArray(char[] text)
        => text.Select(Letter.Parse).ToArray();

    public void Update(GameTime gameTime)
    {
        represent = BuildString(_letters);
        for (int i = 0; i < _letters.Length; i++)
        {
            _letters[i].Update(gameTime);
        }

        rectangle = new Rectangle(Position.ToPoint(),
            new Point(_inGameLength + (spacing + 1) * (_letters.Length - 1), (int) Size.Y));
    }

    public override string ToString()
        => BuildString(_letters);

    public void Draw(SpriteBatch spriteBatch)
    {
        for (int i = 0; i < _letters.Length; i++)
        {
            _letters[i].Draw(spriteBatch);
        }
    }

    public static string BuildString(Letter[] letters)
    {
        string build = string.Empty;
        foreach (var letter in letters)
        {
            build += Letter.ReverseParse(letter.RepresentingCharacter);
        }

        return build;
    }

    public static Vector2 DefaultLetterSize => new Vector2(16, 16);
}