using System;
using Microsoft.Xna.Framework;
using MonoUtils.Logic;
using MonoUtils.Logic.Text;
using MonoUtils.Settings;
using MonoUtils.Sound;
using MonoUtils.Ui;
using MonoUtils.Ui.Objects.TextSystem;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.LevelSystem.Settings;
using NoNameButtonGame.Music;

namespace NoNameButtonGame.LevelSystem.Endless;

public class Level : SampleLevel
{
    public event Action Selected;


    public Level(Display display, Vector2 window, Random random, EffectsRegistry effectsRegistry,
        SettingsAndSaveManager saveManager) : base(display, window, random,
        effectsRegistry, saveManager)
    {
        var textComponent = TextProvider.GetText("Levels.Endless");
        Name = textComponent.GetValue("Name");
        var progress = saveManager.GetSave<EndlessProgress>();

        Default3.Play();

        var text = new Text(textComponent.GetValue("Header"), 3);
        text.InRectangle(Camera.Rectangle)
            .OnX(0.5F)
            .OnY(0.15F)
            .Centered()
            .Move();
        AutoManaged.Add(text);
        text = new Text(textComponent.GetValue("Info1"));
        text.InRectangle(Camera.Rectangle)
            .OnX(0.5F)
            .OnY(0.35F)
            .Centered()
            .Move();
        AutoManaged.Add(text);
        text = new Text(textComponent.GetValue("Info2"));
        text.InRectangle(Camera.Rectangle)
            .OnX(0.5F)
            .OnY(0.45F)
            .Centered()
            .Move();
        AutoManaged.Add(text);
        text = new Text(string.Format(textComponent.GetValue("Best"), progress.HighestLevel));
        text.InRectangle(Camera.Rectangle)
            .OnX(0.5F)
            .OnY(0.55F)
            .Centered()
            .Move();
        AutoManaged.Add(text);
        var button = new Button(textComponent.GetValue("Start"));
        button.InRectangle(Camera.Rectangle)
            .OnX(0.5F)
            .OnY(0.8F)
            .Centered()
            .Move();
        button.Click += ButtonClicked;
        AutoManaged.Add(button);
    }

    private void ButtonClicked(object obj)
        => Selected?.Invoke();
}