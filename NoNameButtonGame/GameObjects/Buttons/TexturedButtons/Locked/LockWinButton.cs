using Microsoft.Xna.Framework;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.Cache;

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
        _textureHitboxMapping = Globals.Textures.GetMappingFromCache<WinButton>();
    }
}