using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logic;
using MonoUtils.Objects;
using MonoUtils.Objects.Buttons;
using MonoUtils.Objects.Buttons.AddOn;
using MonoUtils.Objects.TextSystem;
using MonoUtils.Ui;
using NoNameButtonGame.GameObjects;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level2;

public class Level : SampleLevel
{
    private Storage.Storage _storage;

    private OverTimeMover _overTimeMoverShop;
    private OverTimeMover _overTimeMoverMain;
    private OverTimeMover _overTimeMoverDistraction;

    private Shop _shop;
    private Text _counter;

    private readonly Rectangle _originScreen;

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

    private string ObjectiveText => $"{_objectiveInfoText} {_objectives[_currentObjective]}";

    public Level(Display display, Vector2 window, Random random, Storage.Storage storage) : base(display,
        window, random)
    {
        Name = "Level 2 - Just like Cookie Clicker but with BEANS!";

        _storage = storage;

        var oneScreen = Display.Size / 2;
        var shopScreen = new Vector2(oneScreen.X, 0);

        Camera.Move(oneScreen / 2);
        _originScreen = Camera.Rectangle;


        _overTimeMoverShop = new OverTimeMover(Camera, Camera.Position + shopScreen, 666F, OverTimeMover.MoveMode.Sin);
        _overTimeMoverMain = new OverTimeMover(Camera, Camera.Position, 666F, OverTimeMover.MoveMode.Sin);
        _overTimeMoverDistraction = new OverTimeMover(Camera, Camera.Position + new Vector2(-oneScreen.X, 0), 666F,
            OverTimeMover.MoveMode.Sin);
        AutoManaged.Add(_overTimeMoverShop);
        AutoManaged.Add(_overTimeMoverMain);
        AutoManaged.Add(_overTimeMoverDistraction);

        var shopButton = new TextButton("Shop");
        shopButton.GetCalculator(Camera.Rectangle).OnX(1F).OnY(1F).BySize(-1F).Move();
        shopButton.Click += ShopButtonClick;
        var shopButton1 = new LockButtonAddon(new(shopButton));

        AutoManaged.Add(shopButton1);

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
        clickButton.Move(oneScreen / 2 - clickButton.Size / 2);
        clickButton.Click += _ => _shop.IncreaseBeanCount();
        AutoManaged.Add(clickButton);

        // originally it was planned to have snake on the distraction screen but the idea was cut
        var snakeButton = new TextButton("Distraction");
        snakeButton.GetCalculator(Camera.Rectangle).OnX(0F).OnY(1F).BySizeY(-1F).Move();
        snakeButton.Click += SnakeButtonClick;
        var snakeLockButton = new LockButtonAddon(new(snakeButton));
        AutoManaged.Add(snakeLockButton);

        _finishButton = new TextButton("Finish");
        _finishButton.Click += FinishLevel;
        _finishButton.GetCalculator(Camera.Rectangle).OnCenter().Centered().ByGridX(-1F).Move();

        _shop = new Shop(shopScreen, oneScreen, _storage.GameData.Level2, random);
        _shop.UnlockedShop += shopButton1.Unlock;
        _shop.UnlockedShop += toShopLockSnake.Unlock;
        _shop.UnlockedShop += UnlockedShop;
        _shop.UnlockedDistraction += snakeLockButton.Unlock;
        _shop.UnlockedDistraction += toSnakeLockShop.Unlock;
        _shop.PurchasedAllOptions += EnableFinishButton;
        AutoManaged.Add(_shop);

        _counter = new Text(string.Empty);
        _counter.GetCalculator(Camera.Rectangle).OnCenter().BySizeY(-0.5F).OnY(0.3F).Move();
        AutoManaged.Add(_counter);

        _objectiveDisplay = new Text(ObjectiveText, Display.SimpleScale)
        {
            IsStatic = true
        };
        _objectiveDisplay.GetCalculator(Display.Screen).OnX(0.01F).OnY(0.01F).Move();
        AutoManaged.Add(_objectiveDisplay);

        var nbg = new Nbg(new Rectangle((int) -oneScreen.X, 0, (int) oneScreen.X, (int) oneScreen.Y), random, 2.5F);
        AutoManaged.Add(nbg);
        var cursor = new Cursor();
        Actuator = cursor;
        PositionListener.Add(Mouse, cursor);
        AutoManaged.Add(cursor);
    }

    private void FinishLevel(object obj)
    {
        _storage.GameData.Level2 = new StorageData();
        Finish();
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
        _objectiveDisplay.ChangeText(ObjectiveText);
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