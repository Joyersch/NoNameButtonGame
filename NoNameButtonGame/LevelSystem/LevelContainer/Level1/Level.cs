using System;
using Microsoft.Xna.Framework;
using MonoUtils.Logic;
using MonoUtils.Ui;
using MonoUtils.Ui.Objects;
using MonoUtils.Ui.Objects.Buttons;
using MonoUtils.Ui.Objects.Buttons.AddOn;
using MonoUtils.Ui.Objects.TextSystem;

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
        Name = "Level 1 - Click the Button! (Tutorial)";

        var screen = Camera.Rectangle;
        
        #region StartScreen
        
        var startButton = new TextButton("Start");
        startButton.GetCalculator(screen)
            .OnCenter()
            .Centered()
            .Move();
        startButton.Click += MoveToNextScreen;
        AutoManaged.Add(startButton);
        
        var infoText = new Text("How hard can it be?");
        infoText.GetCalculator(screen)
            .OnCenter()
            .OnY(3,10)
            .Centered()
            .Move();
        AutoManaged.Add(infoText);
        
        #endregion // StartScreen

        screen.Y += Camera.Rectangle.Height;
        _maxScreen++;
        
        _mover = new OverTimeMover(Camera, screen.Location.ToVector2(), 600F, OverTimeMover.MoveMode.Sin);
        AutoManaged.Add(_mover);
        
        #region LockButtonScreen

        var magicButton = new TextButton("Unlock");
        magicButton.GetCalculator(screen)
            .OnCenter()
            .OnY(13,16)
            .Centered()
            .Move();
        magicButton.Click += MagicButtonOnClick;
        AutoManaged.Add(magicButton);

        var lockButton = new TextButton("Next");
        lockButton.GetCalculator(screen)
            .OnCenter()
            .OnY(3,16)
            .Centered()
            .Move();
        
        _lockButtonAddon = new LockButtonAddon(new(lockButton));
        _lockButtonAddon.Callback += (_, state) =>
        {
            if (state != IButtonAddon.CallState.Click)
                return;
            MoveToNextScreen(_lockButtonAddon);
        };
        AutoManaged.Add(_lockButtonAddon);
        
        var info1 = new Text("This button here is locked!");
        info1.GetCalculator(screen)
            .OnCenter()
            .OnY(7,20)
            .Centered()
            .Move();
        AutoManaged.Add(info1);
        
        var info2 = new Text("The button below will unlock the button above!");
        info2.GetCalculator(screen)
            .OnCenter()
            .OnY(13,20)
            .Centered()
            .Move();
        AutoManaged.Add(info2);

        #endregion // LockButtonScreen

        screen.Y += Camera.Rectangle.Height;
        _maxScreen++;
        
        #region CounterButtonScreen

        var counterButton = new TextButton("Next");
        counterButton.GetCalculator(screen)
            .OnCenter()
            .Centered()
            .Move();
        
        var infoAboutCounterButton = new Text("This button has a counter");
        infoAboutCounterButton.GetCalculator(screen)
            .OnCenter()
            .OnY(3, 10)
            .Centered()
            .Move();
        AutoManaged.Add(infoAboutCounterButton);
        
        var infoAboutCounterButton2 = new Text("Press the button to lower the counter and when it hits 0 you win!");
        infoAboutCounterButton2.GetCalculator(screen)
            .OnCenter()
            .OnY(7, 10)
            .Centered()
            .Move();
        AutoManaged.Add(infoAboutCounterButton2);
        
        var counterButtonAddon = new CounterButtonAddon(new(counterButton), 5);
        counterButtonAddon.Callback += (_, state) =>
        {
            if (state != IButtonAddon.CallState.Click)
                return;
            MoveToNextScreen(counterButtonAddon);
        };
        AutoManaged.Add(counterButtonAddon);

        #endregion // CounterButtonScreen

        screen.Y += Camera.Rectangle.Height;
        _maxScreen++;

        #region HoldButtonScreen

        var stateButton = new TextButton("Next");
        stateButton.GetCalculator(screen)
            .OnCenter()
            .Centered()
            .Move();

        var holdButtonAddon = new HoldButtonAddon(new(stateButton), 3000F);
        holdButtonAddon.Callback += (_, state) =>
        {
            if (state != IButtonAddon.CallState.Click)
                return;
            MoveToNextScreen(holdButtonAddon);
        };
        AutoManaged.Add(holdButtonAddon);
        
        var infoAboutButton = new Text("This button has a timer");
        infoAboutButton.GetCalculator(screen)
            .OnCenter()
            .OnY(3, 10)
            .Centered()
            .Move();
        AutoManaged.Add(infoAboutButton);
        
        var infoAboutButton2 = new Text("Press the button to lower the timer and when it hits 0 you win!");
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

        _endText = new DelayedText("Now that you know the basics. Lets actually start!", false)
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

        var mouseCursor = new Cursor();
        Actuator = mouseCursor;
        AutoManaged.Add(mouseCursor);
        PositionListener.Add(Mouse, mouseCursor);
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