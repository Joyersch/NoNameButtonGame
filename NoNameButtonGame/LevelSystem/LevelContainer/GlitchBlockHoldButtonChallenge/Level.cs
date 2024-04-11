using System;
using Microsoft.Xna.Framework;
using MonoUtils;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Logic.Text;
using MonoUtils.Sound;
using MonoUtils.Ui;
using MonoUtils.Ui.Objects.Buttons;
using MonoUtils.Ui.Objects.Buttons.AddOn;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.Glitch;

namespace NoNameButtonGame.LevelSystem.LevelContainer.GlitchBlockHoldButtonChallenge;

internal class Level : SampleLevel
{
    public Level(Display display, Vector2 window, Random random, EffectsRegistry effectsRegistry,
        float difficulty = 950) : base(display, window, random, effectsRegistry)
    {
        var textComponent = TextProvider.GetText("Levels.GlitchBlockHoldButtonChallenge");

        Name = textComponent.GetValue("Name");

        var cleanDifficulty = (difficulty + 100F) / 1050F;
        if (cleanDifficulty > 1F)
            cleanDifficulty = 1F;

        Log.WriteInformation(Camera.Rectangle.ToString());
        //var wall = new GlitchBlockCollection()
    }

    private void SetButton(Rectangle rectangle, IButton button, Random random)
    {
        float x = random.Next(rectangle.Width - button.Rectangle.Width / 2);
        float y = random.Next(rectangle.Height - button.Rectangle.Height / 2);
        var calculator = button.GetCalculator(rectangle)
            .OnX(x / rectangle.Width)
            .OnY(y / rectangle.Height);
        calculator.Move();
    }
}