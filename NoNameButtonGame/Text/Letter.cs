using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.GameObjects;

namespace NoNameButtonGame.Text;

class Letter : GameObject
{
    public Rectangle frameSpace;
    public Character RepresentingCharacter;

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
        RepresentingCharacter = character;
        UpdateCharacter(character);
        rectangle = new Rectangle((position + frameSpace.Location.ToVector2()).ToPoint(),
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
        RepresentingCharacter = character;
    }

    private static Rectangle GetCharacterSpacing(Character character)
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

    public static Letter.Character Parse(char character)
        => character switch
        {
            '0' => Letter.Character.Zero,
            '1' => Letter.Character.One,
            '2' => Letter.Character.Two,
            '3' => Letter.Character.Three,
            '4' => Letter.Character.Four,
            '5' => Letter.Character.Five,
            '6' => Letter.Character.Six,
            '7' => Letter.Character.Seven,
            '8' => Letter.Character.Eight,
            '9' => Letter.Character.Nine,
            'a' => Letter.Character.A,
            'A' => Letter.Character.A,
            'b' => Letter.Character.B,
            'B' => Letter.Character.B,
            'c' => Letter.Character.C,
            'C' => Letter.Character.C,
            'd' => Letter.Character.D,
            'D' => Letter.Character.D,
            'e' => Letter.Character.E,
            'E' => Letter.Character.E,
            'f' => Letter.Character.F,
            'F' => Letter.Character.F,
            'g' => Letter.Character.G,
            'G' => Letter.Character.G,
            'h' => Letter.Character.H,
            'H' => Letter.Character.H,
            'i' => Letter.Character.I,
            'I' => Letter.Character.I,
            'j' => Letter.Character.J,
            'J' => Letter.Character.J,
            'k' => Letter.Character.K,
            'K' => Letter.Character.K,
            'l' => Letter.Character.L,
            'L' => Letter.Character.L,
            'm' => Letter.Character.M,
            'M' => Letter.Character.M,
            'n' => Letter.Character.N,
            'N' => Letter.Character.N,
            'o' => Letter.Character.O,
            'O' => Letter.Character.O,
            'p' => Letter.Character.P,
            'P' => Letter.Character.P,
            'q' => Letter.Character.Q,
            'Q' => Letter.Character.Q,
            'r' => Letter.Character.R,
            'R' => Letter.Character.R,
            's' => Letter.Character.S,
            'S' => Letter.Character.S,
            't' => Letter.Character.T,
            'T' => Letter.Character.T,
            'u' => Letter.Character.U,
            'U' => Letter.Character.U,
            'v' => Letter.Character.V,
            'V' => Letter.Character.V,
            'w' => Letter.Character.W,
            'W' => Letter.Character.W,
            'x' => Letter.Character.X,
            'X' => Letter.Character.X,
            'y' => Letter.Character.Y,
            'Y' => Letter.Character.Y,
            'z' => Letter.Character.Z,
            'Z' => Letter.Character.Z,
            '!' => Letter.Character.Exclamation,
            '?' => Letter.Character.Question,
            '/' => Letter.Character.Slash,
            '-' => Letter.Character.Minus,
            '<' => Letter.Character.SmallerAs,
            '=' => Letter.Character.Equal,
            '>' => Letter.Character.BiggerAs,
            '*' => Letter.Character.Star,
            '+' => Letter.Character.Plus,
            '%' => Letter.Character.Percent,
            '(' => Letter.Character.OpenBracket,
            ')' => Letter.Character.CloseBracket,
            ';' => Letter.Character.Semicolon,
            '.' => Letter.Character.Dot,
            ' ' => Letter.Character.Space,
            '✔' => Letter.Character.Checkmark,
            '❌' => Letter.Character.Crossout,
            '⬇' => Letter.Character.Down,
            '⬆' => Letter.Character.Up,
            '_' => Letter.Character.Line,
            ':' => Letter.Character.DoubleDots,
            ',' => Letter.Character.Komma,
            '⬅' => Letter.Character.Left,
            '➡' => Letter.Character.Right,
            '\"' => Letter.Character.Parentheses,
            '\\' => Letter.Character.Backslash,
            '⬜' => Letter.Character.Full,
            _ => Letter.Character.Full,
        };
    
    public static char ReverseParse(Character character)
        => character switch
        {
            Letter.Character.Zero => '0',
            Letter.Character.One => '1',
            Letter.Character.Two => '2',
            Letter.Character.Three => '3',
            Letter.Character.Four => '4',
            Letter.Character.Five => '5',
            Letter.Character.Six => '6',
            Letter.Character.Seven => '7',
            Letter.Character.Eight => '8',
            Letter.Character.Nine => '9',
            Letter.Character.A => 'A',
            Letter.Character.B => 'B',
            Letter.Character.C => 'C',
            Letter.Character.D => 'D',
            Letter.Character.E => 'E',
            Letter.Character.F => 'F',
            Letter.Character.G => 'G',
            Letter.Character.H => 'H',
            Letter.Character.I => 'I',
            Letter.Character.J => 'J',
            Letter.Character.K => 'K',
            Letter.Character.L => 'L',
            Letter.Character.M => 'M',
            Letter.Character.N => 'N',
            Letter.Character.O => 'O',
            Letter.Character.P => 'P',
            Letter.Character.Q => 'Q',
            Letter.Character.R => 'R',
            Letter.Character.S => 'S',
            Letter.Character.T => 'T',
            Letter.Character.U => 'U',
            Letter.Character.V => 'V',
            Letter.Character.W => 'W',
            Letter.Character.X => 'X',
            Letter.Character.Y => 'Y',
            Letter.Character.Z => 'Z',
            Letter.Character.Exclamation => '!',
            Letter.Character.Question => '?',
            Letter.Character.Slash => '/',
            Letter.Character.Minus => '-',
            Letter.Character.SmallerAs => '<',
            Letter.Character.Equal => '=',
            Letter.Character.BiggerAs => '>',
            Letter.Character.Star => '*',
            Letter.Character.Plus => '+',
            Letter.Character.Percent => '%',
            Letter.Character.OpenBracket => '(',
            Letter.Character.CloseBracket => ')',
            Letter.Character.Semicolon => ';',
            Letter.Character.Dot => '.',
            Letter.Character.Space => ' ',
            Letter.Character.Checkmark => '✔',
            Letter.Character.Crossout => '❌',
            Letter.Character.Down => '⬇',
            Letter.Character.Up => '⬆',
            Letter.Character.Line => '_',
            Letter.Character.DoubleDots => ':',
            Letter.Character.Komma => ',',
            Letter.Character.Left => '⬅',
            Letter.Character.Right => '➡',
            Letter.Character.Parentheses => '\"',
            Letter.Character.Backslash => '\\',
            Letter.Character.Full => '⬜',
        };
}