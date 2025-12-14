using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NoNameButtonGame.Ui.Text;

public class Letter : IMoveable, IManageable, IRotateable
{
    private readonly float _scale;
    public Vector2 Position { get; private set; }
    public Vector2 Size { get; private set; }

    public Microsoft.Xna.Framework.Color DrawColor = Microsoft.Xna.Framework.Color.White;

    private Rectangle _rectangle;
    public Rectangle Rectangle => _rectangle;

    private static List<ILetter> LetterOptions;

    public string Identifier;
    private Texture2D _resolvedTexture;
    private Rectangle _resolvedLocation;

    public float Rotation { get; set; }
    public Vector2 Origin;
    public int Layer;

    public Vector2 FullSize { get; private set; }

    public Letter(float scale, string identifier)
    {
        _scale = scale;
        ChangeLetter(identifier);
        Size = _resolvedLocation.Size.ToVector2() * scale;
        _rectangle = new Rectangle(Position.ToPoint(), Size.ToPoint());
    }

    private void ChangeLetter(string identifier)
    {
        _resolvedTexture = null;
        _resolvedLocation = Rectangle.Empty;
        Identifier = identifier;

        foreach (ILetter letter in LetterOptions)
        {
            int value = letter.Parse(identifier);
            if (value == -1)
                continue;

            SetByLetter(letter, value);
        }

        // Failsave
        if (_resolvedTexture is null)
        {
            var defaultLetters = LetterOptions.First(l => l.GetType() == typeof(DefaultLetters));
            SetByLetter(defaultLetters, (int)DefaultLetters.Letters.Block);
        }
    }

    private void SetByLetter(ILetter letter, int value)
    {
        _resolvedTexture = letter.GetTexture();
        var location = letter.GetImageLocation(value);
        var spacing = letter.GetCharacterSpacing(value);
        FullSize = letter.GetFullSize() * _scale;
        _resolvedLocation = new Rectangle(location.X + spacing.X,
            location.Y + spacing.Y,
            spacing.Width,
            spacing.Height);
    }

    public void Update(GameTime gameTime)
    {
        // Nothing to do
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(
            _resolvedTexture,
            Position,
            _resolvedLocation,
            DrawColor,
            Rotation,
            Origin,
            _scale,
            SpriteEffects.None,
            Layer);
    }

    public static void Initialize()
    {
        LetterOptions = new();
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblies)
        {
            var letterImplementations = assembly.GetTypes()
                .Where(type =>
                    typeof(ILetter).IsAssignableFrom(type) && type is { IsInterface: false, IsAbstract: false });

            foreach (var implementation in letterImplementations)
            {
                var instance = Activator.CreateInstance(implementation);
                if (instance is ILetter letter)
                    LetterOptions.Add(letter);
            }
        }
    }

    public static List<Letter> Parse(string value, float scale)
    {
        List<Letter> result = new List<Letter>();
        string pattern = @"\[([^\]]*)\]|.";

        foreach (Match match in Regex.Matches(value, pattern))
        {
            result.Add(new Letter(scale, match.Groups[1].Success ? $"[{match.Groups[1].Value}]" : match.Value));
        }

        return result;
    }

    public Vector2 GetPosition()
        => Position;

    public Vector2 GetSize()
        => Size;

    public void Move(Vector2 newPosition)
    {
        Position = newPosition;
        _rectangle = new Rectangle(Position.ToPoint(), Size.ToPoint());
    }

    public override string ToString()
        => Identifier;
}