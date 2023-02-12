using Microsoft.Xna.Framework;
using NoNameButtonGame.Cache;

namespace NoNameButtonGame.GameObjects.Buttons;

public class FailButton : EmptyButton
{
    public FailButton(Vector2 position, Vector2 size) : base(position, size)
    {
    }
    
    public override void Initialize()
    {
        _textureHitboxMapping = Globals.Textures.GetMappingFromCache<FailButton>();
    }
}