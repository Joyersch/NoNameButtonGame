using Microsoft.Xna.Framework;
using NoNameButtonGame.GameObjects.Buttons.Level;
using NoNameButtonGame.Hitboxes;

namespace NoNameButtonGame.GameObjects.Buttons.Locked;

public class LockWinButton : LockButton
{
    public LockWinButton(Vector2 position, Vector2 size, bool startState) : base(position, size, startState)
    {
    }

    public override void Initialize()
    {
        _textureHitboxMapping = Mapping.GetMappingFromCache<WinButton>();
    }
}