using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Logic.Text;
using MonoUtils.Sound;
using MonoUtils.Ui;

namespace NoNameButtonGame.LevelSystem.LevelContainer.SimonSaysLevel;

public class Level : SampleLevel
{
    public Level(Display display, Vector2 window, Random random, EffectsRegistry effectsRegistry,
        float difficulty = 1F) : base(display, window, random, effectsRegistry)
    {
        var textComponent = TextProvider.GetText("Levels.SimonSaysLevel");
        Name = textComponent.GetValue("Name");

        Camera.Move(Display.Size / 2);
        Cursor.GetCalculator(Camera.Rectangle)
            .OnCenter()
            .Centered()
            .Move();

        var cleanDifficulty = (difficulty + 100F) / 1050F;
        if (cleanDifficulty > 1F)
            cleanDifficulty = 1F;

        var flippedDifficulty = 3F - 3 * cleanDifficulty;

        Dictionary<string, string> text = new Dictionary<string, string>()
        {
            { "start", textComponent.GetValue("Start") },
            { "clickStart", textComponent.GetValue("clickStart") },
            { "playingSequence", textComponent.GetValue("playingSequence") },
        };

        int playLength = 4 + (int)Math.Ceiling(10 * cleanDifficulty);
        float waitBetweenColors = 125 + 125 * flippedDifficulty;
        float buttonDisplaySpeed = 150 + 150 * flippedDifficulty;
        // Log.WriteInformation(playLength.ToString());

        var simon = new SimonSays(Camera.Rectangle, random, text, playLength, waitBetweenColors, buttonDisplaySpeed);
        simon.Finished += Finish;
        AutoManaged.Add(simon);
    }
}