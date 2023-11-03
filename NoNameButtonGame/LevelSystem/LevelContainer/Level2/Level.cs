using System;
using Microsoft.Xna.Framework;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Logic.Text;
using MonoUtils.Ui;
using MonoUtils.Ui.Color;
using MonoUtils.Ui.Objects;
using MonoUtils.Ui.Objects.Buttons;
using MonoUtils.Ui.Objects.TextSystem;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level2;

public class Level : SampleLevel
{
    public Level(Display display, Vector2 window, Random random) : base(display, window, random)
    {
        var textComponent = TextProvider.GetText("Levels.Level2");

        Name = textComponent.GetValue("Name");


        ColorComponent[] colors =
            Newtonsoft.Json.JsonConvert.DeserializeObject<ColorComponent[]>(textComponent.GetValue("Colors"));
        ColorComponentShuffler shuffler = new(colors, random);

        int usedColor = random.Next(0, 16);
        bool useText = random.Next() % 2 == 0;

        string infoText = textComponent.GetValue("Info");
        string infoMessage = string.Format(infoText, useText ? "text" : "color", shuffler.GetText(usedColor));

        //shuffler.Shuffle();

        var info = new Text(infoMessage);
        info.GetCalculator(Camera.Rectangle)
            .OnCenter()
            .OnY(2, 20)
            .Centered()
            .Move();
        ;
        AutoManaged.Add(info);

        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                int index = x + y * 4;
                string text = shuffler.GetText(index);
                var button = new TextButton(text);
                button.Text.ChangeColor(shuffler.GetColor(index));
                button.GetCalculator(Camera.Rectangle)
                    .OnX(x * 4 + 4, 20)
                    .OnY(y * 4 + 5, 20)
                    .Centered()
                    .Move();
                if (index == usedColor)
                    button.Click += Finish;
                else
                    button.Click += Fail;
                AutoManaged.Add(button);
            }
        }


        var cursor = new Cursor();
        Actuator = cursor;
        PositionListener.Add(Mouse, cursor);
        AutoManaged.Add(cursor);
    }
}