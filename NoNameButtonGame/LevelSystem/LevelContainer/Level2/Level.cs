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

        shuffler.Shuffle();

        int selectedColor = random.Next(0, 16);
        bool useText = random.Next() % 2 == 0;
        int usedText = shuffler.ResolveText(selectedColor);
        int usedColor = shuffler.ResolveColor(selectedColor);

        string infoText = textComponent.GetValue("Info");
        string t = textComponent.GetValue("Text");
        string c = textComponent.GetValue("Color");

        string infoMessage = string.Format(infoText, useText ? t : c, shuffler.GetText(usedText));

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
                if (index == (useText ? usedText : usedColor))
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