using System;
using System.Collections.Generic;
using System.Text;
using NoNameButtonGame.Input;
using NoNameButtonGame.Camera;
using NoNameButtonGame.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NoNameButtonGame.Hitboxes;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.Text;

namespace NoNameButtonGame.LevelSystem.LevelContainer;

internal class Level6 : SampleLevel
{
    private readonly Cursor cursor;
    private readonly GlitchBlockCollection WallLeft;
    private readonly GlitchBlockCollection WallRight;
    private readonly GlitchBlockCollection WallButtom;

    private readonly GlitchBlockCollection _block;
    private readonly LockButton _lockButton;
    private readonly HoldButton _unlockbutton;
    private float gameTime;
    private bool moveLeft = false;

    public Level6(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth,
        defaultHeight, window, rand)
    {
        Name = "Level 6 - Now what?!";

        cursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10));
        WallLeft = new GlitchBlockCollection(new Vector2(-512, -512), new Vector2(420, 1024));
        WallRight = new GlitchBlockCollection(new Vector2(96, -512), new Vector2(420, 1024));
        WallButtom = new GlitchBlockCollection(new Vector2(-512, 96), new Vector2(1024, 1024));
        _block = new GlitchBlockCollection(new Vector2(-256, 32), new Vector2(64, 64));
        WallRight.EnterEventHandler += Fail;
        WallLeft.EnterEventHandler += Fail;
        WallButtom.EnterEventHandler += Fail;
        _block.EnterEventHandler += Fail;
        _lockButton = new LockWinButton(new Vector2(-32, -128), new Vector2(64, 32), true);
        _lockButton.ClickEventHandler += Finish;
        _unlockbutton = new HoldButton(new Vector2(-32, 48), new Vector2(64, 32))
        {
            EndHoldTime = 5000
        };
        _unlockbutton.ClickEventHandler += UnlockButton;
    }

    private void UnlockButton(object sender)
        => _lockButton.Unlock();


    public override void Update(GameTime gameTime)
    {
        cursor.Update(gameTime);
        base.Update(gameTime);
        cursor.Position = mousePosition - cursor.Size / 2;
        this.gameTime += (float) gameTime.ElapsedGameTime.TotalMilliseconds;

        while (this.gameTime > 8)
        {
            this.gameTime -= 8;
            if (moveLeft)
                _block.Move(new Vector2(-2, 0));
            else
                _block.Move(new Vector2(2, 0));

            if (_block.Position.X > 180)
                moveLeft = true;
            if (_block.Position.X < -180)
                moveLeft = false;
        }

        _unlockbutton.Update(gameTime, cursor.Hitbox[0]);
        _lockButton.Update(gameTime, cursor.Hitbox[0]);
        WallLeft.Update(gameTime, cursor.Hitbox[0]);
        WallRight.Update(gameTime, cursor.Hitbox[0]);
        WallButtom.Update(gameTime, cursor.Hitbox[0]);
        _block.Update(gameTime, cursor.Hitbox[0]);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _lockButton.Draw(spriteBatch);
        _unlockbutton.Draw(spriteBatch);
        _block.Draw(spriteBatch);
        WallLeft.Draw(spriteBatch);
        WallRight.Draw(spriteBatch);
        WallButtom.Draw(spriteBatch);

        cursor.Draw(spriteBatch);
    }
}