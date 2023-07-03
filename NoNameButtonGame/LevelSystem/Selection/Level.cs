using System;
using Microsoft.Xna.Framework;
using MonoUtils.Logic;
using MonoUtils.Ui;
using MonoUtils.Ui.Objects;
using MonoUtils.Ui.Objects.Buttons;

namespace NoNameButtonGame.LevelSystem.Selection;

public class Level : SampleLevel
{
    private readonly OverTimeMover _mover;
    public event Action<int> LevelSelectedEventHandler;

    private int _cameraLevel;

    public Level(Display display, Vector2 window, Random rand, Storage.Storage storage) : base(display,
        window, rand)
    {
        Name = "Level Selection";

        _mover = new OverTimeMover(Camera, Vector2.Zero, 500, OverTimeMover.MoveMode.Sin);
        AutoManaged.Add(_mover);
        
        int maxLevel = storage.GameData.MaxLevel;
        int screens = maxLevel / 30;


        for (int i = 0; i < screens; i++)
        {
            var down = new MiniTextButton(
                new Vector2(-300, 138 + (Display.CustomHeight / Camera.Zoom) * i)
                , new Vector2(64, 32)
                , ""
                , "⬇"
                , new Vector2(16, 16));

            down.Click += MoveDown;
            AutoManaged.Add(down);

            var up = new MiniTextButton(
                new Vector2(-300, 190 + (Display.CustomHeight / Camera.Zoom) * i)
                , new Vector2(64, 32)
                , ""
                , "⬆"
                , new Vector2(16, 16));
            up.Click += MoveUp;
            AutoManaged.Add(up);
        }

        for (int i = 0; i < maxLevel; i++)
        {
            var levelButton = new MiniTextButton(
                new Vector2(-200 + 100 * (i % 5), -140 + 50 * (i / 5) + 60 * (i / 30))
                , new Vector2(64, 32)
                , (i + 1).ToString()
                , (i + 1).ToString()
                , new Vector2(16, 16));

            levelButton.Click += SelectLevel;
            AutoManaged.Add(levelButton);
        }

        var cursor = new Cursor();
        Actuator = cursor;
        PositionListener.Add(Mouse, cursor);
        AutoManaged.Add(cursor);
    }

    private void SelectLevel(object sender)
        => LevelSelectedEventHandler?.Invoke(int.Parse(((MiniTextButton) sender).Name));

    private void MoveDown(object sender)
    {
        if (_mover.IsMoving)
            return;

        _cameraLevel++;
        SetNewMoveToLocationForCamera();
        _mover.Start();
    }

    private void MoveUp(object sender)
    {
        if (_mover.IsMoving)
            return;

        _cameraLevel--;
        SetNewMoveToLocationForCamera();
        _mover.Start();
    }

    private void SetNewMoveToLocationForCamera()
        => _mover.ChangeDestination(new Vector2(0, 360 * _cameraLevel));

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        base.CurrentMusic(string.Empty);
    }
}