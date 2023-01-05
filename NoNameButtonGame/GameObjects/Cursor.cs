using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Raigy.Obj;
using NoNameButtonGame.BeforeMaths;
using NoNameButtonGame.Interfaces;

namespace NoNameButtonGame.GameObjects
{
    class Cursor : GameObject, IHitbox
    {
        Rectangle[] textureHitbox;
        Rectangle[] ingameHitbox;
        Vector2 Scale;

        public Rectangle[] Hitbox => ingameHitbox;

        public Cursor(Vector2 Position, Vector2 Size, THBox thBox) {
            base.Size = Size;
            base.Position = Position;
            ImageLocation = new Rectangle(0,0,0,0);
            FrameSize = thBox.Imagesize;
            Texture = thBox.Texture;
            DrawColor = Color.White;
            textureHitbox = thBox.Hitbox;
            ingameHitbox = new Rectangle[textureHitbox.Length];
            Scale = new Vector2(Size.X / FrameSize.X, Size.Y / FrameSize.Y);
            for (int i = 0; i < thBox.Hitbox.Length; i++) {
                ingameHitbox[i] = new Rectangle((int)(base.Position.X + (thBox.Hitbox[i].X * Scale.X)), (int)(base.Position.Y + (thBox.Hitbox[i].Y * Scale.Y)), (int)(thBox.Hitbox[i].Width * Scale.X), (int)(thBox.Hitbox[i].Height * Scale.Y));
            }
        }

        public override void Draw(SpriteBatch sp) {
            base.Draw(sp);
        }

        public override void Update(GameTime gt) {
            base.Update(gt);
            UpdateHitbox();
        }
        private void UpdateHitbox() {
            Scale = new Vector2(Size.X / FrameSize.X, Size.Y / FrameSize.Y);
            for (int i = 0; i < textureHitbox.Length; i++) {
                ingameHitbox[i] = new Rectangle((int)(Position.X + (textureHitbox[i].X * Scale.X)), (int)(Position.Y + (textureHitbox[i].Y * Scale.Y)), (int)(textureHitbox[i].Width * Scale.X), (int)(textureHitbox[i].Height * Scale.Y));
            }
        }
    }
}
