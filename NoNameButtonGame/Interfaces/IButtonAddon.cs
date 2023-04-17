using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NoNameButtonGame.Interfaces;

public interface IButtonAddon
{
   public Vector2 GetPosition();
   public Vector2 GetSize();
   
   public Rectangle GetRectangle();
   public void UpdateInteraction(GameTime gameTime, IHitbox toCheck);
   public void Update(GameTime gameTime);
   public void Draw(SpriteBatch spriteBatch);
   public void Move(Vector2 newPosition);

   public void SetIndicatorOffset(int x);
   public event Action<object, CallState> Callback;

   public void SetDrawColor(Color color);
   public enum CallState
   {
      Leave,
      Enter,
      Click,
      Other
   }
}