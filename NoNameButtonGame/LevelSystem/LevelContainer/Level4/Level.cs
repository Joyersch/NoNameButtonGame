using System;
using Microsoft.Xna.Framework;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.AddOn;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.TextSystem;
using NoNameButtonGame.Interfaces;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level4;

internal class Level : SampleLevel
{
    public Level(Display.Display display, Vector2 window, Random random) : base(display, window, random)
    {
        Name = "Level 4 - Tutorial 3 - Button Addon: Hold";

        var stateButton = new TextButton("Finish Level");
        stateButton.Move(-EmptyButton.DefaultSize / 2);

        var counterButtonAddon = new HoldButtonAddon(new(stateButton), 3000F);
        counterButtonAddon.Callback += (_, state) =>
        {
            if (state != IButtonAddon.CallState.Click)
                return;
            Finish();
        };
        AutoManaged.Add(counterButtonAddon);
        
        var infoAboutButton = new Text("This button has a timer");
        infoAboutButton.Move(new Vector2(-infoAboutButton.Rectangle.Width / 2F, -64));
        AutoManaged.Add(infoAboutButton);
        
        var infoAboutButton2 = new Text("Press the button to lower the timer and when it hits 0 you win!");
        infoAboutButton2.Move(new Vector2(-infoAboutButton2.Rectangle.Width / 2F,
            64 - infoAboutButton2.Rectangle.Height));
        AutoManaged.Add(infoAboutButton2);
        
        var mouseCursor = new Cursor();
        Actuator = mouseCursor;
        PositionListener.Add(Mouse, mouseCursor);
        AutoManaged.Add(mouseCursor);
    }
}