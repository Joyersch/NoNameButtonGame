using System;
using Microsoft.Xna.Framework;
using MonoUtils;
using MonoUtils.Logic;
using MonoUtils.Logic.Text;
using MonoUtils.Sound;
using MonoUtils.Ui;
using MonoUtils.Ui.Color;
using MonoUtils.Ui.Objects.TextSystem;
using NoNameButtonGame.Music;

namespace NoNameButtonGame.LevelSystem.FinishScreen;

public class Level : SampleLevel
{
    public Level(Display display, Vector2 window, Random random, EffectsRegistry effectsRegistry) : base(display, window, random, effectsRegistry)
    {
        var textComponent = TextProvider.GetText("Levels.FinishScreen");
        Name = textComponent.GetValue("Name");

        Text pressToContinueLabel = new Text(textComponent.GetValue("PressToContinue"));
        pressToContinueLabel.InRectangle(Camera.Rectangle)
            .OnCenter()
            .OnY(0.9F)
            .Centered()
            .Move();

        AutoManaged.Add(pressToContinueLabel);

        Text levelFinishedLabel = new Text(textComponent.GetValue("LevelFinished"), 3F);
        levelFinishedLabel.InRectangle(Camera.Rectangle)
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

        // if nothing is pressed, after 5 seconds, level finishes
        // this is here as currently, something input does not work for some reason.
        var failsave = new Timer(5000, true);
        failsave.Trigger += Finish;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        Default2.Play();

        if (InputReaderMouse.CheckKey(InputReaderMouse.MouseKeys.Left))
            Finish();
    }
}