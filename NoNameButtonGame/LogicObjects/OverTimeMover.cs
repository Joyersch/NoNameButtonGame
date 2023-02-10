using System;
using Microsoft.Xna.Framework;
using NoNameButtonGame.Interfaces;

namespace NoNameButtonGame.LogicObjects;

public class OverTimeMover
{
    private float savedGameTime;
    private IMoveable _moveable;
    private int currentTicks;
    private bool inMove = false;
    
    public void Update(GameTime gameTime)
    {
        if (!inMove)
            return;
        
        savedGameTime += (float) gameTime.ElapsedGameTime.TotalMilliseconds;
        var position = _moveable.GetPosition();
        while (savedGameTime > 10)
        {
            /*
            savedGameTime -= 10;
            Vector2 sinWaveRoute =
                new Vector2(0, 12.2F * (float) Math.Sin((float) currentTicks / 50 * Math.PI));
            if (moveUp)
                cameraPosition -= sinWaveRoute;
            else
                cameraPosition += sinWaveRoute;
            currentTicks--;
            if (currentTicks == 0)
            {
                float alignmentOffset = cameraPosition.Y % (defaultHeight / Camera.Zoom);
                if (!moveUp)
                    cameraPosition.Y += (defaultHeight / Camera.Zoom) - alignmentOffset;
                else
                    cameraPosition.Y -= alignmentOffset;
                isInMove = false;
            }
            */
        }
    }
}