using Microsoft.Xna.Framework;
using NoNameButtonGame.Cache;

namespace NoNameButtonGame.GameObjects.Buttons;

public class SelectButton : EmptyButton
{
    public SelectButton(Vector2 position, Vector2 canvas) : base(position, canvas)
    {
    }

    public override void Initialize()
    {
        _textureHitboxMapping = Globals.Textures.GetMappingFromCache<SelectButton>();
    }
}