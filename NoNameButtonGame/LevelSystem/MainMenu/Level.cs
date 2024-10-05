using System;
using System.Linq;
using System.Reflection;
using MonoUtils.Logic;
using MonoUtils.Logic.Text;
using MonoUtils.Settings;
using MonoUtils.Sound;
using MonoUtils.Ui;
using MonoUtils.Ui.Logic;
using MonoUtils.Ui.Buttons.AddOn;
using MonoUtils.Ui.Color;
using MonoUtils.Ui.TextSystem;
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

    public Level(Scene display, Random rand, Progress progress, EffectsRegistry effectsRegistry, int maxLevel,
        bool panIn, SettingsAndSaveManager<string> settingsAndSaveManager) : base(display, rand, effectsRegistry,
        settingsAndSaveManager)
    {
        var textComponent = TextProvider.GetText("Levels.MainMenu");
        Name = textComponent.GetValue("Name");

        Default.Play();

        AnchorCalculator anchorCalculator = null;
        PositionCalculator positionCalculator = null;

        Camera.ZoomSpeed = 3000;

        var startButton = new Button(textComponent.GetValue("StartButton"));
        var lockedStartButton = new LockButtonAddon(startButton);
        if (progress.MaxLevel < maxLevel)
            lockedStartButton.Unlock();
        lockedStartButton.Click += StartButtonPressed;
        DynamicScaler.Register(lockedStartButton);
        AutoManaged.Add(lockedStartButton);

        positionCalculator = lockedStartButton.InRectangle(Camera)
            .OnX(0.125F)
            .OnY(0.15F)
            .Centered();
        CalculatorCollection.Register(positionCalculator);

        var selectLevelButton = new Button(textComponent.GetValue("SelectButton"));

        selectLevelButton.Click += SelectButtonPressed;
        var selectLevelButtonLock = new LockButtonAddon(selectLevelButton);
        DynamicScaler.Register(selectLevelButtonLock);
        AutoManaged.Add(selectLevelButtonLock);
        if (progress.MaxLevel > 0)
            selectLevelButtonLock.Unlock();

        anchorCalculator = selectLevelButtonLock.GetAnchor(startButton)
            .SetMainAnchor(AnchorCalculator.Anchor.BottomLeft)
            .SetSubAnchor(AnchorCalculator.Anchor.TopLeft);
        CalculatorCollection.Register(anchorCalculator);


        var endlessButton = new Button(textComponent.GetValue("EndlessButton"));
        endlessButton.Click += EndlessButtonPressed;
        var endlessLockButton = new LockButtonAddon(endlessButton);
        if (progress.MaxLevel >= maxLevel)
            endlessLockButton.Unlock();
        AutoManaged.Add(endlessLockButton);
        DynamicScaler.Register(endlessLockButton);

        anchorCalculator = endlessLockButton.GetAnchor(selectLevelButton)
            .SetMainAnchor(AnchorCalculator.Anchor.BottomLeft)
            .SetSubAnchor(AnchorCalculator.Anchor.TopLeft);
        CalculatorCollection.Register(anchorCalculator);

        var settingsButton = new Button(textComponent.GetValue("SettingsButton"));
        settingsButton.Click += SettingsButtonPressed;
        AutoManaged.Add(settingsButton);
        DynamicScaler.Register(settingsButton);

        anchorCalculator = settingsButton.GetAnchor(endlessButton)
            .SetMainAnchor(AnchorCalculator.Anchor.BottomLeft)
            .SetSubAnchor(AnchorCalculator.Anchor.TopLeft);
        CalculatorCollection.Register(anchorCalculator);

        var exitButton = new Button(textComponent.GetValue("ExitButton"));
        exitButton.Click += ExitButtonPressed;
        AutoManaged.Add(exitButton);
        DynamicScaler.Register(exitButton);

        anchorCalculator = exitButton.GetAnchor(settingsButton)
            .SetMainAnchor(AnchorCalculator.Anchor.BottomLeft)
            .SetSubAnchor(AnchorCalculator.Anchor.TopLeft);
        CalculatorCollection.Register(anchorCalculator);

        var header = new Text("NoNameButtonGame", Vector2.Zero, 10F, 1);
        AutoManaged.Add(header);
        DynamicScaler.Register(header);

        positionCalculator = header.InRectangle(Camera)
            .OnX(0.605F)
            .OnY(0.25F)
            .Centered();
        CalculatorCollection.Register(positionCalculator);

        var version = new Text(Statics.Version.ToString(), Vector2.Zero, 0.5F * Text.DefaultLetterScale);
        AutoManaged.Add(version);
        DynamicScaler.Register(version);

        positionCalculator = version.InRectangle(Camera)
            .OnX(0.905F)
            .OnY(0.315F)
            .Centered();
        CalculatorCollection.Register(positionCalculator);

        var credits = new ClickableText(textComponent.GetValue("CreditsText"), 2F);
        credits.ChangeColor(ClickableText.LinkColor);
        credits.Click += CreditsLinkPressed;
        AutoManaged.Add(credits);
        DynamicScaler.Register(credits);

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

            var completion = new Text("[star]", 0.5F * Text.DefaultLetterScale);
            AutoManaged.Add(completion);
            ColorListener.Add(color, completion);
            DynamicScaler.Register(completion);

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

            var completion = new Text("[star]", 0.5F * Text.DefaultLetterScale);
            AutoManaged.Add(completion);
            DynamicScaler.Register(completion);
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

            var completion = new Text("[star]", 0.5F * Text.DefaultLetterScale);
            AutoManaged.Add(completion);
            DynamicScaler.Register(completion);
            ColorListener.Add(color, completion);

            positionCalculator = completion.InRectangle(Camera)
                .OnX(0.925F)
                .OnY(0.9F)
                .Centered();
            CalculatorCollection.Register(positionCalculator);
        }

        DynamicScaler.Apply(Display.Scale);
        CalculatorCollection.Apply();

        if (panIn)
        {
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