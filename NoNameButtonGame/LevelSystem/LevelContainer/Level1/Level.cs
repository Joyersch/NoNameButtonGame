using System;
using Microsoft.Xna.Framework;
using NoNameButtonGame.Extensions;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.TextSystem;
using NoNameButtonGame.LogicObjects;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level1;

internal class Level : SampleLevel
{
    public Level(Display.Display display, Vector2 window, Random random) : base(display, window, random)
    {
        Name = "Level 1 - Click the Button!";
        
        var startButton = new TextButton("Start");
        startButton.GetCalculator(Camera.Rectangle).OnCenter().Centered().Move();
        startButton.Click += Finish;
        AutoManaged.Add(startButton);
        
        var infoText = new Text("How hard can it be?");
        infoText.GetCalculator(Camera.Rectangle)
            .OnCenter()
            .OnY(3,10)
            .Centered()
            .Move();
        AutoManaged.Add(infoText);
        
        var mouseCursor = new Cursor();
        Actuator = mouseCursor;
        AutoManaged.Add(mouseCursor);
        PositionListener.Add(base._mouse, mouseCursor);
    }
}