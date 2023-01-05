using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;

namespace Raigy.Camera
{
    class CameraClass
    {
        public Matrix CamMatrix { get; set; }
        public float Zoom = 2f;
        Vector2 Screen;

        public CameraClass(Vector2 screen) {
            Screen = screen;
        }
        public void Update(Vector2 TargetPos,Vector2 TargetSize) {

            var position = Matrix.CreateTranslation(-TargetPos.X - (TargetSize.X / 2), -TargetPos.Y - (TargetSize.Y / 2), 0);

            var offset = Matrix.CreateTranslation(Screen.X/ 2, Screen.Y / 2, 0);

            CamMatrix = position * Matrix.CreateScale(Zoom) * offset;
        }

        
    }
}
