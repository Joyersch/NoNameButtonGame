using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoUtils;
using MonoUtils.Logic;
using MonoUtils.Logic.Text;
using MonoUtils.Ui;
using MonoUtils.Ui.Objects;
using MonoUtils.Ui.Objects.Buttons;
using MonoUtils.Ui.Objects.TextSystem;
using NoNameButtonGame.GameObjects.Glitch;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level5;

internal class Level : SampleLevel
{
    public Level(Display display, Vector2 window, Random random) : base(display, window, random)
    {
        var textComponent = TextProvider.GetText("Levels.Level5");

        Name = textComponent.GetValue("Name");

        GlitchBlockCollection leftWall = new GlitchBlockCollection(new Vector2(40, 40));

        leftWall.GetCalculator(Camera.Rectangle)
            .OnCenter()
            .Centered()
            .Move();
        AutoManaged.Add(leftWall);

        GlitchBlockCollection rightWall = new GlitchBlockCollection(new Vector2(64, 64));
        rightWall.GetAnchor(leftWall)
            .SetMainAnchor(AnchorCalculator.Anchor.Right)
            .SetSubAnchor(AnchorCalculator.Anchor.Left)
            .Move();
        AutoManaged.Add(rightWall);

        var cursor = new Cursor();
        Actuator = cursor;
        PositionListener.Add(Mouse, cursor);
        AutoManaged.Add(cursor);
    }

    private float zoom = 1F;

    public override void Update(GameTime gameTime)
    {
        if (InputReaderKeyboard.CheckKey(Keys.Left, true))
            Camera.Move(Camera.Position - new Vector2(-10, 0));
        if (InputReaderKeyboard.CheckKey(Keys.Right, true))
            Camera.Move(Camera.Position - new Vector2(10, 0));
        if (InputReaderKeyboard.CheckKey(Keys.Up, true))
            Camera.Move(Camera.Position - new Vector2(0, -10));
        if (InputReaderKeyboard.CheckKey(Keys.Down, true))
            Camera.Move(Camera.Position - new Vector2(0, 10));

        if (InputReaderMouse.CheckKey(InputReaderMouse.MouseKeys.MouseUp))
            zoom -= 0.4F;

        if (InputReaderMouse.CheckKey(InputReaderMouse.MouseKeys.MouseDown))
            zoom += 0.4F;

        if (zoom == 0F)
            zoom = 0.1F;

        if (zoom > 10F)
            zoom = 10F;

        Camera.Zoom = zoom;

        base.Update(gameTime);
    }
}