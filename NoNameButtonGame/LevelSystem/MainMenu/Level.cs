using System;
using Joyersch.Monogame;
using Joyersch.Monogame.Sound;
using Joyersch.Monogame.Storage;
using Joyersch.Monogame.Ui;
using Joyersch.Monogame.Ui.Buttons.AddOn;
using Joyersch.Monogame.Ui.Color;
using Joyersch.Monogame.Ui.Text;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.Music;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace NoNameButtonGame.LevelSystem.MainMenu;

public class Level : SampleLevel
{
    public event Action<object> StartClicked;
    public event Action<object> SelectClicked;
    public event Action<object> SettingsClicked;
    public event Action<object> EndlessClicked;
    public event Action<object> CreditsClicked;

    public Level(Scene scene, Random rand, Progress progress, EffectsRegistry effectsRegistry, int maxLevel,
        bool panIn, SettingsAndSaveManager<string> settingsAndSaveManager) : base(scene, rand, effectsRegistry,
        settingsAndSaveManager)
    {
        var textComponent = TextProvider.GetText("Levels.MainMenu");
        Name = textComponent.GetValue("Name");

        Default.Play();

        AnchorCalculator anchorCalculator = null;
        PositionCalculator positionCalculator = null;

        Camera.ZoomSpeed = 3000;

        var startButton = new Button(textComponent.GetValue("StartButton"));
        var lockedStartButton = new LockButtonAddon(startButton, 3f);
        if (progress.MaxLevel < maxLevel)
            lockedStartButton.Unlock();
        lockedStartButton.Click += StartButtonPressed;
        AutoManaged.Add(lockedStartButton);
        AutoScale.Add(lockedStartButton);

        positionCalculator = lockedStartButton.InRectangle(Camera)
            .OnX(0.125F)
            .OnY(0.15F)
            .Centered();
        CalculatorCollection.Register(positionCalculator);

        var selectLevelButton = new Button(textComponent.GetValue("SelectButton"));
        selectLevelButton.Click += SelectButtonPressed;
        var selectLevelButtonLock = new LockButtonAddon(selectLevelButton, 3f);
        AutoManaged.Add(selectLevelButtonLock);
        AutoScale.Add(selectLevelButtonLock);
        if (progress.MaxLevel > 0)
            selectLevelButtonLock.Unlock();

        anchorCalculator = selectLevelButtonLock.GetAnchor(startButton)
            .SetMainAnchor(AnchorCalculator.Anchor.BottomLeft)
            .SetSubAnchor(AnchorCalculator.Anchor.TopLeft);
        CalculatorCollection.Register(anchorCalculator);
        
        var endlessButton = new Button(textComponent.GetValue("EndlessButton"));
        endlessButton.Click += EndlessButtonPressed;
        var endlessLockButton = new LockButtonAddon(endlessButton, 3f);
        if (progress.MaxLevel >= maxLevel)
            endlessLockButton.Unlock();
        AutoManaged.Add(endlessLockButton);
        AutoScale.Add(endlessLockButton);

        anchorCalculator = endlessLockButton.GetAnchor(selectLevelButton)
            .SetMainAnchor(AnchorCalculator.Anchor.BottomLeft)
            .SetSubAnchor(AnchorCalculator.Anchor.TopLeft);
        CalculatorCollection.Register(anchorCalculator);

        var settingsButton = new Button(textComponent.GetValue("SettingsButton"));
        settingsButton.Click += SettingsButtonPressed;
        AutoManaged.Add(settingsButton);
        AutoScale.Add(settingsButton);

        anchorCalculator = settingsButton.GetAnchor(endlessButton)
            .SetMainAnchor(AnchorCalculator.Anchor.BottomLeft)
            .SetSubAnchor(AnchorCalculator.Anchor.TopLeft);
        CalculatorCollection.Register(anchorCalculator);

        var exitButton = new Button(textComponent.GetValue("ExitButton"));
        exitButton.Click += ExitButtonPressed;
        AutoManaged.Add(exitButton);
        AutoScale.Add(exitButton);

        anchorCalculator = exitButton.GetAnchor(settingsButton)
            .SetMainAnchor(AnchorCalculator.Anchor.BottomLeft)
            .SetSubAnchor(AnchorCalculator.Anchor.TopLeft);
        CalculatorCollection.Register(anchorCalculator);

        var header = new BasicText("NoNameButtonGame", Vector2.Zero, 10F, 1);
        AutoManaged.Add(header);
        AutoScale.Add(header);

        positionCalculator = header.InRectangle(Camera)
            .OnX(0.605F)
            .OnY(0.25F)
            .Centered();
        CalculatorCollection.Register(positionCalculator);

        var version = new BasicText(Statics.Version.ToString(), Vector2.Zero, 2f);
        AutoManaged.Add(version);
        AutoScale.Add(version);

        positionCalculator = version.InRectangle(Camera)
            .OnX(0.905F)
            .OnY(0.315F)
            .Centered();
        CalculatorCollection.Register(positionCalculator);

        var credits = new ClickableText(textComponent.GetValue("CreditsText"), 2f);
        credits.ChangeColor(ClickableText.LinkColor);
        credits.Click += CreditsLinkPressed;
        AutoManaged.Add(credits);
        AutoScale.Add(credits);

        anchorCalculator = credits.GetAnchor(header)
            .SetMainAnchor(AnchorCalculator.Anchor.BottomLeft)
            .SetSubAnchor(AnchorCalculator.Anchor.TopLeft)
            .SetDistanceX(-8F)
            .SetDistanceY(-2F)
            .SetDistanceScale(Display);
        CalculatorCollection.Register(anchorCalculator);

        if (progress.FinishedLevels)
        {
            Rainbow color = new Rainbow
            {
                Increment = 5
            };
            AutoManaged.Add(color);

            var completion = new BasicText("[star]", 2f);
            AutoManaged.Add(completion);
            AutoScale.Add(completion);
            ColorListener.Add(color, completion);

            positionCalculator = completion.InRectangle(Camera)
                .OnX(0.875F)
                .OnY(0.9F)
                .Centered();
            CalculatorCollection.Register(positionCalculator);
        }

        if (progress.FinishedSelect)
        {
            Rainbow color = new Rainbow
            {
                Offset = 80,
                Increment = 5
            };
            AutoManaged.Add(color);

            var completion = new BasicText("[star]", 2f);
            AutoManaged.Add(completion);
            AutoScale.Add(completion);
            ColorListener.Add(color, completion);

            positionCalculator = completion.InRectangle(Camera)
                .OnX(0.9F)
                .OnY(0.9F)
                .Centered();
            CalculatorCollection.Register(positionCalculator);
        }

        if (progress.FinishedEndless)
        {
            Rainbow color = new Rainbow
            {
                Offset = 160,
                Increment = 5
            };
            AutoManaged.Add(color);

            var completion = new BasicText("[star]", 2f);
            AutoScale.Add(completion);
            AutoManaged.Add(completion);
            ColorListener.Add(color, completion);

            positionCalculator = completion.InRectangle(Camera)
                .OnX(0.925F)
                .OnY(0.9F)
                .Centered();
            CalculatorCollection.Register(positionCalculator);
        }

        SetScaleAndCalculatePositions();

        if (!panIn)
            return;
        
        Camera.InRectangle(Camera)
            .OnCenter()
            .ByGridY(1)
            .Apply();

        Cursor.InRectangle(Camera)
            .OnCenter()
            .ByGridY(1)
            .Apply();

        var mover = new OverTimeMover(Camera, Vector2.Zero, 666F, OverTimeMover.MoveMode.Sin);
        mover.Start();
        AutoManaged.Add(mover);
    }

    private void StartButtonPressed(object sender)
        => StartClicked?.Invoke(sender);

    private void SelectButtonPressed(object sender)
        => SelectClicked?.Invoke(sender);

    private void SettingsButtonPressed(object sender)
        => SettingsClicked?.Invoke(sender);

    private void CreditsLinkPressed(object sender)
        => CreditsClicked?.Invoke(sender);

    private void EndlessButtonPressed(object sender)
        => EndlessClicked?.Invoke(sender);

    private void ExitButtonPressed(object sender)
        => Exit(sender);
}