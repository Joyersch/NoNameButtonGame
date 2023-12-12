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

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level5;

internal class Level : SampleLevel
{
    private readonly Cursor _cursor;
    private readonly OverTimeMover _moveCamera;
    private readonly Invisible _invisible;

    public Level(Display display, Vector2 window, Random random) : base(display, window, random)
    {
        var textComponent = TextProvider.GetText("Levels.Level5");

        Name = textComponent.GetValue("Name");

        var halfScreen = new Vector2(Camera.Rectangle.Width / 2F, Camera.Rectangle.Height);
        var leftWall = new GlitchBlockCollection(halfScreen, 1F);
        leftWall.GetCalculator(Camera.Rectangle)
            .OnX(0)
            .BySizeX(-1F)
            .Move();
        leftWall.ChangeColor(GlitchBlock.Color);
        leftWall.Enter += Fail;
        AutoManaged.Add(leftWall);

        Log.WriteInformation(leftWall.Rectangle.TopRightCorner().ToString());

        var rightWall = new GlitchBlockCollection(halfScreen, 1F);
        rightWall.GetCalculator(Camera.Rectangle)
            .OnX(1)
            .Move();
        rightWall.ChangeColor(GlitchBlock.Color);
        rightWall.Enter += Fail;

        AutoManaged.Add(rightWall);

        var wallLeftEnd = Camera.Rectangle.Location.ToVector2();
        OverTimeMover moverLeft = new OverTimeMover(leftWall, wallLeftEnd, 10000F, OverTimeMover.MoveMode.Lin);
        moverLeft.Start();
        AutoManaged.Add(moverLeft);

        var wallRightEnd = new Vector2(0, Camera.Rectangle.Top);
        OverTimeMover moverRight = new OverTimeMover(rightWall, wallRightEnd, 10000F,
            OverTimeMover.MoveMode.Lin);
        moverRight.Start();
        AutoManaged.Add(moverRight);

        Vector2 belowLocation = Camera.Position + new Vector2(0, Camera.Rectangle.Height);
        _moveCamera = new OverTimeMover(Camera, belowLocation, 666F, OverTimeMover.MoveMode.Sin);
        AutoManaged.Add(_moveCamera);

        _invisible = new Invisible(Camera.RealPosition, Camera.RealSize);
        _invisible.Leave += delegate
        {
            if (_cursor is null)
                return;

            if (_moveCamera.IsMoving)
                return;

            if (_cursor.Position.Y < 0)
            {
                Fail();
                return;
            }

            _moveCamera.Start();
        };

        AutoManaged.Add(_invisible);

        _cursor = new Cursor();
        Actuator = _cursor;
        PositionListener.Add(Mouse, _cursor);
        AutoManaged.Add(_cursor);
    }
}