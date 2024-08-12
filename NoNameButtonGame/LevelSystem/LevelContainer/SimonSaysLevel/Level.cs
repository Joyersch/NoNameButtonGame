using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoUtils;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Logic.Text;
using MonoUtils.Settings;
using MonoUtils.Sound;
using MonoUtils.Ui;
using NoNameButtonGame.LevelSystem.Settings;
using NoNameButtonGame.Music;

namespace NoNameButtonGame.LevelSystem.LevelContainer.SimonSaysLevel;

public class Level : SampleLevel
{
    public Level(Scene scene, Random random, EffectsRegistry effectsRegistry,
        SettingsAndSaveManager<string> settingsAndSaveManager, float difficulty = 1F) : base(scene, random,
        effectsRegistry, settingsAndSaveManager)
    {
        var textComponent = TextProvider.GetText("Levels.SimonSaysLevel");
        Name = textComponent.GetValue("Name");

        Synthwave.Play();

        Camera.Move(Display.Size / 2);
        Cursor.InRectangle(Camera.Rectangle)
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

        var simon = new SimonSays(Camera.Rectangle, random, text, playLength, waitBetweenColors, buttonDisplaySpeed,
            effectsRegistry);
        simon.Finished += Finish;
        AutoManaged.Add(simon);
    }
}