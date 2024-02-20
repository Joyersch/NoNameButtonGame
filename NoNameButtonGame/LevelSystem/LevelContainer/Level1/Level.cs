using System;
using Microsoft.Xna.Framework;
using MonoUtils.Logic;
using MonoUtils.Logic.Text;
using MonoUtils.Ui;
using MonoUtils.Ui.Logic;
using MonoUtils.Ui.Objects;
using MonoUtils.Ui.Objects.Buttons;
using MonoUtils.Ui.Objects.Buttons.AddOn;
using MonoUtils.Ui.Objects.TextSystem;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level1;

internal class Level : SampleLevel
{
    private readonly LockButtonAddon _lockButtonAddon;
    private readonly OverTimeMover _mover;
    private readonly OverTimeInvoker _invoker;
    private readonly DelayedText _endText;

    private int _screen;
    private int _maxScreen;

    public Level(Display display, Vector2 window, Random random) : base(display, window, random)
    {
        TextComponent textComponent = TextProvider.GetText("Levels.Level1");
        Name = textComponent.GetValue("Name");

        var screen = Camera.Rectangle;

        #region StartScreen

        var startButton = new Button(textComponent.GetValue("Button1"));
        startButton.GetCalculator(screen)
            .OnCenter()
            .Centered()
            .Move();
        startButton.Click += MoveToNextScreen;
        AutoManaged.Add(startButton);

        var infoText = new Text(textComponent.GetValue("Info1"));
        infoText.GetCalculator(screen)
            .OnCenter()
            .OnY(3, 10)
            .Centered()
            .Move();
        AutoManaged.Add(infoText);

        #endregion // StartScreen

        screen.Y += Camera.Rectangle.Height;
        _maxScreen++;

        _mover = new OverTimeMover(Camera, screen.Location.ToVector2(), 600F, OverTimeMover.MoveMode.Sin);
        AutoManaged.Add(_mover);

        #region LockButtonScreen

        var magicButton = new Button(textComponent.GetValue("Button2"));
        magicButton.GetCalculator(screen)
            .OnCenter()
            .OnY(13, 16)
            .Centered()
            .Move();
        magicButton.Click += MagicButtonOnClick;
        AutoManaged.Add(magicButton);

        var lockButton = new Button(textComponent.GetValue("ButtonSkip"));
        lockButton.GetCalculator(screen)
            .OnCenter()
            .OnY(3, 16)
            .Centered()
            .Move();

        _lockButtonAddon = new LockButtonAddon(lockButton);
        _lockButtonAddon.Click += delegate { MoveToNextScreen(_lockButtonAddon); };
        AutoManaged.Add(_lockButtonAddon);


        var info1 = new Text(textComponent.GetValue("Info2"));
        info1.GetCalculator(screen)
            .OnCenter()
            .OnY(7, 20)
            .Centered()
            .Move();
        AutoManaged.Add(info1);

        var info2 = new Text(textComponent.GetValue("Info3"));
        info2.GetCalculator(screen)
            .OnCenter()
            .OnY(13, 20)
            .Centered()
            .Move();
        AutoManaged.Add(info2);

        #endregion // LockButtonScreen

        screen.Y += Camera.Rectangle.Height;
        _maxScreen++;

        #region CounterButtonScreen

        var counterButton = new Button(textComponent.GetValue("ButtonSkip"));
        counterButton.GetCalculator(screen)
            .OnCenter()
            .Centered()
            .Move();

        var infoAboutCounterButton = new Text(textComponent.GetValue("Info4"));
        infoAboutCounterButton.GetCalculator(screen)
            .OnCenter()
            .OnY(3, 10)
            .Centered()
            .Move();
        AutoManaged.Add(infoAboutCounterButton);

        var infoAboutCounterButton2 = new Text(textComponent.GetValue("Info5"));
        infoAboutCounterButton2.GetCalculator(screen)
            .OnCenter()
            .OnY(7, 10)
            .Centered()
            .Move();
        AutoManaged.Add(infoAboutCounterButton2);


        var counterButtonAddon = new CounterButtonAddon(counterButton, 5);
        counterButtonAddon.Click += delegate { MoveToNextScreen(counterButtonAddon); };
        AutoManaged.Add(counterButtonAddon);

        #endregion // CounterButtonScreen

        screen.Y += Camera.Rectangle.Height;
        _maxScreen++;

        #region HoldButtonScreen

        var stateButton = new Button(textComponent.GetValue("ButtonSkip"));
        stateButton.GetCalculator(screen)
            .OnCenter()
            .Centered()
            .Move();


        var holdButtonAddon = new HoldButtonAddon(stateButton, 3000F);
        holdButtonAddon.Click += delegate { MoveToNextScreen(holdButtonAddon); };
        AutoManaged.Add(holdButtonAddon);


        var infoAboutButton = new Text(textComponent.GetValue("Info6"));
        infoAboutButton.GetCalculator(screen)
            .OnCenter()
            .OnY(3, 10)
            .Centered()
            .Move();
        AutoManaged.Add(infoAboutButton);

        var infoAboutButton2 = new Text(textComponent.GetValue("Info7"));
        infoAboutButton2.GetCalculator(screen)
            .OnCenter()
            .OnY(7, 10)
            .Centered()
            .Move();
        AutoManaged.Add(infoAboutButton2);

        #endregion // HoldButtonScreen

        screen.Y += Camera.Rectangle.Height;
        _maxScreen++;

        #region FinishingText

        _endText = new DelayedText(textComponent.GetValue("Info8"), false)
        {
            DisplayDelay = 56,
            StartAfter = 100
        };
        _endText.GetCalculator(screen)
            .OnCenter()
            .Centered()
            .Move();
        AutoManaged.Add(_endText);

        _invoker = new OverTimeInvoker(1000D, false);
        _invoker.Trigger += Finish;
        AutoManaged.Add(_invoker);

        #endregion
    }

    private void MoveToNextScreen(object sender)
    {
        if (_mover.IsMoving)
            return;

        _mover.ChangeDestination(new Vector2(Camera.Position.X, Camera.Position.Y + Camera.Rectangle.Height));
        _mover.Start();
        _screen++;
    }

    private void MagicButtonOnClick(object obj)
    {
        if (_lockButtonAddon.IsLocked)
            _lockButtonAddon.Unlock();
        else
            _lockButtonAddon.Lock();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (_screen != _maxScreen)
            return;

        if (_mover.IsMoving)
            return;

        if (!_endText.IsPlaying && !_endText.HasPlayed)
            _endText.Start();

        if (!_endText.HasPlayed)
            return;

        _invoker.Start();
    }
}