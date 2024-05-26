using System;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using MonoUtils;
using MonoUtils.Logic;
using MonoUtils.Logic.Text;
using MonoUtils.Settings;
using MonoUtils.Sound;
using MonoUtils.Ui;
using MonoUtils.Ui.Logic;
using MonoUtils.Ui.Objects;
using MonoUtils.Ui.Objects.TextSystem;
using NoNameButtonGame.LevelSystem.Settings;
using NoNameButtonGame.Music;

namespace NoNameButtonGame.LevelSystem.Credits;

public class Level : SampleLevel
{
    public Level(Display display, Vector2 window, Random random, EffectsRegistry effectsRegistry,
        SettingsAndSaveManager settingsAndSaveManager) : base(display, window, random, effectsRegistry,
        settingsAndSaveManager)
    {
        var textComponent = TextProvider.GetText("Levels.Credits");
        Name = textComponent.GetValue("Name");

        Default2.Play();

        Camera.Zoom = 1F;
        Camera.Update();

        var credits = Global.ReadFromResources("Text.Credits");
        string pattern = @"\[(.*?)\]";

        var result = Regex.Replace(
                credits,
                pattern,
                match => textComponent.GetValue(match.Groups[1].Value))
            .Replace("\r", "")
            .Split("\n");

        float pointer = Camera.Rectangle.Y;
        Text newLine = null;
        foreach (string s in result)
        {
            newLine = TextNotationProcessor.Parse(s);
            newLine.InRectangle(Camera.Rectangle)
                .OnPositionX(-(Camera.Rectangle.Width / 2) + 16)
                .OnPositionY(pointer)
                .ByGridY(1)
                .Move();

            AutoManaged.Add(newLine);
            pointer += newLine.Rectangle.Height + 8;
        }

        OverTimeMover mover = new OverTimeMover(Camera,
            new Vector2(Camera.Rectangle.Center.X, newLine!.Position.Y + Camera.Rectangle.Height / 2F),
            (pointer + -Camera.Rectangle.Y) * 60, OverTimeMover.MoveMode.Lin);

        mover.ArrivedOnDestination += Exit;
        mover.Start();
        AutoManaged.Add(mover);

        Cursor = new Cursor(2F);
        PositionListener.Add(Mouse, Cursor);
    }
}