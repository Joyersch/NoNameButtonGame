using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NoNameButtonGame.GameObjects;

namespace NoNameButtonGame.Colors
{
    class BaseAniColor
    {
        public Color[] Color;
        int Index;
        float GT;
        public int Increment = 1;
        public float Speed;
        public int Offset;

        public BaseAniColor()
        {
            Init();
        }

        public virtual void Init()
        {
            Color = new Color[0];
        }

        public virtual void Update(GameTime gameTime)
        {
            if (Color.Length != 0 && Speed > 0)
            {
                GT += (float) gameTime.ElapsedGameTime.TotalMilliseconds;
                while (GT > Speed)
                {
                    GT -= Speed;
                    Index += Increment;
                    if (Index >= Color.Length)
                        Index -= Color.Length;
                }
            }
        }

        public Color[] GetColor(int Length)
        {
            Color[] rc = new Color[Length];
            for (int i = 0; i < Length; i++)
            {
                rc[i] = Color[(i * Increment + Index + Offset) % Color.Length];
            }

            return rc;
        }
    }
}