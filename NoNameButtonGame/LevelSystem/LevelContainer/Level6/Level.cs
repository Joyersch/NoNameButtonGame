using System;
using System.Net.Security;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NoNameButtonGame.Extensions;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.AddOn;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.Debug;
using NoNameButtonGame.GameObjects.TextSystem;
using NoNameButtonGame.LogicObjects;
using NoNameButtonGame.LogicObjects.Listener;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level6;

public class Level : SampleLevel
{
    private Storage.Storage _storage;

    private long _bakedBeansCounter;
    private OverTimeMover _overTimeMover;

    private Shop _shop;

    private BeanState _state;

    private enum BeanState : ushort
    {
        Started = 0,
        Reached100 = 1,
        Reached1k = 2,
        Reached5k = 3,
        Reached10k = 4,
        Reached25k = 5,
        Reached100k = 6,
        Reached250k = 7,
        Reached1m = 8
    }

    public Level(Display.Display display, Vector2 window, Random random, Storage.Storage storage) : base(display,
        window, random)
    {
        Name = "Level 6 - Just like Cookie Clicker but with BEANS!";

        _state = BeanState.Started;

        _storage = storage;

        var OneScreen = Display.Display.Size / 2;
        Camera.Move(OneScreen / 2);
        Camera.Zoom = 0.5F;
        
        var clickButton = new TextButton("Bake a Bean!");
        clickButton.Move(OneScreen / 2 - clickButton.Size / 2);
        clickButton.Click += o => _shop.IncreaseBeanCount();
        AutoManaged.Add(clickButton);
        
        
        var shopScreen = new Vector2(OneScreen.X, 0);

        _shop = new Shop(shopScreen, _storage.GameData.Level6);
        AutoManaged.Add(_shop);

        var cursor = new Cursor();
        Actuator = cursor;
        PositionListener.Add(_mouse, cursor);
        AutoManaged.Add(cursor);
    }

    public override void Update(GameTime gameTime)
    {
        Console.WriteLine(_mouse.Position);
        base.Update(gameTime);
    }
}