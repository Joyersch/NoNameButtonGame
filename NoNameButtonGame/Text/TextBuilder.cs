using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.GameObjects;

namespace NoNameButtonGame.Text
{
    class TextBuilder : GameObject
    {
        private Letter[] _letters;
        private int spacing;
        private int _length;
        private string represent;

        public string Text => represent;

        public Letter this[int i] => _letters[i];
        public TextBuilder(string text, Vector2 position, Vector2 letterSize, Color[] color, int spacing) : this(text,
            position, letterSize, spacing)
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
                letter.Position +=  position - Position;
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
            
            _length = currentStringLength;
        }

        private Letter.Character[] ParseArray(char[] text)
            => text.Select(Letter.Parse).ToArray();

        public override void Update(GameTime gameTime)
        {
            represent = BuildString(_letters);
            for (int i = 0; i < _letters.Length; i++)
            {
                _letters[i].Update(gameTime);
            }

            rec = new Rectangle(Position.ToPoint(),
                new Point(_length + (spacing + 1) * (_letters.Length - 1), (int) Size.Y));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _letters.Length; i++)
            {
                _letters[i].Draw(spriteBatch);
            }
        }

        public string BuildString(Letter[] letters)
        {
            string build = string.Empty;
            foreach (var letter in letters)
            {
                build += Letter.ReverseParse(letter.RepresentingCharacter);
            }
            return build;
        }
    }
}