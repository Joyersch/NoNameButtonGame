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
using NoNameButtonGame.GameObjects.Debug;
using NoNameButtonGame.LogicObjects;
using NoNameButtonGame.Text;

namespace NoNameButtonGame.LevelSystem.LevelContainer;

internal class Level1 : SampleLevel
{
    private readonly TextButton startButton;
    private readonly Cursor mouseCursor;
    private readonly TextBuilder infoText;
    private readonly GameObjectLinker gameObjectLinker;
    private readonly MousePointer mouse;

    public Level1(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight,
        window, rand)
    {
        startButton = new TextButton(-TextButton.DefaultSize / 2, "Start", "Start");
        startButton.ClickEventHandler += Finish;
        mouseCursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10));
        mouse = new MousePointer();
        Name = "Level 1 - Click the Button!";
        infoText = new TextBuilder("How hard can it be?", new Vector2(-100, -64));
        infoText.ChangePosition(Vector2.Zero -
                                new Vector2(infoText.rectangle.Width,
                                    infoText.rectangle.Height + TextButton.DefaultSize.Y * 2) / 2);
        gameObjectLinker = new GameObjectLinker();
        gameObjectLinker.Add(mouse, mouseCursor);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        mouse.Update(gameTime, mousePosition);
        gameObjectLinker.Update(gameTime);
        mouseCursor.Update(gameTime);
        startButton.Update(gameTime, mouseCursor.Hitbox[0]);
        infoText.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        infoText.Draw(spriteBatch);
        startButton.Draw(spriteBatch);
        mouseCursor.Draw(spriteBatch);
    }
}