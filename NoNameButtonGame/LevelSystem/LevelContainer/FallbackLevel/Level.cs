using System;
using Microsoft.Xna.Framework;
using MonoUtils.Logic;
using MonoUtils.Logic.Text;
using MonoUtils.Sound;
using MonoUtils.Ui;
using MonoUtils.Ui.Objects.TextSystem;
using NoNameButtonGame.GameObjects.Buttons;

namespace NoNameButtonGame.LevelSystem.LevelContainer.FallbackLevel;

internal class Level : SampleLevel
{
    public Level(Display display, Vector2 window, Random random, EffectsRegistry effectsRegistry) : base(display, window, random, effectsRegistry)
    {
        var textComponent = TextProvider.GetText("Levels.FallbackLevel");

        Name = textComponent.GetValue("Name");

        var failButton = new Button(textComponent.GetValue("Button"));
        failButton.GetCalculator(Camera.Rectangle)
            .OnCenter()
            .Centered()
            .Move();
        failButton.Click += Fail;
        AutoManaged.Add(failButton);


        var info = new Text(textComponent.GetValue("Text"));
        info.GetCalculator(Camera.Rectangle)
            .OnCenter()
            .OnY(3,10)
            .Centered()
            .Move();
        AutoManaged.Add(info);
    }
}