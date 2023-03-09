using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.TextSystem;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level0;

internal class Level : SampleLevel
{
    public Level(Display.Display display, Vector2 window, Random rand) : base(display, window, rand)
    {
        Name = "Level 404";
        
        var failButton = new TextButton(new Vector2(-64, -32), "end", "Restart")
        {
            DrawColor = Color.White,
        };
        failButton.Click += Fail;
        AutoManaged.Add(failButton);
        
        var info = new Text("Unknown level requested [404]", Vector2.Zero);
        info.Move(-info.Rectangle.Size.ToVector2() / 2F + new Vector2(0,-64));
        var cursor = new Cursor();
        Actuator = cursor;
        PositionListener.Add(_mouse, cursor);
        AutoManaged.Add(cursor);
    }
}