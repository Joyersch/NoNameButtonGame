using Microsoft.Xna.Framework;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.Cache;

namespace NoNameButtonGame.GameObjects.Buttons;

public class StateWinButton : StateButton
{
    public StateWinButton(Vector2 position, Vector2 canvas, int states) : base(position, canvas, states)
    {
    }

    public override void Initialize()
    {
        _textureHitboxMapping = Globals.Textures.GetMappingFromCache<WinButton>();
    }
}