using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoUtils;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Logic.Text;
using MonoUtils.Settings;
using MonoUtils.Sound;
using MonoUtils.Ui;
using MonoUtils.Ui.Color;
using MonoUtils.Ui.TextSystem;
using NoNameButtonGame.LevelSystem.Settings;
using NoNameButtonGame.Music;

namespace NoNameButtonGame.LevelSystem.FinishScreen;

public class Level : SampleLevel
{
    private bool _canExit;

    public Level(Scene scene, Random random, EffectsRegistry effectsRegistry,
        SettingsAndSaveManager<string> settingsAndSaveManager) : base(scene, random, effectsRegistry,
        settingsAndSaveManager)
    {
        var textComponent = TextProvider.GetText("Levels.FinishScreen");
        Name = textComponent.GetValue("Name");

        PositionCalculator positionCalculator = null;

        Text pressToContinueLabel = new Text(textComponent.GetValue("PressToContinue"));
        AutoManaged.Add(pressToContinueLabel);
        DynamicScaler.Register(pressToContinueLabel);

        positionCalculator = pressToContinueLabel.InRectangle(Camera)
            .OnCenter()
            .OnY(0.9F)
            .Centered();
        CalculatorCollection.Register(positionCalculator);


        Text levelFinishedLabel = new Text(textComponent.GetValue("LevelFinished"), 3F * Text.DefaultLetterScale);
        AutoManaged.Add(levelFinishedLabel);
        DynamicScaler.Register(levelFinishedLabel);

        positionCalculator = levelFinishedLabel.InRectangle(Camera)
            .OnCenter()
            .OnY(0.2F)
            .Centered();
        CalculatorCollection.Register(positionCalculator);

        Rainbow rainbowColor = new Rainbow()
        {
            GameTimeStepInterval = 25F,
            Increment = 10,
            NoGradient = false
        };
        ColorListener.Add(rainbowColor, levelFinishedLabel);
        AutoManaged.Add(rainbowColor);

        DynamicScaler.Apply(Display.Scale);
        CalculatorCollection.Apply();
    }

    public override void Update(GameTime gameTime)
    {
        Camera.Move(Vector2.Zero);
        base.Update(gameTime);
        Default2.Play();

        if (!_canExit)
            _canExit = Microsoft.Xna.Framework.Input.Mouse.GetState().LeftButton == ButtonState.Released;
        else if (Microsoft.Xna.Framework.Input.Mouse.GetState().LeftButton == ButtonState.Pressed)
        {
            _canExit = false;
            Finish();
        }
    }
}