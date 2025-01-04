using System;
using Joyersch.Monogame;
using Joyersch.Monogame.Sound;
using Joyersch.Monogame.Storage;
using Joyersch.Monogame.Ui.Text;
using NoNameButtonGame.GameObjects.Buttons;
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


        var info = new BasicText(textComponent.GetValue("Text"));
        info.InRectangle(Camera)
            .OnCenter()
            .OnY(3, 10)
            .Centered()
            .Apply();
        AutoManaged.Add(info);
    }
}