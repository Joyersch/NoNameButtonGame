using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using Microsoft.Xna.Framework;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Logic.Text;
using MonoUtils.Ui;
using MonoUtils.Ui.Objects;
using MonoUtils.Ui.Objects.Buttons;
using MonoUtils.Ui.Objects.TextSystem;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace NoNameButtonGame.LevelSystem.MainMenu;

public class Level : SampleLevel
{
    public event Action<object> StartClicked;
    public event Action<object> SelectClicked;
    public event Action<object> SettingsClicked;
    public event Action<object> CreditsClicked;
    public event Action<object> BonusClicked;

    private Cursor mouseCursor;

    private float _tilt = 0;
    private bool _leftTilt;

    public Level(Display display, Vector2 window, Random rand) : base(display, window, rand)
    {
        var textComponent = TextProvider.GetText("Levels.MainMenu");
        Name = textComponent.GetValue("Name");

        var startButton = new Button(textComponent.GetValue("StartButton"));
        startButton.GetCalculator(Camera.Rectangle)
            .OnX(0.125F)
            .OnY(0.15F)
            .Centered()
            .Move();
        startButton.Click += StartButtonPressed;
        AutoManaged.Add(startButton);

        var selectLevelButton = new Button(textComponent.GetValue("SelectButton"));
        selectLevelButton.GetAnchor(startButton)
            .SetMainAnchor(AnchorCalculator.Anchor.BottomLeft)
            .SetSubAnchor(AnchorCalculator.Anchor.TopLeft)
            .Move();
        selectLevelButton.Click += SelectButtonPressed;
        AutoManaged.Add(selectLevelButton);

        var settingsButton = new Button(textComponent.GetValue("SettingsButton"));
        settingsButton.Click += SettingsButtonPressed;
        settingsButton.GetAnchor(selectLevelButton)
            .SetMainAnchor(AnchorCalculator.Anchor.BottomLeft)
            .SetSubAnchor(AnchorCalculator.Anchor.TopLeft)
            .Move();
        AutoManaged.Add(settingsButton);

        var creditButton = new Button(textComponent.GetValue("CreditsButton"));
        creditButton.Click += CreditButtonPressed;
        creditButton.GetAnchor(settingsButton)
            .SetMainAnchor(AnchorCalculator.Anchor.BottomLeft)
            .SetSubAnchor(AnchorCalculator.Anchor.TopLeft)
            .Move();
        AutoManaged.Add(creditButton);

        var exitButton = new Button(textComponent.GetValue("ExitButton"));
        exitButton.Click += ExitButtonPressed;
        exitButton.GetAnchor(creditButton)
            .SetMainAnchor(AnchorCalculator.Anchor.BottomLeft)
            .SetSubAnchor(AnchorCalculator.Anchor.TopLeft)
            .Move();
        AutoManaged.Add(exitButton);

        var header = new Text("NoNameButtonGame", Vector2.Zero, 5F, 1);
        header.GetCalculator(Camera.Rectangle)
            .OnX(0.605F)
            .OnY(0.25F)
            .Centered()
            .Move();
        AutoManaged.Add(header);

        var assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;
        var version = new Text($"v{assemblyVersion.Major}.{assemblyVersion.Minor}.{assemblyVersion.Revision}",
            Vector2.Zero, 0.5F);
        version.GetCalculator(Camera.Rectangle)
            .OnX(0.905F)
            .OnY(0.315F)
            .Centered()
            .Move();
        AutoManaged.Add(version);
    }

    private void StartButtonPressed(object sender)
        => StartClicked?.Invoke(sender);

    private void SelectButtonPressed(object sender)
        => SelectClicked?.Invoke(sender);

    private void SettingsButtonPressed(object sender)
        => SettingsClicked?.Invoke(sender);

    private void CreditButtonPressed(object sender)
        => CreditsClicked?.Invoke(sender);

    private void ExitButtonPressed(object sender)
        => Exit(sender);
}