using Microsoft.Xna.Framework;
using NoNameButtonGame.Hitboxes;

namespace NoNameButtonGame.GameObjects.Buttons.StartMenu;

public class StartButton : EmptyButton
{
    public StartButton(Vector2 position, Vector2 size) : base(position, size)
    {
    }

    public override void Initialize()
    {
        _textureHitboxMapping = Mapping.GetMappingFromCache<StartButton>();
    }
}