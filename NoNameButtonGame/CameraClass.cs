using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;

namespace NoNameButtonGame.Camera
{
    internal class CameraClass
    {
        public Matrix CameraMatrix { get; private set; }
        public readonly float Zoom = 2f;
        private Vector2 _screen;

        public CameraClass(Vector2 screen)
        {
            _screen = screen;
        }

        public void Update(Vector2 TargetPos, Vector2 TargetSize)
        {
            var position =
                Matrix.CreateTranslation(-TargetPos.X - (TargetSize.X / 2), -TargetPos.Y - (TargetSize.Y / 2), 0);

            var offset = Matrix.CreateTranslation(_screen.X / 2, _screen.Y / 2, 0);

            CameraMatrix = position * Matrix.CreateScale(Zoom) * offset;
        }
    }
}