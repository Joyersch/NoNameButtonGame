using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.Colors;
using NoNameButtonGame.GameObjects.AddOn;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.Debug;
using NoNameButtonGame.GameObjects.TextSystem;
using NoNameButtonGame.LogicObjects.Listener;

namespace NoNameButtonGame.LevelSystem.LevelContainer;

internal class Level2 : SampleLevel
{
    private readonly LockButtonAddon _lockButtonAddon;
    public Level2(Display.Display display, Vector2 window, Random random) : base(display, window, random)
    {
        Name = "Level 2 - Tutorial 1 - Button Addon: Lock";
        
        var magicButton = new TextButton("Unlock");
        magicButton.Move(-TextButton.DefaultSize / 2 + new Vector2(0, TextButton.DefaultSize.Y));
        magicButton.Click += MagicButtonOnClick;
        AutoManaged.Add(magicButton);

        var lockButton = new TextButton("Finish Level");
        lockButton.Move(-TextButton.DefaultSize / 2 + new Vector2(0, -TextButton.DefaultSize.Y));
        
        _lockButtonAddon = new LockButtonAddon(lockButton);
        _lockButtonAddon.Callback += Finish;
        AutoManaged.Add(_lockButtonAddon);

        var info1 = new Text("This button here is locked!");
        info1.ChangePosition(new Vector2(-info1.Rectangle.Width / 2F, -128));
        AutoManaged.Add(info1);
        
        var info2 = new Text("The button below will unlock the button above!");
        info2.ChangePosition(new Vector2(-info2.Rectangle.Width / 2F, -Text.DefaultLetterSize.Y / 2));
        AutoManaged.Add(info2);
        
        var cursor = new Cursor();
        Interactable = cursor;
        PositionListener.Add(_mouse, cursor);
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