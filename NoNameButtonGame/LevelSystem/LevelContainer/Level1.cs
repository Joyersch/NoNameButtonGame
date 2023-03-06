using System;
using Microsoft.Xna.Framework;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.TextSystem;

namespace NoNameButtonGame.LevelSystem.LevelContainer;

internal class Level1 : SampleLevel
{
    public Level1(Display.Display display, Vector2 window, Random random) : base(display, window, random)
    {
        Name = "Level 1 - Click the Button!";
        
        var startButton = new TextButton("Start");
        startButton.Move(-TextButton.DefaultSize / 2);
        startButton.Click += Finish;
        AutoManaged.Add(startButton);

        var infoText = new Text("How hard can it be?");
        infoText.Move(Vector2.Zero -
                       new Vector2(infoText.Rectangle.Width,
                           infoText.Rectangle.Height + TextButton.DefaultSize.Y * 2) / 2);
        AutoManaged.Add(infoText);
        
        var mouseCursor = new Cursor();
        Interactable = mouseCursor;
        AutoManaged.Add(mouseCursor);
        PositionListener.Add(base._mouse, mouseCursor);
    }
}