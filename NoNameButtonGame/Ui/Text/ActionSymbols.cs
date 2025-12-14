using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NoNameButtonGame.Ui.Text;

public sealed class ActionSymbols : ILetter
{
    public static Vector2 ImageSize => new Vector2(8, 8);

    public new static Texture2D Texture;

    // How many letter till line wrap on texture
    public static int TextureWidth => 8;

    public Rectangle GetImageLocation(int letter)
    {
        int x = letter % TextureWidth;
        int y = letter / TextureWidth;
        Vector2 location = new Vector2(ImageSize.X * x, ImageSize.Y * y);
        return new Rectangle(location.ToPoint(), ImageSize.ToPoint());
    }

    public Texture2D GetTexture()
        => Texture;

    public Vector2 GetFullSize()
        => ImageSize;

    public int Parse(string identifier)
    {
        var letter = identifier switch
        {
            "[checkmark]" => Letters.Checkmark,
            "[crossout]" => Letters.Crossout,
            "[down]" => Letters.Down,
            "[up]" => Letters.Up,
            "[left]" => Letters.Left,
            "[right]" => Letters.Right,
            _ => Letters.None
        };
        return (int)letter;
    }

    public Rectangle GetCharacterSpacing(int letters)
    {
        return (Letters)letters switch
        {
            Letters.Checkmark => new Rectangle(0, 1, 8, 7),
            Letters.Crossout => new Rectangle(0, 0, 7, 8),
            Letters.Down => new Rectangle(0, 2, 8, 6),
            Letters.Up => new Rectangle(0, 2, 8, 6),
            Letters.Left => new Rectangle(2, 0, 4, 8),
            Letters.Right => new Rectangle(2, 0, 4, 8),
            _ => new Rectangle(0, 0, 8, 8)
        };
    }

    public enum Letters
    {
        None = -1,
        Checkmark,
        Crossout,
        Left,
        Down,
        Up,
        Right,
    }
}