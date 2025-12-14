using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NoNameButtonGame.Ui.Text;

public interface ILetter
{
    public int Parse(string identifier);

    public Rectangle GetCharacterSpacing(int character);

    public Rectangle GetImageLocation(int letter);

    public Texture2D GetTexture();

    public Vector2 GetFullSize();
}