using Microsoft.Xna.Framework;

namespace NoNameButtonGame.GameObjects.Buttons.TexturedButtons.Empty;

public class SquareButton : EmptyButton
{
    public SquareButton(Vector2 position) : this(position, DefaultSize)
    {
    }
    
    public SquareButton(Vector2 position, float scale) : this(position, DefaultSize * scale)
    {
    }

    public SquareButton(Vector2 position, Vector2 size) : base(position, size)
    {
    }

    public override void Initialize()
    {
        textureHitboxMapping = Globals.Textures.GetMappingFromCache<SquareButton>();
        clickEffect = Globals.SoundEffects.GetEffect("ButtonSound");
    }

    public new static Vector2 DefaultSize => new Vector2(64, 32);
}