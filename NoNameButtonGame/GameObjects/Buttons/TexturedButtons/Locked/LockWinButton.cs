using Microsoft.Xna.Framework;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.Hitboxes;

namespace NoNameButtonGame.GameObjects.Buttons;

public class LockWinButton : LockButton
{
    public LockWinButton(Vector2 position) : base(position)
    {
    }
    
    public LockWinButton(Vector2 position, bool startState) : base(position, startState)
    {
    }
    
    public LockWinButton(Vector2 position, Vector2 size, bool startState) : base(position, size, startState)
    {
    }

    public override void Initialize()
    {
        _textureHitboxMapping = Mapping.GetMappingFromCache<WinButton>();
    }
}