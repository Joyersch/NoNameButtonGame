using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logic;
using MonoUtils.Logic.Text;
using MonoUtils.Settings;
using MonoUtils.Ui;
using MonoUtils.Ui.Logic;
using MonoUtils.Ui.Objects;
using MonoUtils.Ui.Objects.Buttons;
using MonoUtils.Ui.Objects.Buttons.AddOn;
using MonoUtils.Ui.Objects.TextSystem;
using NoNameButtonGame.GameObjects;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level11;

public class Level : SampleLevel
{
    private LevelSave _save;

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
    private readonly string _objectiveInfoText;
    private int _currentObjective;

    private readonly List<string> _objectives;

    private string ObjectiveText => $"{_objectiveInfoText} {_objectives[_currentObjective]}";

    public Level(Display display, Vector2 window, Random random, SettingsAndSaveManager settingsAndSave) : base(display,
        window, random)
    {
        var textComponent = TextProvider.GetText("Levels.Level11");
        Name = textComponent.GetValue("Name");

        _objectiveInfoText = textComponent.GetValue("ObjectiveInfo");

        _objectives = new()
        {
            textComponent.GetValue("Objective1"),
            textComponent.GetValue("Objective2"),
            textComponent.GetValue("Objective3")
        };

        _save = settingsAndSave.GetSave<LevelSave>();

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

        var shopButton = new TextButton(textComponent.GetValue("Shop"));
        shopButton.GetCalculator(Camera.Rectangle).OnX(1F).OnY(1F).BySize(-1F).Move();
        shopButton.Click += ShopButtonClick;
        var shopButton1 = new LockButtonAddon(new(shopButton));

        AutoManaged.Add(shopButton1);

        var toMainButtonShop = new TextButton(textComponent.GetValue("Return"));
        toMainButtonShop.GetCalculator(Camera.Rectangle).OnX(1F).OnY(1F).BySizeY(-1F).Move();
        toMainButtonShop.Click += ReturnButtonClick;
        AutoManaged.Add(toMainButtonShop);

        var toMainButtonSnake = new TextButton(textComponent.GetValue("Return"));
        toMainButtonSnake.GetCalculator(Camera.Rectangle).OnX(0F).OnY(1F).BySize(-1F).Move();
        toMainButtonSnake.Click += ReturnButtonClick;
        AutoManaged.Add(toMainButtonSnake);

        var toSnakeButtonShop = new TextButton(textComponent.GetValue("Distraction"));
        toSnakeButtonShop.GetCalculator(Camera.Rectangle).OnX(1F).OnY(1F).ByGridX(1F).BySize(-1F).Move();
        toSnakeButtonShop.Click += SnakeButtonClick;

        var toSnakeLockShop = new LockButtonAddon(new(toSnakeButtonShop));
        AutoManaged.Add(toSnakeLockShop);

        var toShopButtonSnake = new TextButton(textComponent.GetValue("Shop"));
        toShopButtonSnake.GetCalculator(Camera.Rectangle).OnX(0F).OnY(1F).ByGridX(-1F).BySizeY(-1F).Move();
        toShopButtonSnake.Click += ShopButtonClick;

        var toShopLockSnake = new LockButtonAddon(new(toShopButtonSnake));
        AutoManaged.Add(toShopLockSnake);

        var clickButton = new TextButton(textComponent.GetValue("BakeABean"));
        clickButton.Move(oneScreen / 2 - clickButton.Size / 2);
        clickButton.Click += _ => _shop.IncreaseBeanCount();
        AutoManaged.Add(clickButton);

        // originally it was planned to have snake on the distraction screen but the idea was cut
        var snakeButton = new TextButton(textComponent.GetValue("Distraction"));
        snakeButton.GetCalculator(Camera.Rectangle).OnX(0F).OnY(1F).BySizeY(-1F).Move();
        snakeButton.Click += SnakeButtonClick;
        var snakeLockButton = new LockButtonAddon(new(snakeButton));
        AutoManaged.Add(snakeLockButton);

        _finishButton = new TextButton(textComponent.GetValue("Finish"));
        _finishButton.Click += FinishLevel;
        _finishButton.GetCalculator(Camera.Rectangle).OnCenter().Centered().ByGridX(-1F).Move();

        var infos = new string[4]
        {
            textComponent.GetValue("ShopInfo1"),
            textComponent.GetValue("ShopInfo2"),
            textComponent.GetValue("ShopInfo3"),
            textComponent.GetValue("ShopInfo4")
        };

        var shopOptions = new string[4]
        {
            textComponent.GetValue("ShopOption1"),
            textComponent.GetValue("ShopOption2"),
            textComponent.GetValue("ShopOption3"),
            textComponent.GetValue("ShopOption4")
        };
        _shop = new Shop(shopScreen, oneScreen, _save, random, infos, shopOptions);
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

        _objectiveDisplay = new Text(ObjectiveText, Display.SimpleScale);
        _objectiveDisplay.GetCalculator(Display.Screen).OnX(0.01F).OnY(0.01F).Move();
        AutoManagedStatic.Add(_objectiveDisplay);

        var nbg = new Nbg(new Rectangle((int)-oneScreen.X, 0, (int)oneScreen.X, (int)oneScreen.Y), random, 2.5F);
        AutoManaged.Add(nbg);
        var cursor = new Cursor();
        Actuator = cursor;
        PositionListener.Add(Mouse, cursor);
        AutoManaged.Add(cursor);
    }

    private void FinishLevel(object obj)
    {
        _save.Reset();
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

    protected override void Draw(SpriteBatch spriteBatch)
    {
        if (_drawFinish)
            _finishButton.Draw(spriteBatch);
        base.Draw(spriteBatch);
    }

    private bool IsAnyOverTimeMoverRunning()
        => _overTimeMoverMain.IsMoving || _overTimeMoverDistraction.IsMoving || _overTimeMoverShop.IsMoving;
}