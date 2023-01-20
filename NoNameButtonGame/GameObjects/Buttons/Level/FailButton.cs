using Microsoft.Xna.Framework;
using NoNameButtonGame.Hitboxes;

namespace NoNameButtonGame.GameObjects.Buttons.Level;

public class FailButton : EmptyButton
{
    public FailButton(Vector2 Position, Vector2 Size) : base(Position, Size)
    {
    }
    
    public override void Initialize()
    {
        _textureHitboxMapping = Mapping.GetMappingFromCache<FailButton>();
    }
}