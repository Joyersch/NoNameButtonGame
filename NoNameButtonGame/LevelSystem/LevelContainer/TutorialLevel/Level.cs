using System;
using Microsoft.Xna.Framework;
using MonoUtils;
using MonoUtils.Logic;
using MonoUtils.Logic.Text;
using MonoUtils.Settings;
using MonoUtils.Sound;
using MonoUtils.Ui;
using MonoUtils.Ui.Logic;
using MonoUtils.Ui.Buttons.AddOn;
using MonoUtils.Ui.TextSystem;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.LevelSystem.Settings;
using NoNameButtonGame.Music;

namespace NoNameButtonGame.LevelSystem.LevelContainer.TutorialLevel;

internal class Level : SampleLevel
{
    private readonly LockButtonAddon _lockButtonAddon;
    private readonly OverTimeMover _mover;


    public Level(Scene scene, Random random, EffectsRegistry effectsRegistry,
        SettingsAndSaveManager<string> settingsAndSaveManager) : base(scene, random, effectsRegistry,
        settingsAndSaveManager)
    {
        TextComponent textComponent = TextProvider.GetText("Levels.TutorialLevel");
        Name = textComponent.GetValue("Name");

        Default.Play();

        AnchorCalculator anchorCalculator = null;
        PositionCalculator positionCalculator = null;

        #region StartScreen

        var keyInfo = new Text(textComponent.GetValue("KeyInfo"), 0.5F * Text.DefaultLetterScale);
        AutoManaged.Add(keyInfo);
        DynamicScaler.Register(keyInfo);

        positionCalculator = keyInfo.InRectangle(Camera)
            .OnX(0)
            .OnY(0)
            // Note: this will not dynamically scale this as the calculation is done here. I'm just lazy!
            .With(5 * Display.Scale, 5 * Display.Scale);
        CalculatorCollection.Register(positionCalculator);

        var startButton = new Button(textComponent.GetValue("Button1"));
        startButton.Click += MoveToNextScreen;
        AutoManaged.Add(startButton);
        DynamicScaler.Register(startButton);

        positionCalculator = startButton.InRectangle(Camera)
            .OnCenter()
            .Centered();
        CalculatorCollection.Register(positionCalculator);

        var infoText = new Text(textComponent.GetValue("Info1"));
        AutoManaged.Add(infoText);
        DynamicScaler.Register(infoText);

        positionCalculator = infoText.InRectangle(Camera)
            .OnCenter()
            .OnY(3, 10)
            .Centered();
        CalculatorCollection.Register(positionCalculator);

        #endregion // StartScreen

        var secondScreen = new PositionCalculator(Camera, Camera).ByGridY(1).Calculate();

        _mover = new OverTimeMover(Camera, secondScreen, 600F, OverTimeMover.MoveMode.Sin);
        AutoManaged.Add(_mover);

        #region LockButtonScreen

        var magicButton = new Button(textComponent.GetValue("Button2"));
        magicButton.Click += MagicButtonOnClick;
        AutoManaged.Add(magicButton);
        DynamicScaler.Register(magicButton);

        positionCalculator = magicButton.InRectangle(Camera)
            .OnCenter()
            .OnY(13, 16)
            .Centered()
            .ByGridY(1);
        CalculatorCollection.Register(positionCalculator);

        var lockButton = new Button(textComponent.GetValue("ButtonSkip"));
        _lockButtonAddon = new LockButtonAddon(lockButton);
        _lockButtonAddon.Click += delegate { MoveToNextScreen(_lockButtonAddon); };
        AutoManaged.Add(_lockButtonAddon);
        DynamicScaler.Register(_lockButtonAddon);

        positionCalculator = _lockButtonAddon.InRectangle(Camera)
            .OnCenter()
            .OnY(3, 16)
            .Centered()
            .ByGridY(1);
        CalculatorCollection.Register(positionCalculator);

        var info1 = new Text(textComponent.GetValue("Info2"));
        AutoManaged.Add(info1);
        DynamicScaler.Register(info1);

        positionCalculator = info1.InRectangle(Camera)
            .OnCenter()
            .OnY(7, 20)
            .Centered()
            .ByGridY(1);
        CalculatorCollection.Register(positionCalculator);


        var info2 = new Text(textComponent.GetValue("Info3"));
        AutoManaged.Add(info2);
        DynamicScaler.Register(info2);

        positionCalculator = info2.InRectangle(Camera)
            .OnCenter()
            .OnY(13, 20)
            .Centered()
            .ByGridY(1);
        CalculatorCollection.Register(positionCalculator);

        #endregion // LockButtonScreen

        #region CounterButtonScreen

        var counterButton = new Button(textComponent.GetValue("ButtonSkip"));
        var counterButtonAddon = new CounterButtonAddon(counterButton, 5);
        counterButtonAddon.Click += delegate { MoveToNextScreen(counterButtonAddon); };
        AutoManaged.Add(counterButtonAddon);
        DynamicScaler.Register(counterButtonAddon);

        positionCalculator = counterButtonAddon.InRectangle(Camera)
            .OnCenter()
            .Centered()
            .ByGridY(2);
        CalculatorCollection.Register(positionCalculator);

        var infoAboutCounterButton = new Text(textComponent.GetValue("Info4"));
        AutoManaged.Add(infoAboutCounterButton);
        DynamicScaler.Register(infoAboutCounterButton);

        positionCalculator = infoAboutCounterButton.InRectangle(Camera)
            .OnCenter()
            .OnY(3, 10)
            .Centered()
            .ByGridY(2);
        CalculatorCollection.Register(positionCalculator);

        var infoAboutCounterButton2 = new Text(textComponent.GetValue("Info5"));
        AutoManaged.Add(infoAboutCounterButton2);
        DynamicScaler.Register(infoAboutCounterButton2);

        positionCalculator = infoAboutCounterButton2.InRectangle(Camera)
            .OnCenter()
            .OnY(7, 10)
            .Centered()
            .ByGridY(2);
        CalculatorCollection.Register(positionCalculator);

        #endregion // CounterButtonScreen

        #region HoldButtonScreen

        var stateButton = new Button(textComponent.GetValue("ButtonFinish"));
        var holdButtonAddon = new HoldButtonAddon(stateButton, 3000F);
        holdButtonAddon.Click += Finish;
        AutoManaged.Add(holdButtonAddon);
        DynamicScaler.Register(holdButtonAddon);

        positionCalculator = holdButtonAddon.InRectangle(Camera)
            .OnCenter()
            .Centered()
            .ByGridY(3);
        CalculatorCollection.Register(positionCalculator);

        var infoAboutButton = new Text(textComponent.GetValue("Info6"));
        AutoManaged.Add(infoAboutButton);
        DynamicScaler.Register(infoAboutButton);

        positionCalculator = infoAboutButton.InRectangle(Camera)
            .OnCenter()
            .OnY(3, 10)
            .Centered()
            .ByGridY(3);
        CalculatorCollection.Register(positionCalculator);

        var infoAboutButton2 = new Text(textComponent.GetValue("Info7"));
        AutoManaged.Add(infoAboutButton2);
        DynamicScaler.Register(infoAboutButton2);

        positionCalculator = infoAboutButton2.InRectangle(Camera)
            .OnCenter()
            .OnY(7, 10)
            .Centered()
            .ByGridY(3);
        CalculatorCollection.Register(positionCalculator);

        #endregion // HoldButtonScreen

        DynamicScaler.Apply(Display.Scale);
        CalculatorCollection.Apply();
    }

    private void MoveToNextScreen(object sender)
    {
        if (_mover.IsMoving)
            return;

        _mover.ChangeDestination(new Vector2(Camera.Position.X, Camera.Position.Y + Camera.Rectangle.Height));
        _mover.Start();
    }

    private void MagicButtonOnClick(object obj)
    {
        if (_lockButtonAddon.IsLocked)
            _lockButtonAddon.Unlock();
        else
            _lockButtonAddon.Lock();
    }
}