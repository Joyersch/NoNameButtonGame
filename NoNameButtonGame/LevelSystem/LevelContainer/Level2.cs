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

class Level2 : SampleLevel
{
    private readonly StateButton _stateButton;
    private readonly Cursor _mouseCursor;
    private readonly TextBuilder _infoAboutButton;
    private readonly TextBuilder _infoAboutButton2;
    private readonly TextBuilder _infoAboutButton3;
    private readonly MousePointer _mousePointer;
    private readonly GameObjectLinker _linker;

    public Level2(int defaultWidth, int defaultHeight, Vector2 window, Random random) : base(defaultWidth,
        defaultHeight, window, random)
    {
        Name = "Level 2 - Tutorial 1 - Button type: State";
        _stateButton = new StateButton(new Vector2(-280, -16), 5);
        _stateButton.ClickEventHandler += Finish;
        _infoAboutButton = new TextBuilder("This is a \"StateButton\".", new Vector2(-128, -128));
        _infoAboutButton2 = new TextBuilder("Press the button to lower the number.", new Vector2(-128, 0));
        _infoAboutButton3 = new TextBuilder("When it hits 0, you win!", new Vector2(-128, 16));
        _mouseCursor = new Cursor(Vector2.One);
        _mousePointer = new MousePointer();
        _linker = new GameObjectLinker();
        _linker.Add(_mousePointer, _mouseCursor);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        _mousePointer.Update(gameTime, mousePosition);
        _linker.Update(gameTime);
        _mouseCursor.Update(gameTime);
        _stateButton.Update(gameTime, _mouseCursor.rectangle);
        _infoAboutButton.Update(gameTime);
        _infoAboutButton2.Update(gameTime);
        _infoAboutButton3.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _stateButton.Draw(spriteBatch);
        _infoAboutButton.Draw(spriteBatch);
        _infoAboutButton2.Draw(spriteBatch);
        _infoAboutButton3.Draw(spriteBatch);
        _mouseCursor.Draw(spriteBatch);
    }
}