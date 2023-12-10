using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoUtils;
using MonoUtils.Logic;
using MonoUtils.Logic.Text;
using MonoUtils.Ui;
using MonoUtils.Ui.Color;
using MonoUtils.Ui.Objects;
using MonoUtils.Ui.Objects.TextSystem;

namespace NoNameButtonGame.LevelSystem.FinishScreen;

public class Level : SampleLevel
{
    public Level(Display display, Vector2 window, Random random) : base(display, window, random)
    {
        var textComponent = TextProvider.GetText("Levels.FinishScreen");
        Name = textComponent.GetValue("Name");

        Text pressToContinueLabel = new Text(textComponent.GetValue("PressToContinue"));
        pressToContinueLabel.GetCalculator(Camera.Rectangle)
            .OnCenter()
            .OnY(0.9F)
            .Centered()
            .Move();

        AutoManaged.Add(pressToContinueLabel);

        Text levelFinishedLabel = new Text(textComponent.GetValue("LevelFinished"), 3F);
        levelFinishedLabel.GetCalculator(Camera.Rectangle)
            .OnCenter()
            .OnY(0.2F)
            .Centered()
            .Move();

        AutoManaged.Add(levelFinishedLabel);

        Rainbow rainbowColor = new Rainbow()
        {
            GameTimeStepInterval = 25F,
            Increment = 10,
            NoGradient = false
        };
        ColorListener.Add(rainbowColor, levelFinishedLabel);
        AutoManaged.Add(rainbowColor);

        var cursor = new Cursor();
        Actuator = cursor;
        PositionListener.Add(Mouse, cursor);
        AutoManaged.Add(cursor);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (InputReaderMouse.CheckKey(InputReaderMouse.MouseKeys.Left, true))
            Finish();
    }
}