using System;
using Joyersch.Monogame;
using Joyersch.Monogame.Sound;
using Joyersch.Monogame.Storage;
using Joyersch.Monogame.Ui.Color;
using Joyersch.Monogame.Ui.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
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

        BasicText pressToContinueLabel = new BasicText(textComponent.GetValue("PressToContinue"));
        AutoManaged.Add(pressToContinueLabel);
        DynamicScaler.Register(pressToContinueLabel);

        positionCalculator = pressToContinueLabel.InRectangle(Camera)
            .OnCenter()
            .OnY(0.9F)
            .Centered();
        CalculatorCollection.Register(positionCalculator);


        BasicText levelFinishedLabel = new BasicText(textComponent.GetValue("LevelFinished"), 3F * BasicText.DefaultLetterScale);
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