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

        Camera.ZoomSpeed = 3000;

        var startButton = new Button(textComponent.GetValue("StartButton"));
        startButton.InRectangle(Camera.Rectangle)
            .OnX(0.125F)
            .OnY(0.15F)
            .Centered()
            .Move();

        var lockedStartButton = new LockButtonAddon(startButton);
        if (progress.MaxLevel < maxLevel)
            lockedStartButton.Unlock();
        lockedStartButton.Click += StartButtonPressed;

        AutoManaged.Add(lockedStartButton);

        var selectLevelButton = new Button(textComponent.GetValue("SelectButton"));
        selectLevelButton.GetAnchor(startButton)
            .SetMainAnchor(AnchorCalculator.Anchor.BottomLeft)
            .SetSubAnchor(AnchorCalculator.Anchor.TopLeft)
            .Move();
        selectLevelButton.Click += SelectButtonPressed;
        var selectLevelButtonLock = new LockButtonAddon(selectLevelButton);

        if (progress.MaxLevel > 0)
            selectLevelButtonLock.Unlock();

        AutoManaged.Add(selectLevelButtonLock);

        var endlessButton = new Button(textComponent.GetValue("EndlessButton"));
        endlessButton.Click += EndlessButtonPressed;
        endlessButton.GetAnchor(selectLevelButton)
            .SetMainAnchor(AnchorCalculator.Anchor.BottomLeft)
            .SetSubAnchor(AnchorCalculator.Anchor.TopLeft)
            .Move();
        var endlessLockButton = new LockButtonAddon(endlessButton);
        if (progress.MaxLevel >= maxLevel)
            endlessLockButton.Unlock();
        AutoManaged.Add(endlessLockButton);

        var settingsButton = new Button(textComponent.GetValue("SettingsButton"));
        settingsButton.Click += SettingsButtonPressed;
        settingsButton.GetAnchor(endlessButton)
            .SetMainAnchor(AnchorCalculator.Anchor.BottomLeft)
            .SetSubAnchor(AnchorCalculator.Anchor.TopLeft)
            .Move();
        AutoManaged.Add(settingsButton);

        var exitButton = new Button(textComponent.GetValue("ExitButton"));
        exitButton.Click += ExitButtonPressed;
        exitButton.GetAnchor(settingsButton)
            .SetMainAnchor(AnchorCalculator.Anchor.BottomLeft)
            .SetSubAnchor(AnchorCalculator.Anchor.TopLeft)
            .Move();
        AutoManaged.Add(exitButton);

        var header = new Text("NoNameButtonGame", Vector2.Zero, 10F, 1);
        header.InRectangle(Camera.Rectangle)
            .OnX(0.605F)
            .OnY(0.25F)
            .Centered()
            .Move();
        AutoManaged.Add(header);


        var assembly = Assembly.GetExecutingAssembly();
        var assemblyVersion = assembly.GetName().Version;
        var assemblyFileInfo = assembly.GetCustomAttributes<AssemblyFileVersionAttribute>()
            .FirstOrDefault()?.Version ?? string.Empty;
        string formatted =
            $"v{assemblyVersion!.Major}.{assemblyVersion!.Minor}.{assemblyVersion!.Build} {assemblyFileInfo}";
        var version = new Text(formatted, Vector2.Zero, 0.5F * Text.DefaultLetterScale);
        version.InRectangle(Camera.Rectangle)
            .OnX(0.905F)
            .OnY(0.315F)
            .Centered()
            .Move();
        AutoManaged.Add(version);

        var credits = new ClickableText(textComponent.GetValue("CreditsText"), 2F);
        credits.ChangeColor(ClickableText.LinkColor);
        credits.Click += CreditsLinkPressed;
        credits.GetAnchor(header)
            .SetMainAnchor(AnchorCalculator.Anchor.BottomLeft)
            .SetSubAnchor(AnchorCalculator.Anchor.TopLeft)
            .SetDistanceX(-8F)
            .SetDistanceY(-2F)
            .Move();
        AutoManaged.Add(credits);


        if (progress.FinishedLevels)
        {
            Rainbow color = new Rainbow
            {
                Increment = 5
            };
            var completion = new Text("[star]", 0.5F * Text.DefaultLetterScale);
            completion.InRectangle(Camera.Rectangle)
                .OnX(0.875F)
                .OnY(0.9F)
                .Centered()
                .Move();
            AutoManaged.Add(completion);
            AutoManaged.Add(color);
            ColorListener.Add(color, completion);
        }

        if (progress.FinishedSelect)
        {
            Rainbow color = new Rainbow
            {
                Offset = 80,
                Increment = 5
            };
            var completion = new Text("[star]", 0.5F * Text.DefaultLetterScale);
            completion.InRectangle(Camera.Rectangle)
                .OnX(0.9F)
                .OnY(0.9F)
                .Centered()
                .Move();
            AutoManaged.Add(completion);
            AutoManaged.Add(color);
            ColorListener.Add(color, completion);
        }

        if (progress.FinishedEndless)
        {
            Rainbow color = new Rainbow
            {
                Offset = 160,
                Increment = 5
            };
            var completion = new Text("[star]", 0.5F * Text.DefaultLetterScale);
            completion.InRectangle(Camera.Rectangle)
                .OnX(0.925F)
                .OnY(0.9F)
                .Centered()
                .Move();
            AutoManaged.Add(completion);
            AutoManaged.Add(color);
            ColorListener.Add(color, completion);
        }

        if (panIn)
        {
            Camera.InRectangle(Camera.Rectangle)
                .OnCenter()
                .ByGridY(1)
                .Move();

            Cursor.InRectangle(Camera.Rectangle)
                .OnCenter()
                .ByGridY(1)
                .Move();

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