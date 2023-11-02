using System;
using Microsoft.Xna.Framework;
using MonoUtils.Logic;
using MonoUtils.Logic.Text;
using MonoUtils.Ui;
using MonoUtils.Ui.Objects;
using MonoUtils.Ui.Objects.Buttons;
using MonoUtils.Ui.Objects.TextSystem;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level0;

internal class Level : SampleLevel
{
    public Level(Display display, Vector2 window, Random random) : base(display, window, random)
    {
        Name = "Level 404";


        var textComponent = TextProvider.GetText("Levels.Level0");

        var failButton = new TextButton(textComponent.GetValue("Button"));
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
        
        var cursor = new Cursor();
        Actuator = cursor;
        PositionListener.Add(Mouse, cursor);
        AutoManaged.Add(cursor);
    }
}