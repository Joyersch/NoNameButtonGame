using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils;
using MonoUtils.Logic;
using MonoUtils.Logic.Text;
using MonoUtils.Settings;
using MonoUtils.Sound;
using MonoUtils.Ui;
using MonoUtils.Ui.Logic;
using MonoUtils.Ui.Buttons;
using MonoUtils.Ui.Buttons.AddOn;
using MonoUtils.Ui.TextSystem;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.LevelSystem.Settings;
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
        shopButton.InRectangle(Camera.Rectangle).OnX(1F).OnY(1F).BySize(-1F).Move();
        shopButton.Click += ShopButtonClick;
        var shopButton1 = new LockButtonAddon(shopButton);
        AutoManaged.Add(shopButton1);

        var toMainButtonShop = new Button(textComponent.GetValue("Return"));
        toMainButtonShop.InRectangle(Camera.Rectangle).OnX(1F).OnY(1F).BySizeY(-1F).Move();
        toMainButtonShop.Click += ReturnButtonClick;
        AutoManaged.Add(toMainButtonShop);

        var toMainButtonDistraction = new Button(textComponent.GetValue("Return"));
        toMainButtonDistraction.InRectangle(Camera.Rectangle).OnX(0F).OnY(1F).BySize(-1F).Move();
        toMainButtonDistraction.Click += ReturnButtonClick;
        AutoManaged.Add(toMainButtonDistraction);

        var toDistractionButtonShop = new Button(textComponent.GetValue("Distraction"));
        toDistractionButtonShop.InRectangle(Camera.Rectangle).OnX(1F).OnY(1F).ByGridX(1F).BySize(-1F).Move();
        toDistractionButtonShop.Click += DistractionButtonClick;

        var toDistractionLockShop = new LockButtonAddon(toDistractionButtonShop);
        AutoManaged.Add(toDistractionLockShop);

        var toShopButtonDistraction = new Button(textComponent.GetValue("Shop"));
        toShopButtonDistraction.InRectangle(Camera.Rectangle).OnX(0F).OnY(1F).ByGridX(-1F).BySizeY(-1F).Move();
        toShopButtonDistraction.Click += ShopButtonClick;

        var toShopLockDistraction = new LockButtonAddon(toShopButtonDistraction);
        AutoManaged.Add(toShopLockDistraction);

        var clickButton = new Button(textComponent.GetValue("BakeABean"));
        clickButton.Move(oneScreen / 2 - clickButton.GetSize() / 2);
        clickButton.Click += _ => _shop!.IncreaseBeanCount();
        AutoManaged.Add(clickButton);

        var distractionButton = new Button(textComponent.GetValue("Distraction"));
        distractionButton.InRectangle(Camera.Rectangle).OnX(0F).OnY(1F).BySizeY(-1F).Move();
        distractionButton.Click += DistractionButtonClick;
        var distractionLockButton = new LockButtonAddon(distractionButton);
        AutoManaged.Add(distractionLockButton);

        _finishButton = new Button(textComponent.GetValue("Finish"));
        _finishButton.Click += Finish;
        _finishButton.InRectangle(Camera.Rectangle).OnCenter().Centered().ByGridX(-1F).Move();

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

        _counter = new Text(string.Empty);
        _counter.InRectangle(Camera.Rectangle).OnCenter().BySizeY(-0.5F).OnY(0.3F).Move();
        AutoManaged.Add(_counter);

        _objectiveDisplay = new Text(ObjectiveText, Display.SimpleScale);
        _objectiveDisplay.InRectangle(Display.Screen).OnX(0.01F).OnY(0.01F).Move();
        AutoManagedStaticFront.Add(_objectiveDisplay);

        var nbg = new Nbg(new Rectangle((int)-oneScreen.X, 0, (int)oneScreen.X, (int)oneScreen.Y), random, 5F);
        AutoManaged.Add(nbg);
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
        _counter.InRectangle(_originScreen).OnCenter().Centered().OnY(3, 10).Move();
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