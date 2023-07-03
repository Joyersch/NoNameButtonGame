using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils;
using MonoUtils.Logic.Hitboxes;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level4.Overworld.Quest;

public class QuestItem : GameObject, IInteractable
{
    public new static Vector2 DefaultSize => DefaultMapping.ImageSize;
    public new static Texture2D DefaultTexture;

    public new static TextureHitboxMapping DefaultMapping => new TextureHitboxMapping()
    {
        ImageSize = new Vector2(16, 16),
        Hitboxes = new[]
        {
            new Rectangle(1, 2, 21, 13)
        }
    };

    public bool IsCollected { get; private set; }

    public QuestItem(Vector2 position, float scale) : base(position, DefaultSize * scale,
        DefaultTexture,
        DefaultMapping)
    {
    }

    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        if (IsCollected)
            return;
        
        foreach (var rect in toCheck.Hitbox)
        {
            if (rect.Intersects(Rectangle))
            {
                IsCollected = true;
                // TpDo: Play sound here
                return;
            }
        }
    }
}