using System;
using Microsoft.Xna.Framework;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.Extensions;
using NoNameButtonGame.GameObjects.AddOn;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.TextSystem;
using NoNameButtonGame.Interfaces;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level2;

internal class Level : SampleLevel
{
    private readonly LockButtonAddon _lockButtonAddon;
    public Level(Display.Display display, Vector2 window, Random random) : base(display, window, random)
    {
        Name = "Level 2 - Tutorial 1 - Button Addon: Lock";
        
        var magicButton = new TextButton("Unlock");
        magicButton.GetCalculator(Camera.Rectangle).OnCenter().OnY(13,16).Centered().Move();
        magicButton.Click += MagicButtonOnClick;
        AutoManaged.Add(magicButton);

        var lockButton = new TextButton("Finish Level");
        lockButton.GetCalculator(Camera.Rectangle).OnCenter().OnY(3,16).Centered().Move();
        
        _lockButtonAddon = new LockButtonAddon(new(lockButton));
        _lockButtonAddon.Callback += (_, state) =>
        {
            if (state != IButtonAddon.CallState.Click)
                return;
            Finish();
        };
        AutoManaged.Add(_lockButtonAddon);

        var info1 = new Text("This button here is locked!");
        info1.GetCalculator(Camera.Rectangle).OnCenter().OnY(7,20).Centered().Move();
        AutoManaged.Add(info1);
        
        var info2 = new Text("The button below will unlock the button above!");
        info2.GetCalculator(Camera.Rectangle).OnCenter().OnY(13,20).Centered().Move();
        AutoManaged.Add(info2);
        
        var cursor = new Cursor();
        Actuator = cursor;
        PositionListener.Add(Mouse, cursor);
        AutoManaged.Add(cursor);
    }

    private void MagicButtonOnClick(object obj)
    {
        if (_lockButtonAddon.IsLocked)
            _lockButtonAddon.Unlock();
        else
            _lockButtonAddon.Lock();
    }
}