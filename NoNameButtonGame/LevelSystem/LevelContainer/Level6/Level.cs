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
    
    private OverTimeMover _overTimeMoverShop;
    private OverTimeMover _overTimeMoverMain;

    private Shop _shop;
    private Text _counter;
    
    private BeanState _state;

    private LockButtonAddon _lockButtonAddon;

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
        var shopScreen = new Vector2(OneScreen.X, 0);
        
        Camera.Move(OneScreen / 2);
        //Camera.Zoom = 0.5F;
        
        _overTimeMoverShop = new OverTimeMover(Camera, Camera.Position + shopScreen, 666F, OverTimeMover.MoveMode.Sin);
        _overTimeMoverMain = new OverTimeMover(Camera, Camera.Position, 666F, OverTimeMover.MoveMode.Sin);
        AutoManaged.Add(_overTimeMoverShop);
        AutoManaged.Add(_overTimeMoverMain);
        
        var shopButton = new TextButton("Shop");
        shopButton.GetCalculator(Camera.Rectangle).OnX(1F).OnY(1F).BySize(-1F).Move();
        shopButton.Click += ShopButtonClick;
        
        AutoManaged.Add(shopButton);
        
        var returnButton = new TextButton("Return");
        returnButton.GetCalculator(Camera.Rectangle).OnX(1F).OnY(1F).BySizeY(-1F).Move();
        returnButton.Click += ReturnButtonClick; 
        AutoManaged.Add(returnButton);

        var clickButton = new TextButton("Bake a Bean!");
        clickButton.Move(OneScreen / 2 - clickButton.Size / 2);
        clickButton.Click += o => _shop.IncreaseBeanCount();
        
        var hold = new HoldButtonAddon(new(clickButton), 3000F);
        _lockButtonAddon = new LockButtonAddon(new(hold));
        AutoManaged.Add(_lockButtonAddon);

        _shop = new Shop(shopScreen, OneScreen, _storage.GameData.Level6);
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
        
        _lockButtonAddon.Unlock();
        
        _overTimeMoverShop.Start();
    }

    public override void Update(GameTime gameTime)
    {
        _counter.ChangeText(_shop.BeanCount.ToString());
        
        base.Update(gameTime);
        _counter.GetCalculator(Camera.Rectangle).OnCenter().BySizeY(-0.5F).OnY(0.3F).Move();
    }
}