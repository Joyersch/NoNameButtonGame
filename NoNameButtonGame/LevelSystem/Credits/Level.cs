using System;
using System.Text.RegularExpressions;
using Joyersch.Monogame;
using Joyersch.Monogame.Sound;
using Joyersch.Monogame.Storage;
using Joyersch.Monogame.Ui;
using Joyersch.Monogame.Ui.Text;
using Microsoft.Xna.Framework;
using NoNameButtonGame.Music;

namespace NoNameButtonGame.LevelSystem.Credits;

public class Level : SampleLevel
{
    public Level(Scene scene, Random random, EffectsRegistry effectsRegistry,
        SettingsAndSaveManager<string> settingsAndSaveManager) : base(scene, random, effectsRegistry,
        settingsAndSaveManager)
    {
        var textComponent = TextProvider.GetText("Levels.Credits");
        Name = textComponent.GetValue("Name");

        Default2.Play();

        var credits = Global.ReadFromResources("Text.Credits");
        string pattern = @"\[(.*?)\]";

        var result = Regex.Replace(
                credits,
                pattern,
                match => textComponent.GetValue(match.Groups[1].Value))
            .Replace("\r", "")
            .Split("\n");

        float pointer = Camera.Rectangle.Y;
        BasicText newLine = null;
        PositionCalculator positionCaluclator = null;
        foreach (string s in result)
        {
            newLine = TextNotationProcessor.Parse(s);
            newLine.SetScale(Display.Scale);
            AutoManaged.Add(newLine);

            newLine.InRectangle(Camera)
                .OnPositionX(-(Camera.Rectangle.Width / 2) + 16)
                .OnPositionY(pointer)
                .ByGridY(1).Apply();

            pointer += newLine.Rectangle.Height + 8;
        }

        OverTimeMover mover = new OverTimeMover(Camera,
            new Vector2(Camera.Rectangle.Center.X, newLine!.Position.Y + Camera.Rectangle.Height / 2F),
            (pointer + -Camera.Rectangle.Y) * 60, OverTimeMover.MoveMode.Lin);

        mover.ArrivedOnDestination += Exit;
        mover.Start();
        AutoManaged.Add(mover);
        DynamicScaler.Apply(Display.Scale);
    }
}