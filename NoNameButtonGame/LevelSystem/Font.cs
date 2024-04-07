using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Ui.Objects.TextSystem;

namespace NoNameButtonGame.LevelSystem;

public class Font : ILetter
{
    public static Vector2 ImageSize => new Vector2(8, 8);

    public static Texture2D Texture;

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
        var letter = identifier.ToLower() switch
        {
            "[arrow]" => Letters.Arrow,
            _ => Letters.None
        };
        return (int)letter;
    }

    public Rectangle GetCharacterSpacing(int character)
    {
        return (Letters)character switch
        {
            Letters.Arrow => new Rectangle(2, 1, 5, 5),
            _ => new Rectangle(0, 0, 8, 8)
        };
    }

    public enum Letters
    {
        None = -1,
        Arrow
    }
}