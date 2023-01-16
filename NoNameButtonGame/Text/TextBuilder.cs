using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.GameObjects;

namespace NoNameButtonGame.Text
{
    class TextBuilder : GameObject
    {
        Letter[] LetterAry;
        string text;
        Letter.Character[] characters;
        Color[] LColor;
        int spacing;
        int Length;

        public int Spacing
        {
            get { return spacing; }
            set
            {
                spacing = value;
                CreateLetters();
            }
        }

        public string Text
        {
            get { return text; }
        }

        public TextBuilder(string InitText, Vector2 Position, Vector2 LSize, Color[] LColor, int Spacing)
        {
            this.Size = LSize;
            spacing = Spacing;
            this.Position = Position;
            if (LColor == null)
            {
                LColor = new Color[InitText.Length];
                for (int i = 0; i < InitText.Length; i++)
                {
                    LColor[i] = Color.White;
                }
            }

            if (InitText.Length != LColor.Length)
            {
                throw new Exception("InitText and LColor length do not match!");
            }

            ChangeText(InitText, LColor);
        }

        public void ChangePosition(Vector2 Pos)
        {
            Position = Pos;
            CreateLetters();
        }

        public void ChangeColor(Color[] LColor)
        {
            if (text.Length != LColor.Length)
            {
                throw new Exception("InitText and LColor length do not match!");
            }
            else
            {
                this.LColor = LColor;
                CreateLetters();
            }
        }

        public void ChangeText(string Text, Color[] colors)
        {
            char[] strArr = Text.ToCharArray();
            characters = ParseArray(strArr);
            LColor = colors;
            text = Text.ToUpper();
            CreateLetters();
        }

        public void ChangeText(string Text)
        {
            Color[] c = new Color[Text.Length];
            for (int i = 0; i < c.Length; i++)
            {
                c[i] = Color.White;
            }

            ChangeText(Text, c);
        }

        private void CreateLetters()
        {
            if (characters.Length != LColor.Length)
            {
                throw new Exception("char and color length do not match!");
            }
            else
            {
                LetterAry = new Letter[characters.Length];
                int CurrentStrLength = 0;
                for (int i = 0; i < characters.Length; i++)
                {
                    LetterAry[i] = new Letter(new Vector2(Position.X + CurrentStrLength, Position.Y), this.Size,
                        characters[i], LColor[i]);
                    CurrentStrLength += LetterAry[i].frameSpace.Width * ((int) Size.X / 8)
                                        +
                                        LetterAry[i].frameSpace.X * ((int) Size.X / 8)
                                        +
                                        (spacing + 1) * ((int) Size.X / 8);
                }

                Length = CurrentStrLength;
            }
        }

