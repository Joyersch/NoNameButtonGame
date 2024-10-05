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

        var screen = Camera.Rectangle;

        #region StartScreen

        var keyInfo = new Text(textComponent.GetValue("KeyInfo"), 0.5F * Text.DefaultLetterScale);
        keyInfo.InRectangle(Camera)
            .With(5, 5)
            .Apply();
        AutoManaged.Add(keyInfo);

        var startButton = new Button(textComponent.GetValue("Button1"));
        startButton.InRectangle(Camera)
            .OnCenter()
            .Centered()
            .Apply();
        startButton.Click += MoveToNextScreen;
        AutoManaged.Add(startButton);

        var infoText = new Text(textComponent.GetValue("Info1"));
        infoText.InRectangle(Camera)
            .OnCenter()
            .OnY(3, 10)
            .Centered()
            .Apply();
        AutoManaged.Add(infoText);

        #endregion // StartScreen

        screen.Y += Camera.Rectangle.Height;

        _mover = new OverTimeMover(Camera, screen.Location.ToVector2(), 600F, OverTimeMover.MoveMode.Sin);
        AutoManaged.Add(_mover);

        #region LockButtonScreen

        var magicButton = new Button(textComponent.GetValue("Button2"));
        magicButton.InRectangle(Camera)
            .OnCenter()
            .OnY(13, 16)
            .Centered()
            .Apply();
        magicButton.Click += MagicButtonOnClick;
        AutoManaged.Add(magicButton);

        var lockButton = new Button(textComponent.GetValue("ButtonSkip"));
        lockButton.InRectangle(Camera)
            .OnCenter()
            .OnY(3, 16)
            .Centered()
            .Apply();

        _lockButtonAddon = new LockButtonAddon(lockButton);
        _lockButtonAddon.Click += delegate { MoveToNextScreen(_lockButtonAddon); };
        AutoManaged.Add(_lockButtonAddon);


        var info1 = new Text(textComponent.GetValue("Info2"));
        info1.InRectangle(Camera)
            .OnCenter()
            .OnY(7, 20)
            .Centered()
            .Apply();
        AutoManaged.Add(info1);

        var info2 = new Text(textComponent.GetValue("Info3"));
        info2.InRectangle(Camera)
            .OnCenter()
            .OnY(13, 20)
            .Centered()
            .Apply();
        AutoManaged.Add(info2);

        #endregion // LockButtonScreen

        screen.Y += Camera.Rectangle.Height;

        #region CounterButtonScreen

        var counterButton = new Button(textComponent.GetValue("ButtonSkip"));
        counterButton.InRectangle(Camera)
            .OnCenter()
            .Centered()
            .Apply();

        var infoAboutCounterButton = new Text(textComponent.GetValue("Info4"));
        infoAboutCounterButton.InRectangle(Camera)
            .OnCenter()
            .OnY(3, 10)
            .Centered()
            .Apply();
        AutoManaged.Add(infoAboutCounterButton);

        var infoAboutCounterButton2 = new Text(textComponent.GetValue("Info5"));
        infoAboutCounterButton2.InRectangle(Camera)
            .OnCenter()
            .OnY(7, 10)
            .Centered()
            .Apply();
        AutoManaged.Add(infoAboutCounterButton2);


        var counterButtonAddon = new CounterButtonAddon(counterButton, 5);
        counterButtonAddon.Click += delegate { MoveToNextScreen(counterButtonAddon); };
        AutoManaged.Add(counterButtonAddon);

        #endregion // CounterButtonScreen

        screen.Y += Camera.Rectangle.Height;

        #region HoldButtonScreen

        var stateButton = new Button(textComponent.GetValue("ButtonFinish"));
        stateButton.InRectangle(Camera)
            .OnCenter()
            .Centered()
            .Apply();

        var holdButtonAddon = new HoldButtonAddon(stateButton, 3000F);
        holdButtonAddon.Click += Finish;
        AutoManaged.Add(holdButtonAddon);

        var infoAboutButton = new Text(textComponent.GetValue("Info6"));
        infoAboutButton.InRectangle(Camera)
            .OnCenter()
            .OnY(3, 10)
            .Centered()
            .Apply();
        AutoManaged.Add(infoAboutButton);

        var infoAboutButton2 = new Text(textComponent.GetValue("Info7"));
        infoAboutButton2.InRectangle(Camera)
            .OnCenter()
            .OnY(7, 10)
            .Centered()
            .Apply();
        AutoManaged.Add(infoAboutButton2);

        #endregion // HoldButtonScreen

        screen.Y += Camera.Rectangle.Height;
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