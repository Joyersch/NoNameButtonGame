using System;
using Microsoft.Xna.Framework;
using MonoUtils.Logic;
using MonoUtils.Logic.Text;
using MonoUtils.Ui;
using MonoUtils.Ui.Objects.Buttons;
using MonoUtils.Ui.Objects.TextSystem;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level10;

internal class Level : SampleLevel
{
    public Level(Display display, Vector2 window, Random random) : base(display, window, random)
    {
        var textComponent = TextProvider.GetText("Levels.Level10");

        Name = textComponent.GetValue("Name");

        var button = new Button("Finish");
        button.GetCalculator(Camera.Rectangle)
            .OnCenter()
            .Centered()
            .Move();
        button.Click += Finish;
        AutoManaged.Add(button);
    }
}