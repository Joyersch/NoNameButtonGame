using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.Hitboxes;

namespace NoNameButtonGame.Text;

public class Letter : GameObject
{
    public Rectangle frameSpacing;
    public Character RepresentingCharacter;
    private Rectangle drawOffset;

    public Letter(Vector2 position, Vector2 size, Character character) : this(position, size, character, Color.White)
    {
    }

    public Letter(Vector2 position, Vector2 size, Character character, Color color) : base(position, size)
    {
        RepresentingCharacter = character;
        UpdateCharacter(character);
        _textureHitboxMapping.Hitboxes = new[]
        {
            new Rectangle((position + frameSpacing.Location.ToVector2()).ToPoint(),
                (frameSpacing.Size.ToVector2() * _scale).ToPoint())
        };
        _hitboxes = new Rectangle[1];
    }

    public override void Initialize()
    {
        _textureHitboxMapping = Mapping.GetMappingFromCache<Letter>();
    }

    public void ChangeColor(Color color)
    {
        DrawColor = color;
    }

    public void UpdateCharacter(Character character)
    {
        frameSpacing = GetCharacterSpacing(character);
        var inImagePosition = new Rectangle(new Point((int) character % 5 * 8, (int) character / 5 * 8),
            (FrameSize).ToPoint());
        ImageLocation = new Rectangle(
            inImagePosition.X + frameSpacing.X
            , inImagePosition.Y + frameSpacing.Y
            , frameSpacing.Width
            , frameSpacing.Height
        );
        Size = frameSpacing.Size.ToVector2() * _scale;
        UpdateRectangle();
        RepresentingCharacter = character;
    }

    private static Rectangle GetCharacterSpacing(Character character)
    {
        return character switch
        {
            Character.Zero => new Rectangle(1, 0, 5, 7),
            Character.One => new Rectangle(1, 0, 4, 7),
            Character.Two => new Rectangle(1, 0, 5, 7),
            Character.Three => new Rectangle(1, 0, 4, 7),
            Character.Four => new Rectangle(1, 0, 5, 7),
            Character.Five => new Rectangle(1, 0, 6, 7),
            Character.Six => new Rectangle(1, 0, 5, 7),
            Character.Seven => new Rectangle(1, 0, 6, 7),
            Character.Eight => new Rectangle(1, 0, 5, 7),
            Character.Nine => new Rectangle(1, 0, 5, 7),
            Character.A => new Rectangle(1, 0, 5, 7),
            Character.B => new Rectangle(1, 0, 5, 7),
            Character.C => new Rectangle(1, 0, 4, 7),
            Character.D => new Rectangle(1, 0, 5, 7),
            Character.E => new Rectangle(1, 0, 5, 7),
            Character.F => new Rectangle(1, 0, 5, 7),
            Character.G => new Rectangle(1, 0, 5, 7),
            Character.H => new Rectangle(1, 0, 5, 7),
            Character.I => new Rectangle(1, 0, 1, 7),
            Character.J => new Rectangle(1, 0, 2, 7),
            Character.K => new Rectangle(1, 0, 4, 7),
            Character.L => new Rectangle(1, 0, 4, 7),
            Character.M => new Rectangle(0, 0, 6, 7),
            Character.N => new Rectangle(1, 0, 5, 7),
            Character.O => new Rectangle(1, 0, 5, 7),
            Character.P => new Rectangle(1, 0, 5, 7),
            Character.Q => new Rectangle(1, 0, 5, 7),
            Character.R => new Rectangle(1, 0, 5, 7),
            Character.S => new Rectangle(1, 0, 5, 7),
            Character.T => new Rectangle(1, 0, 5, 7),
            Character.U => new Rectangle(1, 0, 5, 7),
            Character.V => new Rectangle(1, 0, 5, 7),
            Character.W => new Rectangle(0, 0, 7, 7),
            Character.X => new Rectangle(1, 0, 5, 7),
            Character.Y => new Rectangle(1, 0, 5, 7),
            Character.Z => new Rectangle(1, 0, 5, 7),
            Character.Exclamation => new Rectangle(1, 0, 1, 7),
            Character.Question => new Rectangle(1, 0, 5, 7),
            Character.Slash => new Rectangle(2, 0, 4, 8),
            Character.Minus => new Rectangle(2, 4, 4, 4),
            Character.SmallerAs => new Rectangle(1, 1, 3, 6),
            Character.Equal => new Rectangle(1, 2, 6, 5),
            Character.BiggerAs => new Rectangle(2, 1, 3, 6),
            Character.Asterisk => new Rectangle(1, 1, 5, 6),
            Character.Plus => new Rectangle(1, 1, 5, 6),
            Character.Percent => new Rectangle(0, 0, 8, 8),
            Character.OpenBracket => new Rectangle(1, 0, 2, 7),
            Character.CloseBracket => new Rectangle(1, 0, 2, 7),
            Character.Semicolon => new Rectangle(1, 2, 2, 5),
            Character.Dot => new Rectangle(0, 6, 1, 1),
            Character.Space => new Rectangle(0, 0, 2, 0),
            Character.Checkmark => new Rectangle(0, 1, 8, 6),
            Character.Crossout => new Rectangle(0, 1, 7, 7),
            Character.Down => new Rectangle(0, 2, 8, 6),
            Character.Up => new Rectangle(0, 2, 8, 6),
            Character.Line => new Rectangle(0, 7, 8, 1),
            Character.DoubleDots => new Rectangle(3, 2, 1, 6),
            Character.Komma => new Rectangle(2, 4, 2, 3),
            Character.Left => new Rectangle(2, 0, 4, 8),
            Character.Right => new Rectangle(2, 0, 4, 8),
            Character.Parentheses => new Rectangle(1, 0, 5, 8),
            Character.Backslash => new Rectangle(2, 0, 4, 8),
            _ => new Rectangle(0, 0, 8, 8)
        };
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
        Asterisk,
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
            '*' => Letter.Character.Asterisk,
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
            Letter.Character.Asterisk => '*',
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