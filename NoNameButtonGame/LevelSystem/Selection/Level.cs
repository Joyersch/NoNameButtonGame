using System;
using Microsoft.Xna.Framework;
using MonoUtils.Logic;
using MonoUtils.Logic.Text;
using MonoUtils.Ui;
using MonoUtils.Ui.Logic;
using MonoUtils.Ui.Objects;
using MonoUtils.Ui.Objects.Buttons;
using NoNameButtonGame.GameObjects.Buttons;

namespace NoNameButtonGame.LevelSystem.Selection;

public class Level : SampleLevel
{
    private readonly OverTimeMover _mover;
    public event Action<int> OnLevelSelect;

    private int _cameraLevel;

    public Level(Display display, Vector2 window, Random rand, Progress progress) : base(display,
        window, rand)
    {
        var textComponent = TextProvider.GetText("Levels.Select");
        Name = textComponent.GetValue("Name");

        _mover = new OverTimeMover(Camera, Vector2.Zero, 500, OverTimeMover.MoveMode.Sin);
        AutoManaged.Add(_mover);

        int maxLevel = progress.MaxLevel;
        int screens = (int)Math.Ceiling(maxLevel / 10F);

        int placed = 0;
        for (int i = 0; i < screens; i++)
        {
            for (int y = 0; y < 2 && placed < maxLevel; y++)
            {
                for (int x = 0; x < 5 && placed < maxLevel; x++)
                {
                    var levelButton =
                        new MiniButton((placed + 1).ToString(), (placed + 1).ToString());
                    levelButton.GetCalculator(Camera.Rectangle)
                        .OnX(0.20F + 0.15F * x)
                        .OnY(0.4F + 0.2F * y)
                        .ByGridY(i)
                        .Centered()
                        .Move();
                    levelButton.Click += SelectLevel;
                    AutoManaged.Add(levelButton);
                    placed++;
                }
            }

            if (maxLevel <= placed)
                break;

            var down = new MiniButton("[down]");
            down.GetCalculator(Camera.Rectangle)
                .OnX(0.1F)
                .OnY(0.9F)
                .ByGridY(i)
                .Centered()
                .Move();
            down.Click += MoveDown;
            AutoManaged.Add(down);

            var up = new MiniButton("[up]");
            up.GetCalculator(Camera.Rectangle)
                .OnX(0.1F)
                .OnY(1.1F)
                .ByGridY(i)
                .Centered()
                .Move();
            up.Click += MoveUp;
            AutoManaged.Add(up);
        }
    }

    private void SelectLevel(object sender)
        => OnLevelSelect?.Invoke(int.Parse(((MiniButton)sender).Name));

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
    }
}