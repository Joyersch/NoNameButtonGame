using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoUtils;
using MonoUtils.Logic;
using MonoUtils.Logic.Text;
using MonoUtils.Ui;
using MonoUtils.Ui.Logic;
using MonoUtils.Ui.Objects;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level3;

public class Level : SampleLevel
{
    public Level(Display display, Vector2 window, Random random) : base(display, window, random)
    {
        var textComponent = TextProvider.GetText("Levels.Level3");
        Name = textComponent.GetValue("Name");

        var scale = 2F;
        Camera.Move(Display.Size / 2);

        Dictionary<string, string> text = new Dictionary<string, string>()
        {
            { "start", textComponent.GetValue("Start") },
            { "clickStart", textComponent.GetValue("clickStart") },
            { "playingSequence", textComponent.GetValue("playingSequence") },
        };

        var simon = new SimonSays(Camera.Rectangle, random, 3, text);
        simon.Finished += Finish;
        AutoManaged.Add(simon);
    }
}