using Microsoft.Xna.Framework;
using NoNameButtonGame.Hitboxes;

namespace NoNameButtonGame.GameObjects.Buttons.StartMenu;

public class SettingsButton : EmptyButton
{
    public SettingsButton(Vector2 Position, Vector2 Size) : base(Position, Size)
    {
    }

    public override void Initialize()
    {
        _textureHitboxMapping = Mapping.GetMappingFromCache<SettingsButton>();
    }
}