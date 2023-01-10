using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Joyersch.Obj;

namespace NoNameButtonGame.Colors
{
    class BaseAniColor : MonoObject
    {
        public Color[] Color;
        int Index;
        float GT;
        public int Increment;
        public float Speed;
        public int Offset;

        public BaseAniColor() {
            Init();
        }
        public virtual void Init() {
            Color = new Color[0];
            
        }

        public override void Draw(SpriteBatch sp) {

        }

        public override void Update(GameTime gt) {
             if (Color.Length != 0 && Speed > 0) {
                GT += (float)gt.ElapsedGameTime.TotalMilliseconds;
                while (GT > Speed) {
                    GT -= Speed;
                    Index += Increment;
                    if (Index >= Color.Length)
                        Index -= Color.Length;
                }
            }
          
        }
        public Color[] GetColor(int Length) {
            Color[] rc = new Color[Length];
            for (int i = 0; i < Length; i++) {
                rc[i] = Color[(i * Increment + Index + Offset)  % Color.Length];
            }
            return rc;
        }
    }
}
