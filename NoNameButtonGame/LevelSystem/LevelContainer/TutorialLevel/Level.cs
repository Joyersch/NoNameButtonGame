using System;
using Joyersch.Monogame;
using Joyersch.Monogame.Sound;
using Joyersch.Monogame.Storage;
using Joyersch.Monogame.Ui;
using Microsoft.Xna.Framework;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.Music;
using NoNameButtonGame.Ui;
using NoNameButtonGame.Ui.Buttons.AddOn;
using NoNameButtonGame.Ui.Text;

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

        var keyInfo = new BasicText(textComponent.GetValue("KeyInfo"), 1f);
        AutoManaged.Add(keyInfo);
        AutoScale.Add(keyInfo);

        positionCalculator = keyInfo.InRectangle(Camera)
            .OnX(0)
            .OnY(0)
            // Note: this will not dynamically scale this as the calculation is done here. I'm just lazy!
            .With(5 * Display.Scale, 5 * Display.Scale);
        CalculatorCollection.Register(positionCalculator);

        var startButton = new Button(textComponent.GetValue("Button1"));
        startButton.Click += MoveToNextScreen;
        AutoManaged.Add(startButton);
        AutoScale.Add(startButton);

        positionCalculator = startButton.InRectangle(Camera)
            .OnCenter()
            .Centered();
        CalculatorCollection.Register(positionCalculator);

        var infoText = new BasicText(textComponent.GetValue("Info1"), 2f);
        AutoManaged.Add(infoText);
        AutoScale.Add(infoText);

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
        AutoScale.Add(magicButton);

        positionCalculator = magicButton.InRectangle(Camera)
            .OnCenter()
            .OnY(13, 16)
            .Centered()
            .ByGridY(1);
        CalculatorCollection.Register(positionCalculator);

        var lockButton = new Button(textComponent.GetValue("ButtonSkip"));
        _lockButtonAddon = new LockButtonAddon(lockButton, 3f);
        _lockButtonAddon.Click += delegate { MoveToNextScreen(_lockButtonAddon); };
        AutoManaged.Add(_lockButtonAddon);
        AutoScale.Add(_lockButtonAddon);

        positionCalculator = _lockButtonAddon.InRectangle(Camera)
            .OnCenter()
            .OnY(3, 16)
            .Centered()
            .ByGridY(1);
        CalculatorCollection.Register(positionCalculator);

        var info1 = new BasicText(textComponent.GetValue("Info2"), 2f);
        AutoManaged.Add(info1);
        AutoScale.Add(info1);

        positionCalculator = info1.InRectangle(Camera)
            .OnCenter()
            .OnY(7, 20)
            .Centered()
            .ByGridY(1);
        CalculatorCollection.Register(positionCalculator);


        var info2 = new BasicText(textComponent.GetValue("Info3"), 2f);
        AutoManaged.Add(info2);
        AutoScale.Add(info2);

        positionCalculator = info2.InRectangle(Camera)
            .OnCenter()
            .OnY(13, 20)
            .Centered()
            .ByGridY(1);
        CalculatorCollection.Register(positionCalculator);

        #endregion // LockButtonScreen

        #region CounterButtonScreen

        var counterButton = new Button(textComponent.GetValue("ButtonSkip"));
        var counterButtonAddon = new CounterButtonAddon(counterButton, 5, 2f);
        counterButtonAddon.Click += delegate { MoveToNextScreen(counterButtonAddon); };
        AutoManaged.Add(counterButtonAddon);
        AutoScale.Add(counterButtonAddon);

        positionCalculator = counterButtonAddon.InRectangle(Camera)
            .OnCenter()
            .Centered()
            .ByGridY(2);
        CalculatorCollection.Register(positionCalculator);

        var infoAboutCounterButton = new BasicText(textComponent.GetValue("Info4"), 2f);
        AutoManaged.Add(infoAboutCounterButton);
        AutoScale.Add(infoAboutCounterButton);

        positionCalculator = infoAboutCounterButton.InRectangle(Camera)
            .OnCenter()
            .OnY(3, 10)
            .Centered()
            .ByGridY(2);
        CalculatorCollection.Register(positionCalculator);

        var infoAboutCounterButton2 = new BasicText(textComponent.GetValue("Info5"), 2f);
        AutoManaged.Add(infoAboutCounterButton2);
        AutoScale.Add(infoAboutCounterButton2);

        positionCalculator = infoAboutCounterButton2.InRectangle(Camera)
            .OnCenter()
            .OnY(7, 10)
            .Centered()
            .ByGridY(2);
        CalculatorCollection.Register(positionCalculator);

        #endregion // CounterButtonScreen

        #region HoldButtonScreen

        var stateButton = new Button(textComponent.GetValue("ButtonFinish"));
        var holdButtonAddon = new HoldButtonAddon(stateButton, 3000F, 2f);
        holdButtonAddon.Click += Finish;
        AutoManaged.Add(holdButtonAddon);
        AutoScale.Add(holdButtonAddon);

        positionCalculator = holdButtonAddon.InRectangle(Camera)
            .OnCenter()
            .Centered()
            .ByGridY(3);
        CalculatorCollection.Register(positionCalculator);

        var infoAboutButton = new BasicText(textComponent.GetValue("Info6"), 2f);
        AutoManaged.Add(infoAboutButton);
        AutoScale.Add(infoAboutButton);

        positionCalculator = infoAboutButton.InRectangle(Camera)
            .OnCenter()
            .OnY(3, 10)
            .Centered()
            .ByGridY(3);
        CalculatorCollection.Register(positionCalculator);

        var infoAboutButton2 = new BasicText(textComponent.GetValue("Info7"), 2f);
        AutoManaged.Add(infoAboutButton2);
        AutoScale.Add(infoAboutButton2);

        positionCalculator = infoAboutButton2.InRectangle(Camera)
            .OnCenter()
            .OnY(7, 10)
            .Centered()
            .ByGridY(3);
        CalculatorCollection.Register(positionCalculator);

        #endregion // HoldButtonScreen

        SetScaleAndCalculatePositions();
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