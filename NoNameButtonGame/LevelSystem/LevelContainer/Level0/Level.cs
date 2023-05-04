using System;
using Microsoft.Xna.Framework;
using NoNameButtonGame.Extensions;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.TextSystem;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level0;

internal class Level : SampleLevel
{
    public Level(Display.Display display, Vector2 window, Random rand) : base(display, window, rand)
    {
        Name = "Level 404";

        var failButton = new TextButton("Restart");
        failButton.GetCalculator(Camera.Rectangle)
            .OnCenter()
            .Centered()
            .Move();
        failButton.Click += Fail;
        AutoManaged.Add(failButton);
        
        var info = new Text("Unknown level requested [404]");
        info.GetCalculator(Camera.Rectangle)
            .OnCenter()
            .OnY(3,10)
            .Centered()
            .Move();
        AutoManaged.Add(info);
        
        var cursor = new Cursor();
        Actuator = cursor;
        PositionListener.Add(Mouse, cursor);
        AutoManaged.Add(cursor);
    }
}