using System;
using System.Net.Security;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NoNameButtonGame.Debug;
using NoNameButtonGame.Extensions;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.AddOn;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.Debug;
using NoNameButtonGame.GameObjects.TextSystem;
using NoNameButtonGame.LogicObjects;
using NoNameButtonGame.LogicObjects.Listener;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level6;

public class Level : SampleLevel
{
    private Storage.Storage _storage;
    
    private OverTimeMover _overTimeMoverShop;
    private OverTimeMover _overTimeMoverMain;

    private Shop _shop;
    private Text _counter;

    private readonly Rectangle _originScreen;

    private readonly LockButtonAddon _shopButton;

    public Level(Display.Display display, Vector2 window, Random random, Storage.Storage storage) : base(display,
        window, random)
    {
        Name = "Level 6 - Just like Cookie Clicker but with BEANS!";

        _storage = storage;

        var OneScreen = Display.Display.Size / 2;
        var shopScreen = new Vector2(OneScreen.X, 0);
        
        Camera.Move(OneScreen / 2);
        _originScreen = Camera.Rectangle;
        //Camera.Zoom = 0.5F;
        
        _overTimeMoverShop = new OverTimeMover(Camera, Camera.Position + shopScreen, 666F, OverTimeMover.MoveMode.Sin);
        _overTimeMoverMain = new OverTimeMover(Camera, Camera.Position, 666F, OverTimeMover.MoveMode.Sin);
        AutoManaged.Add(_overTimeMoverShop);
        AutoManaged.Add(_overTimeMoverMain);
        
        var shopButton = new TextButton("Shop");
        shopButton.GetCalculator(Camera.Rectangle).OnX(1F).OnY(1F).BySize(-1F).Move();
        shopButton.Click += ShopButtonClick;
        _shopButton = new LockButtonAddon(new(shopButton));
        
        AutoManaged.Add(_shopButton);
        
        var returnButton = new TextButton("Return");
        returnButton.GetCalculator(Camera.Rectangle).OnX(1F).OnY(1F).BySizeY(-1F).Move();
        returnButton.Click += ReturnButtonClick; 
        AutoManaged.Add(returnButton);

        var clickButton = new TextButton("Bake a Bean!");
        clickButton.Move(OneScreen / 2 - clickButton.Size / 2);
        clickButton.Click += o => _shop.IncreaseBeanCount();
        
        AutoManaged.Add(clickButton);

        _shop = new Shop(shopScreen, OneScreen, _storage.GameData.Level6, random);
        _shop.UnlockedShop += _shopButton.Unlock;
        AutoManaged.Add(_shop);

        _counter = new Text(string.Empty);
        _counter.GetCalculator(Camera.Rectangle).OnCenter().BySizeY(-0.5F).OnY(0.3F).Move();
        AutoManaged.Add(_counter);

        var cursor = new Cursor();
        Actuator = cursor;
        PositionListener.Add(_mouse, cursor);
        AutoManaged.Add(cursor);
    }

    private void ReturnButtonClick(object obj)
    {
        if (_overTimeMoverMain.IsMoving || _overTimeMoverShop.IsMoving)
            return;
        
        _overTimeMoverMain.Start();
    }

    private void ShopButtonClick(object obj)
    {
        if (_overTimeMoverMain.IsMoving || _overTimeMoverShop.IsMoving)
            return;
        
        _overTimeMoverShop.Start();
    }

    public override void Update(GameTime gameTime)
    {
        _counter.ChangeText(_shop.BeanCount.ToString("n0"));
        base.Update(gameTime);
        _counter.GetCalculator(_originScreen).OnCenter().Centered().OnY(3,10).Move();
    }
}