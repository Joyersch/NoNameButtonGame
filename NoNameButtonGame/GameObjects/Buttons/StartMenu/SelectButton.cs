using Microsoft.Xna.Framework;
using NoNameButtonGame.Hitboxes;

namespace NoNameButtonGame.GameObjects.Buttons.StartMenu;

public class SelectButton : EmptyButton
{
    public SelectButton(Vector2 Position, Vector2 Size) : base(Position, Size)
    {
    }

    public override void Initialize()
    {
        _textureHitboxMapping = Mapping.GetMappingFromCache<SelectButton>();
    }
}