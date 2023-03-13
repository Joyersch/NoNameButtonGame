using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NoNameButtonGame.Extensions;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.AddOn;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.Debug;
using NoNameButtonGame.GameObjects.TextSystem;
using NoNameButtonGame.LogicObjects.Listener;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level3;

internal class Level : SampleLevel
{
    public Level(Display.Display display, Vector2 window, Random random) : base(display, window, random)
    {
        Name = "Level 3 - Tutorial 2 - Button Addon: Counter";

        var stateButton = new TextButton("Finish Level");
        stateButton.GetCalculator(Camera.Rectangle).OnCenter().Centered().Move();
        
        var infoAboutButton = new Text("This button has a counter");
        infoAboutButton.GetCalculator(Camera.Rectangle).OnCenter().OnY(3, 10).Centered().Move();
        AutoManaged.Add(infoAboutButton);
        
        var infoAboutButton2 = new Text("Press the button to lower the counter and when it hits 0 you win!");
        infoAboutButton2.GetCalculator(Camera.Rectangle).OnCenter().OnY(7, 10).Centered().Move();
        AutoManaged.Add(infoAboutButton2);
        
        var counterButtonAddon = new CounterButtonAddon(stateButton, 5);
        counterButtonAddon.StateReachedZero += Finish;
        AutoManaged.Add(counterButtonAddon);
        
        var mouseCursor = new Cursor();
        PositionListener.Add(_mouse, mouseCursor);
        Actuator = mouseCursor;
        AutoManaged.Add(mouseCursor);
    }
}