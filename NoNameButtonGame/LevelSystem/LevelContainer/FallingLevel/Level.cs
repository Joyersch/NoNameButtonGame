using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logic;
using MonoUtils.Logic.Text;
using MonoUtils.Sound;
using MonoUtils.Ui;
using MonoUtils.Ui.Logic;
using NoNameButtonGame.GameObjects.Glitch;
using NoNameButtonGame.Music;

namespace NoNameButtonGame.LevelSystem.LevelContainer.FallingLevel;

internal class Level : SampleLevel
{
    private readonly Random _random;
    private Queue<(Row row, OverTimeMover mover)> _rows;

    private bool _skipCreation;
    private int _totalRowsCount;

    private bool _hasWon;
    private bool _checkOffscreen;

    public Level(Display display, Vector2 window, Random random, EffectsRegistry effectsRegistry,
        float difficulty = 1F) : base(display,
        window, random, effectsRegistry)
    {
        _random = random;
        var textComponent = TextProvider.GetText("Levels.FallingLevel");
        Name = textComponent.GetValue("Name");

        DnB2.Play();

        _rows = new Queue<(Row row, OverTimeMover mover)>();

        var cleanDifficulty = (difficulty + 100F) / 1050F;
        if (cleanDifficulty > 1F)
            cleanDifficulty = 1F;

        var flippedDifficulty = 3.5F - cleanDifficulty * 2F;
        float winCondition = 10 + 40 * cleanDifficulty;
        float singleScale = 5F;
        float wallWidth = 3F;

        float moveTime = (664F * flippedDifficulty + 50F) / 5;
        float singleHeight = GlitchBlock.ImageSize.Y * singleScale;
        var singleSize = GlitchBlock.ImageSize * singleScale;

        int oneGridLength = Camera.Rectangle.Size.Y;
        var size = new Vector2(GlitchBlock.ImageSize.X * (singleScale * wallWidth), oneGridLength + singleHeight);
        var left = new GlitchBlockCollection(size, singleScale);
        left.InRectangle(Camera.Rectangle)
            .Move();
        left.Enter += Fail;
        AutoManaged.Add(left);

        var leftStartPosition = left.GetPosition();
        var leftDestination = new Vector2(leftStartPosition.X, leftStartPosition.Y - singleHeight);
        var leftMover = new OverTimeMover(left, leftDestination, moveTime, OverTimeMover.MoveMode.Lin);
        leftMover.Start();
        leftMover.ArrivedOnDestination += delegate
        {
            left.Move(leftStartPosition);
            _hasWon = _totalRowsCount >= winCondition;
            if (!_skipCreation && !_hasWon)
            {
                var destination = leftStartPosition + new Vector2(left.GetSize().X, -singleHeight);
                CreateRow(left, singleSize, singleScale, destination, moveTime * 10, left.GetCurrentFrame());
            }

            _skipCreation = !_skipCreation;
            leftMover.Start();
        };
        AutoManaged.Add(leftMover);

        var right = new GlitchBlockCollection(size, singleScale);
        right.InRectangle(Camera.Rectangle)
            .OnX(1F)
            .BySizeX(-1F)
            .Move();
        right.Enter += Fail;
        AutoManaged.Add(right);

        var rightStartPosition = right.GetPosition();
        var rightDestination = new Vector2(rightStartPosition.X, rightStartPosition.Y - singleHeight);
        var rightMover = new OverTimeMover(right, rightDestination, moveTime, OverTimeMover.MoveMode.Lin);
        rightMover.Start();
        rightMover.ArrivedOnDestination += delegate
        {
            right.Move(rightStartPosition);
            rightMover.Start();
        };
        AutoManaged.Add(rightMover);
        if (difficulty > 1F)
            _checkOffscreen = true;
    }

    private void CreateRow(IMoveable anchor, Vector2 singleSize, float singleScale, Vector2 destination, float time, int frame)
    {
        var row = new Row(Vector2.Zero, singleSize, singleScale, frame);
        row.GetAnchor(anchor)
            .SetMainAnchor(AnchorCalculator.Anchor.BottomRight)
            .SetSubAnchor(AnchorCalculator.Anchor.BottomLeft)
            .Move();
        row.Enter += Fail;

        var id = _random.Next(10);
        row.SetPath(id);

        var mover = new OverTimeMover(row, destination, time, OverTimeMover.MoveMode.Lin);
        mover.ArrivedOnDestination += RemoveOldestRow;
        mover.Start();
        _rows.Enqueue((row, mover));
        _totalRowsCount++;
    }

    private void RemoveOldestRow()
    {
        _rows.Dequeue();
    }

    public override void Update(GameTime gameTime)
    {
        if (_checkOffscreen && !Cursor.Rectangle.Intersects(Camera.Rectangle))
            Fail();

        var rows = _rows.ToArray();
        foreach (var row in rows)
        {
            row.mover.Update(gameTime);
            row.row.Update(gameTime);
            row.row.UpdateInteraction(gameTime, Cursor);
        }

        if (rows.Length == 0 && _hasWon)
        {
            Finish();
        }

        base.Update(gameTime);
    }

    protected override void Draw(SpriteBatch spriteBatch)
    {
        var rows = _rows.ToArray();
        foreach (var row in rows)
        {
            row.row.Draw(spriteBatch);
        }
        base.Draw(spriteBatch);
    }
}