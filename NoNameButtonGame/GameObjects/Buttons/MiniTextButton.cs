using Microsoft.Xna.Framework;
using NoNameButtonGame.Hitboxes;

namespace NoNameButtonGame.GameObjects.Buttons;

public class MiniTextButton : TextButton
{
    public MiniTextButton(Vector2 Pos, Vector2 Size, string Name, string Text, Vector2 TextSize) : base(Pos, Size, Name, Text, TextSize)
    {
    }

    public override void Initialize()
    {
        _textureHitboxMapping = Mapping.GetMappingFromCache<MiniTextButton>();
    }
}