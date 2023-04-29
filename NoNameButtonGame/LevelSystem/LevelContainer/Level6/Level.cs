using System;
using System.Collections.Generic;
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
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level6;

public class Level : SampleLevel
{
    private Storage.Storage _storage;

    private OverTimeMover _overTimeMoverShop;
    private OverTimeMover _overTimeMoverMain;
    private OverTimeMover _overTimeMoverDistraction;

    private Shop _shop;
    private Text _counter;

    private readonly Rectangle _originScreen;

    private readonly LockButtonAddon _shopButton;
    private readonly LockButtonAddon _snakeButton;

    private readonly TextButton _finishButton;
    private bool _drawFinish;
    private bool _shopUnlocked;

    private readonly Text _objectiveDisplay;
    private readonly string _objectiveInfoText = "Current objective:";
    private int _currentObjective;
    private readonly List<string> _objectives = new()
    {
        "Unlock the shop",
        "Purchase all upgrades",
        "Find the \"Finish\" button"
    };
    private string _objectiveText => $"{_objectiveInfoText} {_objectives[_currentObjective]}";
    
    private readonly Text hoverInfoDisplay;

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
        _overTimeMoverDistraction = new OverTimeMover(Camera, Camera.Position + new Vector2(-OneScreen.X, 0), 666F,
            OverTimeMover.MoveMode.Sin);
        AutoManaged.Add(_overTimeMoverShop);
        AutoManaged.Add(_overTimeMoverMain);
        AutoManaged.Add(_overTimeMoverDistraction);

        var shopButton = new TextButton("Shop");
        shopButton.GetCalculator(Camera.Rectangle).OnX(1F).OnY(1F).BySize(-1F).Move();
        shopButton.Click += ShopButtonClick;
        _shopButton = new LockButtonAddon(new(shopButton));

        AutoManaged.Add(_shopButton);

        var toMainButtonShop = new TextButton("Return");
        toMainButtonShop.GetCalculator(Camera.Rectangle).OnX(1F).OnY(1F).BySizeY(-1F).Move();
        toMainButtonShop.Click += ReturnButtonClick;
        AutoManaged.Add(toMainButtonShop);

        var toMainButtonSnake = new TextButton("Return");
        toMainButtonSnake.GetCalculator(Camera.Rectangle).OnX(0F).OnY(1F).BySize(-1F).Move();
        toMainButtonSnake.Click += ReturnButtonClick;
        AutoManaged.Add(toMainButtonSnake);

        var toSnakeButtonShop = new TextButton("Distraction");
        toSnakeButtonShop.GetCalculator(Camera.Rectangle).OnX(1F).OnY(1F).ByGridX(1F).BySize(-1F).Move();
        toSnakeButtonShop.Click += SnakeButtonClick;

        var toSnakeLockShop = new LockButtonAddon(new(toSnakeButtonShop));
        AutoManaged.Add(toSnakeLockShop);

        var toShopButtonSnake = new TextButton("Shop");
        toShopButtonSnake.GetCalculator(Camera.Rectangle).OnX(0F).OnY(1F).ByGridX(-1F).BySizeY(-1F).Move();
        toShopButtonSnake.Click += ShopButtonClick;

        var toShopLockSnake = new LockButtonAddon(new(toShopButtonSnake));
        AutoManaged.Add(toShopLockSnake);

        var clickButton = new TextButton("Bake a Bean!");
        clickButton.Move(OneScreen / 2 - clickButton.Size / 2);
        clickButton.Click += o => _shop.IncreaseBeanCount();
        AutoManaged.Add(clickButton);

        var snakeButton = new TextButton("Distraction");
        snakeButton.GetCalculator(Camera.Rectangle).OnX(0F).OnY(1F).BySizeY(-1F).Move();
        snakeButton.Click += SnakeButtonClick;
        _snakeButton = new LockButtonAddon(new(snakeButton));
        AutoManaged.Add(_snakeButton);

        _finishButton = new TextButton("Finish");
        _finishButton.Click += Finish;
        _finishButton.GetCalculator(Camera.Rectangle).OnCenter().Centered().ByGridX(-1F).Move();

        _shop = new Shop(shopScreen, OneScreen, _storage.GameData.Level6, random);
        _shop.UnlockedShop += _shopButton.Unlock;
        _shop.UnlockedShop += toShopLockSnake.Unlock;
        _shop.UnlockedShop += UnlockedShop;
        _shop.UnlockedDistraction += _snakeButton.Unlock;
        _shop.UnlockedDistraction += toSnakeLockShop.Unlock;
        _shop.PurchasedAllOptions += EnableFinishButton;
        AutoManaged.Add(_shop);

        _counter = new Text(string.Empty);
        _counter.GetCalculator(Camera.Rectangle).OnCenter().BySizeY(-0.5F).OnY(0.3F).Move();
        AutoManaged.Add(_counter);

        _objectiveDisplay = new Text(_objectiveText, _display.SimpleScale)
        {
            IsStatic = true
        };
        _objectiveDisplay.GetCalculator(_display.Screen).OnX(0.01F).OnY(0.01F).Move();
        AutoManaged.Add(_objectiveDisplay);

        var cursor = new Cursor();
        Actuator = cursor;
        PositionListener.Add(_mouse, cursor);
        AutoManaged.Add(cursor);
    }

    private void UnlockedShop()
    {
        if (_shopUnlocked)
            return;
        _shopUnlocked = true;
        AdvanceObjective();
    }

    private void AdvanceObjective()
        => _currentObjective++;

    private void EnableFinishButton()
    {
        if (_drawFinish)
            return;
        _drawFinish = true;
        AdvanceObjective();
    }

    private void SnakeButtonClick(object obj)
    {
        if (IsAnyOverTimeMoverRunning())
            return;

        _overTimeMoverDistraction.Start();
    }

    private void ReturnButtonClick(object obj)
    {
        if (IsAnyOverTimeMoverRunning())
            return;

        _overTimeMoverMain.Start();
    }

    private void ShopButtonClick(object obj)
    {
        if (IsAnyOverTimeMoverRunning())
            return;

        _overTimeMoverShop.Start();
    }

    public override void Update(GameTime gameTime)
    {
        _objectiveDisplay.ChangeText(_objectiveText);
        _counter.ChangeText(_shop.BeanDisplay);
        _finishButton.Update(gameTime);
        if (_drawFinish)
            _finishButton.UpdateInteraction(gameTime, Actuator);
        base.Update(gameTime);
        _counter.GetCalculator(_originScreen).OnCenter().Centered().OnY(3, 10).Move();
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (_drawFinish)
            _finishButton.Draw(spriteBatch);
        base.Draw(spriteBatch);
    }

    private bool IsAnyOverTimeMoverRunning()
        => _overTimeMoverMain.IsMoving || _overTimeMoverDistraction.IsMoving || _overTimeMoverShop.IsMoving;
}