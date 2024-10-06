using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logic;
using MonoUtils.Logic.Text;
using MonoUtils.Settings;
using MonoUtils.Sound;
using MonoUtils.Ui;
using MonoUtils.Ui.Logic;
using MonoUtils.Ui.Buttons;
using MonoUtils.Ui.Buttons.AddOn;
using MonoUtils.Ui.TextSystem;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.Music;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace NoNameButtonGame.LevelSystem.LevelContainer.CookieClickerLevel;

public class Level : SampleLevel
{
    private readonly SettingsAndSaveManager<string> _settingsAndSave;
    private LevelSave _save;

    private OverTimeMover _overTimeMoverShop;
    private OverTimeMover _overTimeMoverMain;
    private OverTimeMover _overTimeMoverDistraction;

    private Shop _shop;
    private Text _counter;

    private readonly Rectangle _originScreen;

    private readonly TextButton<SampleButton> _finishButton;
    private bool _drawFinish;
    private bool _shopUnlocked;

    private readonly Text _objectiveDisplay;
    private readonly string _objectiveInfoText;
    private int _currentObjective;

    private readonly List<string> _objectives;

    private string ObjectiveText => $"{_objectiveInfoText} {_objectives[_currentObjective]}";

    public Level(Scene scene, Random random, SettingsAndSaveManager<string> settingsAndSave,
        EffectsRegistry effectsRegistry) : base(scene, random,
        effectsRegistry, settingsAndSave)
    {
        _settingsAndSave = settingsAndSave;
        var textComponent = TextProvider.GetText("Levels.CookieClickerLevel");
        Name = textComponent.GetValue("Name");
        Lofi.Play();

        _objectiveInfoText = textComponent.GetValue("ObjectiveInfo");

        _objectives = new()
        {
            textComponent.GetValue("Objective1"),
            textComponent.GetValue("Objective2"),
            textComponent.GetValue("Objective3")
        };

        _save = settingsAndSave.GetSave<LevelSave>();

        AnchorCalculator anchorCalculator = null;
        PositionCalculator positionCalculator = null;

        var oneScreen = Display.Size;
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

        var shopButton = new Button(textComponent.GetValue("Shop"));
        shopButton.Click += ShopButtonClick;
        var shopButton1 = new LockButtonAddon(shopButton);
        AutoManaged.Add(shopButton1);
        DynamicScaler.Register(shopButton1);

        positionCalculator = shopButton1.InRectangle(Camera)
            .OnX(1F)
            .OnY(1F)
            .BySize(-1F);
        CalculatorCollection.Register(positionCalculator);

        var toMainButtonShop = new Button(textComponent.GetValue("Return"));
        toMainButtonShop.Click += ReturnButtonClick;
        AutoManaged.Add(toMainButtonShop);
        DynamicScaler.Register(toMainButtonShop);

        positionCalculator = toMainButtonShop.InRectangle(Camera)
            .OnX(1F)
            .OnY(1F)
            .BySizeY(-1F);
        CalculatorCollection.Register(positionCalculator);

        var toMainButtonDistraction = new Button(textComponent.GetValue("Return"));
        toMainButtonDistraction.Click += ReturnButtonClick;
        AutoManaged.Add(toMainButtonDistraction);
        DynamicScaler.Register(toMainButtonDistraction);

        positionCalculator = toMainButtonDistraction.InRectangle(Camera)
            .OnX(0F)
            .OnY(1F)
            .BySize(-1F);
        CalculatorCollection.Register(positionCalculator);

        var toDistractionButtonShop = new Button(textComponent.GetValue("Distraction"));
        toDistractionButtonShop.Click += DistractionButtonClick;
        var toDistractionLockShop = new LockButtonAddon(toDistractionButtonShop);
        AutoManaged.Add(toDistractionLockShop);
        DynamicScaler.Register(toDistractionLockShop);

        positionCalculator = toDistractionLockShop.InRectangle(Camera)
            .OnX(1F)
            .OnY(1F)
            .ByGridX(1F)
            .BySize(-1F);
        CalculatorCollection.Register(positionCalculator);

        var toShopButtonDistraction = new Button(textComponent.GetValue("Shop"));
        toShopButtonDistraction.Click += ShopButtonClick;
        var toShopLockDistraction = new LockButtonAddon(toShopButtonDistraction);
        AutoManaged.Add(toShopLockDistraction);
        DynamicScaler.Register(toShopLockDistraction);

        positionCalculator = toShopButtonDistraction.InRectangle(Camera)
            .OnX(0F)
            .OnY(1F)
            .ByGridX(-1F)
            .BySizeY(-1F);
        CalculatorCollection.Register(positionCalculator);

        var clickButton = new Button(textComponent.GetValue("BakeABean"));

        clickButton.Click += _ => _shop!.IncreaseBeanCount();
        AutoManaged.Add(clickButton);
        DynamicScaler.Register(clickButton);

        positionCalculator = clickButton.InRectangle(Camera)
            .OnCenter()
            .Centered();
        CalculatorCollection.Register(positionCalculator);

        var distractionButton = new Button(textComponent.GetValue("Distraction"));
        distractionButton.Click += DistractionButtonClick;
        var distractionLockButton = new LockButtonAddon(distractionButton);
        AutoManaged.Add(distractionLockButton);
        DynamicScaler.Register(distractionLockButton);

        positionCalculator = distractionLockButton.InRectangle(Camera)
            .OnX(0F)
            .OnY(1F)
            .BySizeY(-1F);
        CalculatorCollection.Register(positionCalculator);

        _finishButton = new Button(textComponent.GetValue("Finish"));
        _finishButton.Click += Finish;
        DynamicScaler.Register(_finishButton);

        positionCalculator = _finishButton.InRectangle(Camera)
            .OnCenter()
            .Centered()
            .ByGridX(-1F);
        CalculatorCollection.Register(positionCalculator);

        var infos = new[]
        {
            textComponent.GetValue("ShopInfo1"),
            textComponent.GetValue("ShopInfo2"),
            textComponent.GetValue("ShopInfo3"),
            textComponent.GetValue("ShopInfo4")
        };

        var shopOptions = new[]
        {
            textComponent.GetValue("ShopOption1"),
            textComponent.GetValue("ShopOption2"),
            textComponent.GetValue("ShopOption3"),
            textComponent.GetValue("ShopOption4")
        };

        var shopOptionsSus = new[]
        {
            textComponent.GetValue("ShopOption1Sus"),
            textComponent.GetValue("ShopOption2Sus"),
            textComponent.GetValue("ShopOption3Sus"),
            textComponent.GetValue("ShopOption4Sus")
        };

        _shop = new Shop(shopScreen, oneScreen, _save, random, infos, shopOptions, shopOptionsSus);

        _shop.UnlockedShop += shopButton1.Unlock;
        _shop.UnlockedShop += toShopLockDistraction.Unlock;
        _shop.UnlockedShop += UnlockedShop;
        _shop.UnlockedDistraction += distractionLockButton.Unlock;
        _shop.UnlockedDistraction += toDistractionLockShop.Unlock;
        _shop.PurchasedAllOptions += EnableFinishButton;

        AutoManaged.Add(_shop);
        DynamicScaler.Register(_shop);

        _counter = new Text(string.Empty);
        AutoManaged.Add(_counter);
        DynamicScaler.Register(_counter);

        positionCalculator = _counter.InRectangle(Camera)
            .OnCenter()
            .BySizeY(-0.5F)
            .OnY(0.3F);
        CalculatorCollection.Register(positionCalculator);


        _objectiveDisplay = new Text(ObjectiveText, Display.Scale / 2 * Text.DefaultLetterScale);
        AutoManagedStaticFront.Add(_objectiveDisplay);
        DynamicScaler.Register(_objectiveDisplay);

        positionCalculator = _objectiveDisplay.InRectangle(Display)
            .OnX(0.01F)
            .OnY(0.01F);
        CalculatorCollection.Register(positionCalculator);

        var nbg = new Nbg(new Rectangle((int)-oneScreen.X, 0, (int)oneScreen.X, (int)oneScreen.Y), random, 5F);
        AutoManaged.Add(nbg);
        DynamicScaler.Register(nbg);

        DynamicScaler.Apply(Display.Scale);
        CalculatorCollection.Apply();
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

    private void DistractionButtonClick(object obj)
    {
        if (IsAnyOverTimeMoverRunning())
            return;

        LofiMuffled.Play();
        _overTimeMoverDistraction.Start();
    }

    private void ReturnButtonClick(object obj)
    {
        if (IsAnyOverTimeMoverRunning())
            return;

        Lofi.Play();
        _overTimeMoverMain.Start();
    }

    private void ShopButtonClick(object obj)
    {
        if (IsAnyOverTimeMoverRunning())
            return;

        Lofi.Play();
        _overTimeMoverShop.Start();
    }

    public override void Update(GameTime gameTime)
    {
        _objectiveDisplay.ChangeText(ObjectiveText);
        _counter.ChangeText(_shop.BeanDisplay);
        _finishButton.Update(gameTime);
        if (_drawFinish)
            _finishButton.UpdateInteraction(gameTime, Cursor);
        base.Update(gameTime);
        _counter.InRectangle(new RectangleWrapper(_originScreen)).OnCenter().Centered().OnY(3, 10).Apply();
    }

    protected override void Draw(SpriteBatch spriteBatch)
    {
        if (_drawFinish)
            _finishButton.Draw(spriteBatch);
        base.Draw(spriteBatch);
    }

    private bool IsAnyOverTimeMoverRunning()
        => _overTimeMoverMain.IsMoving || _overTimeMoverDistraction.IsMoving || _overTimeMoverShop.IsMoving;

    public override void Exit()
    {
        _settingsAndSave.SaveSave();
        base.Exit();
    }

    public override void Finish()
    {
        _save.Reset();
        _settingsAndSave.SaveSave();
        base.Finish();
    }
}