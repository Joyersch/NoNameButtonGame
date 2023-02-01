using Microsoft.Xna.Framework;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.Hitboxes;

namespace NoNameButtonGame.GameObjects.Buttons;

public class StateWinButton : StateButton
{
    public StateWinButton(Vector2 position, Vector2 size, int states) : base(position, size, states)
    {
    }

    public override void Initialize()
    {
        _textureHitboxMapping = Mapping.GetMappingFromCache<WinButton>();
    }
}