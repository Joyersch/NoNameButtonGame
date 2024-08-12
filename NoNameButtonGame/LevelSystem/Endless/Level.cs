using System;
using Microsoft.Xna.Framework;
using MonoUtils;
using MonoUtils.Logic;
using MonoUtils.Logic.Text;
using MonoUtils.Settings;
using MonoUtils.Sound;
using MonoUtils.Ui;
using MonoUtils.Ui.Buttons;
using MonoUtils.Ui.Logic;
using MonoUtils.Ui.TextSystem;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.LevelSystem.Settings;
using NoNameButtonGame.Music;

namespace NoNameButtonGame.LevelSystem.Endless;

public class Level : SampleLevel
{
    public event Action Selected;

    public Level(Scene scene, Random random, EffectsRegistry effectsRegistry,
        SettingsAndSaveManager<string> saveManager) : base(scene, random,
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
        button.Click += _ => Selected?.Invoke();
        AutoManaged.Add(button);

        var left = Camera.Rectangle.Center.ToVector2();
        var right = left + new Vector2(Camera.Rectangle.Size.X, 0F);

        var mover = new OverTimeMover(Camera, right, 666F, OverTimeMover.MoveMode.Sin);
        AutoManaged.Add(mover);

        button = new Button(textComponent.GetValue("Challenges"));
        button.InRectangle(Camera.Rectangle)
            .OnX(0.9F)
            .OnY(0.9F)
            .Centered()
            .Move();
        button.Click += _ =>
        {
            if (mover.IsMoving)
                return;
            mover.ChangeDestination(right);
            mover.Start();
        };
        AutoManaged.Add(button);

        button = new Button(textComponent.GetValue("Back"));
        button.InRectangle(Camera.Rectangle)
            .OnX(0.1F)
            .OnY(0.9F)
            .ByGridX(1)
            .Centered()
            .Move();
        button.Click += _ =>
        {
            if (mover.IsMoving)
                return;
            mover.ChangeDestination(left);
            mover.Start();
        };
        AutoManaged.Add(button);

        var challenges = saveManager.GetSave<Challenges>();

        text = new Text(textComponent.GetValue("Challenges"));
        text.InRectangle(Camera.Rectangle)
            .OnX(0.5F)
            .OnY(0.175F)
            .ByGridX(1)
            .Centered()
            .Move();
        AutoManaged.Add(text);

        text = new Text(textComponent.GetValue("Beat10"));
        text.ChangeColor(challenges.Score10 ? Color.White : Color.Gray);
        text.InRectangle(Camera.Rectangle)
            .OnX(0.5F)
            .OnY(0.266F)
            .ByGridX(1)
            .Centered()
            .Move();
        AutoManaged.Add(text);

        text = new Text(textComponent.GetValue("Beat25"));
        text.ChangeColor(challenges.Score25 ? Color.White : Color.Gray);
        text.InRectangle(Camera.Rectangle)
            .OnX(0.5F)
            .OnY(0.333F)
            .ByGridX(1)
            .Centered()
            .Move();
        AutoManaged.Add(text);

        text = new Text(textComponent.GetValue("Beat50"));
        text.ChangeColor(challenges.Score50 ? Color.White : Color.Gray);
        text.InRectangle(Camera.Rectangle)
            .OnX(0.5F)
            .OnY(0.4F)
            .ByGridX(1)
            .Centered()
            .Move();
        AutoManaged.Add(text);

        text = new Text(textComponent.GetValue("Time1h"));
        text.ChangeColor(challenges.Time1h ? Color.White : Color.Gray);
        text.InRectangle(Camera.Rectangle)
            .OnX(0.5F)
            .OnY(0.466F)
            .ByGridX(1)
            .Centered()
            .Move();
        AutoManaged.Add(text);

        text = new Text(textComponent.GetValue("Time30min"));
        text.ChangeColor(challenges.Time30min ? Color.White : Color.Gray);
        text.InRectangle(Camera.Rectangle)
            .OnX(0.5F)
            .OnY(0.533F)
            .ByGridX(1)
            .Centered()
            .Move();
        AutoManaged.Add(text);
    }
}