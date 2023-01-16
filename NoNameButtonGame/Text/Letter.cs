using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.GameObjects;

namespace NoNameButtonGame.Text;

class Letter : GameObject
{
    public Rectangle frameSpace;

    public Letter(Vector2 position, Vector2 size, Character character) : this(position, size, character, Color.White)
    {
    }

    public Letter(Vector2 position, Vector2 size, Character character, Color color)
    {
        Texture = Globals.Content.Load<Texture2D>("font");
        FrameSize = new Vector2(8, 8);
        Position = position;
        Size = size;
        DrawColor = color;

        UpdateCharacter(character);
        rec = new Rectangle((position + frameSpace.Location.ToVector2()).ToPoint(),
            (size + frameSpace.Size.ToVector2()).ToPoint());
    }

    public void ChangeColor(Color color)
    {
        DrawColor = color;
    }

    public void UpdateCharacter(Character character)
    {
        frameSpace = GetCharacterSpacing(character);
        ImageLocation = new Rectangle(new Point((int) character % 5 * 8, (int) character / 5 * 8),
            (FrameSize).ToPoint());
    }

    private Rectangle GetCharacterSpacing(Character character)
    {
        switch (character)
        {
            case Character.Zero:
            case Character.One:
            case Character.Two:
            case Character.Four:
            case Character.Five:
            case Character.Six:
            case Character.Seven:
            case Character.Eight:
            case Character.Nine:
            case Character.B:
            case Character.D:
            case Character.E:
            case Character.F:
            case Character.G:
            case Character.H:
            case Character.N:
            case Character.O:
            case Character.P:
            case Character.Q:
            case Character.R:
            case Character.S:
            case Character.T:
            case Character.U:
            case Character.V:
            case Character.X:
            case Character.Y:
            case Character.Z:
            case Character.Question:
                return new Rectangle(1, 0, 5, 7);
            case Character.A:
                return new Rectangle(1, 0, 6, 7);
            case Character.Three:
            case Character.C:
            case Character.K:
            case Character.L:
                return new Rectangle(1, 0, 4, 7);
            case Character.I:
            case Character.Exclamation:
                return new Rectangle(1, 0, 1, 7);
            case Character.Dot:
                return new Rectangle(0, 0, 0, 7);
            case Character.J:
            case Character.OpenBracket:
            case Character.CloseBracket:
            case Character.Semicolon:
                return new Rectangle(1, 0, 2, 7);
            case Character.M:
                return new Rectangle(2, 0, 4, 7);
            case Character.W:
                return new Rectangle(0, 0, 7, 7);
            case Character.Crossout:
                return new Rectangle(0, 0, 8, 8);
            case Character.Slash:
                return new Rectangle(2, 0, 4, 8);
            case Character.Minus:
                return new Rectangle(2, 0, 4, 1);
            case Character.SmallerAs:
            case Character.BiggerAs:
                return new Rectangle(2, 0, 3, 5);
            case Character.Equal:
                return new Rectangle(1, 0, 6, 3);
            case Character.Star:
            case Character.Plus:
                return new Rectangle(1, 0, 5, 5);
            case Character.Down:
            case Character.Up:
                return new Rectangle(0, 2, 8, 4);
            default:
                return new Rectangle(0, 0, 8, 8);
        }
    }

    public enum Character
    {
        Zero,
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        A,
        B,
        C,
        D,
        E,
        F,
        G,
        H,
        I,
        J,
        K,
        L,
        M,
        N,
        O,
        P,
        Q,
        R,
        S,
        T,
        U,
        V,
        W,
        X,
        Y,
        Z,
        Exclamation,
        Question,
        Slash,
        Minus,
        SmallerAs,
        Equal,
        BiggerAs,
        Star,
        Plus,
        Percent,
        OpenBracket,
        CloseBracket,
        Semicolon,
        Dot,
        Space,
        Checkmark,
        Crossout,
        Down,
        Up,
        Full,
        Line,
        DoubleDots,
        Komma,
        Left,
        Right,
        Parentheses,
        Backslash,
    }
}