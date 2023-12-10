using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoUtils;
using MonoUtils.Logic;
using MonoUtils.Logic.Text;
using MonoUtils.Ui;
using MonoUtils.Ui.Color;
using MonoUtils.Ui.Logic;
using MonoUtils.Ui.Objects;
using MonoUtils.Ui.Objects.TextSystem;

namespace NoNameButtonGame.LevelSystem.Credits;

public class Level : SampleLevel
{
    public Level(Display display, Vector2 window, Random random) : base(display, window, random)
    {
        var textComponent = TextProvider.GetText("Levels.Credits");
        Name = textComponent.GetValue("Name");

        Camera.Zoom = 1F;
        Camera.Update();

        var credits = Global.ReadFromResources("Text.Credits");
        string pattern = @"\[(.*?)\]";

        var result = Regex.Replace(
                credits,
                pattern,
                match => textComponent.GetValue(match.Groups[1].Value))
            .Split("\n");

        float pointer = Camera.Rectangle.Y;
        Text newLine = null;
        foreach (string s in result)
        {
            newLine = TextNotationProcessor.Parse(s);
            newLine.GetCalculator(Camera.Rectangle)
                .OnPositionX(-(Camera.Rectangle.Width / 2) + 16)
                .OnPositionY(pointer)
                .ByGridY(1)
                .Move();

            AutoManaged.Add(newLine);
            pointer += newLine.Rectangle.Height + Text.DefaultLetterSize.Y / 2;
        }

        OverTimeMover mover = new OverTimeMover(Camera,
            new Vector2(Camera.Rectangle.Center.X, newLine.Position.Y + Camera.Rectangle.Height / 2),
            (pointer + -Camera.Rectangle.Y) * 50, OverTimeMover.MoveMode.Lin);

        mover.ArrivedOnDestination += Exit;
        mover.Start();
        AutoManaged.Add(mover);

        var cursor = new Cursor(2F);
        Actuator = cursor;
        PositionListener.Add(Mouse, cursor);
        AutoManaged.Add(cursor);
    }
}