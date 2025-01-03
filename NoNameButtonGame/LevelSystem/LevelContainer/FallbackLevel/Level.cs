﻿using System;
using Microsoft.Xna.Framework;
using MonoUtils;
using MonoUtils.Logic;
using MonoUtils.Logic.Text;
using MonoUtils.Settings;
using MonoUtils.Sound;
using MonoUtils.Ui;
using MonoUtils.Ui.TextSystem;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.LevelSystem.Settings;
using NoNameButtonGame.Music;

namespace NoNameButtonGame.LevelSystem.LevelContainer.FallbackLevel;

internal class Level : SampleLevel
{
    public Level(Scene scene, Random random, EffectsRegistry effectsRegistry,
        SettingsAndSaveManager<string> settingsAndSaveManager) : base(scene, random, effectsRegistry,
        settingsAndSaveManager)
    {
        var textComponent = TextProvider.GetText("Levels.FallbackLevel");

        Name = textComponent.GetValue("Name");

        None.Play();

        var failButton = new Button(textComponent.GetValue("Button"));
        failButton.InRectangle(Camera)
            .OnCenter()
            .Centered()
            .Apply();
        failButton.Click += Fail;
        AutoManaged.Add(failButton);


        var info = new Text(textComponent.GetValue("Text"));
        info.InRectangle(Camera)
            .OnCenter()
            .OnY(3, 10)
            .Centered()
            .Apply();
        AutoManaged.Add(info);
    }
}