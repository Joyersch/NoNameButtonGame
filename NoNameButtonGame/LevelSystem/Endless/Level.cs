using System;
using Microsoft.Xna.Framework;
using MonoUtils.Logic;
using MonoUtils.Logic.Text;
using MonoUtils.Settings;
using MonoUtils.Sound;
using MonoUtils.Ui;
using MonoUtils.Ui.Objects.TextSystem;
using NoNameButtonGame.GameObjects.Buttons;

namespace NoNameButtonGame.LevelSystem.Endless;

public class Level : SampleLevel
{
    public event Action Selected;


    public Level(Display display, Vector2 window, Random random, EffectsRegistry effectsRegistry,
        SettingsAndSaveManager saveManager) : base(display,
        window, random, effectsRegistry)
    {
        var textComponent = TextProvider.GetText("Levels.Endless");
        Name = textComponent.GetValue("Name");
        var progress = saveManager.GetSave<EndlessProgress>();

        var text = new Text(textComponent.GetValue("Info1"));
        text.GetCalculator(Camera.Rectangle)
            .OnX(0.5F)
            .OnY(0.3F)
            .Centered()
            .Move();
        AutoManaged.Add(text);
        text = new Text(textComponent.GetValue("Info2"));
        text.GetCalculator(Camera.Rectangle)
            .OnX(0.5F)
            .OnY(0.4F)
            .Centered()
            .Move();
        AutoManaged.Add(text);
        text = new Text(string.Format(textComponent.GetValue("Best"), progress.HighestLevel));
        text.GetCalculator(Camera.Rectangle)
            .OnX(0.5F)
            .OnY(0.5F)
            .Centered()
            .Move();
        AutoManaged.Add(text);
        var button = new Button(textComponent.GetValue("Start"));
        button.GetCalculator(Camera.Rectangle)
            .OnX(0.5F)
            .OnY(0.66F)
            .Centered()
            .Move();
        button.Click += ButtonClicked;
        AutoManaged.Add(button);
    }

    private void ButtonClicked(object obj)
        => Selected?.Invoke();
}