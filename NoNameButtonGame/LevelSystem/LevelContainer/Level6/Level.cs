using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoUtils;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Logic.Text;
using MonoUtils.Ui;
using MonoUtils.Ui.Color;
using MonoUtils.Ui.Logic;
using MonoUtils.Ui.Objects;
using MonoUtils.Ui.Objects.Buttons;
using MonoUtils.Ui.Objects.TextSystem;
using NoNameButtonGame.GameObjects.Glitch;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level6;

internal class Level : SampleLevel
{
    private readonly Cursor _cursor;
    private readonly OverTimeMover _moveCamera;
    private readonly Invisible _invisible;
    private CameraAnchorGrid _anchorGrid;

    public Level(Display display, Vector2 window, Random random) : base(display, window, random)
    {
        var textComponent = TextProvider.GetText("Levels.Level6");

        Name = textComponent.GetValue("Name");

        var halfScreen = new Vector2(Camera.Rectangle.Width / 2F, Camera.Rectangle.Height);
        GlitchBlockCollection wall = new GlitchBlockCollection(halfScreen, 1F);
        wall.GetCalculator(Camera.Rectangle)
            .OnX(0)
            .BySizeX(-1F)
            .Move();
        wall.ChangeColor(GlitchBlock.Color);
        wall.Enter += Fail;
        AutoManaged.Add(wall);

        var wallLeftEnd = Camera.Rectangle.Location.ToVector2();
        OverTimeMover moverLeft = new OverTimeMover(wall, wallLeftEnd, 10000F, OverTimeMover.MoveMode.Lin);
        moverLeft.Start();
        AutoManaged.Add(moverLeft);

        wall = new GlitchBlockCollection(halfScreen, 1F);
        wall.GetCalculator(Camera.Rectangle)
            .OnX(1)
            .Move();
        wall.ChangeColor(GlitchBlock.Color);
        wall.Enter += Fail;

        AutoManaged.Add(wall);

        var wallRightEnd = new Vector2(Camera.Rectangle.Center.X, Camera.Rectangle.Top);
        OverTimeMover moverRight = new OverTimeMover(wall, wallRightEnd, 10000F,
            OverTimeMover.MoveMode.Lin);
        moverRight.Start();
        AutoManaged.Add(moverRight);

        Vector2 belowLocation = Camera.Position + new Vector2(0, Camera.Rectangle.Height);

        _cursor = new Cursor();
        Actuator = _cursor;
        PositionListener.Add(Mouse, _cursor);
        AutoManaged.Add(_cursor);

        _anchorGrid = new CameraAnchorGrid(Camera, _cursor, 666F, OverTimeMover.MoveMode.Sin);
        AutoManaged.Add(_anchorGrid);

        wall = new GlitchBlockCollection(Camera.RealSize, 1F);
        wall.ChangeColor(GlitchBlock.Color);
        wall.GetCalculator(Camera.Rectangle)
            .ByGridY(-1)
            .Move();
        wall.Enter += Fail;
        AutoManaged.Add(wall);

        GameObject indicator = new GameObject(Vector2.Zero, new Vector2(16, 16));
        indicator.GetCalculator(Camera.Rectangle)
            .OnCenter()
            .ByGridY(1)
            .Move();
        AutoManaged.Add(indicator);
    }
}