        private Letter.Character[] ParseArray(char[] strArr)
        {
            List<Letter.Character> Letters = new List<Letter.Character>();
            for (int i = 0; i < strArr.Length; i++)
            {
                switch (strArr[i])
                {
                    case '0':
                        Letters.Add(Letter.Character.Zero);
                        break;
                    case '1':
                        Letters.Add(Letter.Character.One);
                        break;
                    case '2':
                        Letters.Add(Letter.Character.Two);
                        break;
                    case '3':
                        Letters.Add(Letter.Character.Three);
                        break;
                    case '4':
                        Letters.Add(Letter.Character.Four);
                        break;
                    case '5':
                        Letters.Add(Letter.Character.Five);
                        break;
                    case '6':
                        Letters.Add(Letter.Character.Six);
                        break;
                    case '7':
                        Letters.Add(Letter.Character.Seven);
                        break;
                    case '8':
                        Letters.Add(Letter.Character.Eight);
                        break;
                    case '9':
                        Letters.Add(Letter.Character.Nine);
                        break;
                    case 'a':
                    case 'A':
                        Letters.Add(Letter.Character.A);
                        break;
                    case 'b':
                    case 'B':
                        Letters.Add(Letter.Character.B);
                        break;
                    case 'c':
                    case 'C':
                        Letters.Add(Letter.Character.C);
                        break;
                    case 'd':
                    case 'D':
                        Letters.Add(Letter.Character.D);
                        break;
                    case 'e':
                    case 'E':
                        Letters.Add(Letter.Character.E);
                        break;
                    case 'f':
                    case 'F':
                        Letters.Add(Letter.Character.F);
                        break;
                    case 'g':
                    case 'G':
                        Letters.Add(Letter.Character.G);
                        break;
                    case 'h':
                    case 'H':
                        Letters.Add(Letter.Character.H);
                        break;
                    case 'i':
                    case 'I':
                        Letters.Add(Letter.Character.I);
                        break;
                    case 'j':
                    case 'J':
                        Letters.Add(Letter.Character.J);
                        break;
                    case 'k':
                    case 'K':
                        Letters.Add(Letter.Character.K);
                        break;
                    case 'l':
                    case 'L':
                        Letters.Add(Letter.Character.L);
                        break;
                    case 'm':
                    case 'M':
                        Letters.Add(Letter.Character.M);
                        break;
                    case 'n':
                    case 'N':
                        Letters.Add(Letter.Character.N);
                        break;
                    case 'o':
                    case 'O':
                        Letters.Add(Letter.Character.O);
                        break;
                    case 'p':
                    case 'P':
                        Letters.Add(Letter.Character.P);
                        break;
                    case 'q':
                    case 'Q':
                        Letters.Add(Letter.Character.Q);
                        break;
                    case 'r':
                    case 'R':
                        Letters.Add(Letter.Character.R);
                        break;
                    case 's':
                    case 'S':
                        Letters.Add(Letter.Character.S);
                        break;
                    case 't':
                    case 'T':
                        Letters.Add(Letter.Character.T);
                        break;
                    case 'u':
                    case 'U':
                        Letters.Add(Letter.Character.U);
                        break;
                    case 'v':
                    case 'V':
                        Letters.Add(Letter.Character.V);
                        break;
                    case 'w':
                    case 'W':
                        Letters.Add(Letter.Character.W);
                        break;
                    case 'x':
                    case 'X':
                        Letters.Add(Letter.Character.X);
                        break;
                    case 'y':
                    case 'Y':
                        Letters.Add(Letter.Character.Y);
                        break;
                    case 'z':
                    case 'Z':
                        Letters.Add(Letter.Character.Z);
                        break;
                    case '!':
                        Letters.Add(Letter.Character.Exclamation);
                        break;
                    case '?':
                        Letters.Add(Letter.Character.Question);
                        break;
                    case '/':
                        Letters.Add(Letter.Character.Slash);
                        break;
                    case '-':
                        Letters.Add(Letter.Character.Minus);
                        break;
                    case '<':
                        Letters.Add(Letter.Character.SmallerAs);
                        break;
                    case '=':
                        Letters.Add(Letter.Character.Equal);
                        break;
                    case '>':
                        Letters.Add(Letter.Character.BiggerAs);
                        break;
                    case '*':
                        Letters.Add(Letter.Character.Star);
                        break;
                    case '+':
                        Letters.Add(Letter.Character.Plus);
                        break;
                    case '%':
                        Letters.Add(Letter.Character.Percent);
                        break;
                    case '(':
                        Letters.Add(Letter.Character.OpenBracket);
                        break;
                    case ')':
                        Letters.Add(Letter.Character.CloseBracket);
                        break;
                    case ';':
                        Letters.Add(Letter.Character.Semicolon);
                        break;
                    case '.':
                        Letters.Add(Letter.Character.Dot);
                        break;
                    case ' ':
                        Letters.Add(Letter.Character.Space);
                        break;
                    case '✔':
                        Letters.Add(Letter.Character.Checkmark);
                        break;
                    case '❌':
                        Letters.Add(Letter.Character.Crossout);
                        break;
                    case '⬇':
                        Letters.Add(Letter.Character.Down);
                        break;
                    case '⬆':
                        Letters.Add(Letter.Character.Up);
                        break;
                    case '⬜':
                        Letters.Add(Letter.Character.Full);
                        break;
                    case '_':
                        Letters.Add(Letter.Character.Line);
                        break;
                    case ':':
                        Letters.Add(Letter.Character.DoubleDots);
                        break;
                    case ',':
                        Letters.Add(Letter.Character.Komma);
                        break;
                    case '⬅':
                        Letters.Add(Letter.Character.Left);
                        break;
                    case '➡':
                        Letters.Add(Letter.Character.Right);
                        break;
                    case '\"':
                        Letters.Add(Letter.Character.Parentheses);
                        break;
                    case '\\':
                        Letters.Add(Letter.Character.Backslash);
                        break;
                    default:
                        throw new Exception("Unknown Character at char \'" + strArr[i] + "\' ");
                }
            }

            return Letters.ToArray();
        }

        public override void Update(GameTime gameTime)
        {
            //base.Update(gt);
            for (int i = 0; i < LetterAry.Length; i++)
            {
                LetterAry[i].Update(gameTime);
            }

            rec = new Rectangle(Position.ToPoint(),
                new Point(Length + (spacing + 1) * (LetterAry.Length - 1), (int) Size.Y));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //base.Draw(sp);
            for (int i = 0; i < LetterAry.Length; i++)
            {
                LetterAry[i].Draw(spriteBatch);
            }
        }

        public static Letter.Character Parse(char character)
        {
            return character switch
            {
                '0' => Letter.Character.Zero,
                '1' => Letter.Character.One,
                '2' => Letter.Character.Two,
                '3' => Letter.Character.Three,
                '4' => Letter.Character.Four,
                '5' => Letter.Character.Four,
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
                'j' => Letter.Character.J,
                'J' => Letter.Character.J,
                'k' => Letter.Character.K,
                'K' => Letter.Character.K,
                'l' => Letter.Character.L,
                'L' => Letter.Character.L,
                'm' => Letter.Character.M,
                'M' => Letter.Character.M,
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
        }
    }
}