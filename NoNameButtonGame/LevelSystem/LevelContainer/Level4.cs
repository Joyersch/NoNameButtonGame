using System;
using System.Collections.Generic;
using System.Text;
using NoNameButtonGame.Input;
using NoNameButtonGame.Camera;
using NoNameButtonGame.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NoNameButtonGame.Cache;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.Text;
using NoNameButtonGame.Colors;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.Debug;
using NoNameButtonGame.LogicObjects;

namespace NoNameButtonGame.LevelSystem.LevelContainer;

internal class Level4 : SampleLevel
{
    private Random _random;

    private readonly Cursor _cursor;
    private readonly MousePointer _mousePointer;
    private readonly GameObjectLinker _objectLinker;

    private readonly ColorLinker _colorLinker;
    private readonly PulsatingRed _pulsatingRed;
    private readonly GlitchBlockCollection bigBad;


    public Level4(int defaultWidth, int defaultHeight, Vector2 window, Random random) : base(defaultWidth,
        defaultHeight,
        window, random)
    {
        Name = "Level 4 - Tutorial 3 - The big bad";
        _random = random;

        _cursor = new Cursor(Vector2.One);
        _mousePointer = new MousePointer();

        _objectLinker = new GameObjectLinker();
        _objectLinker.Add(_mousePointer, _cursor);

        _colorLinker = new ColorLinker();
        _pulsatingRed = new PulsatingRed();
        bigBad = new GlitchBlockCollection(new Vector2(32,32), new Vector2(64,96), 2F);
        bigBad.EnterEventHandler += Fail;
        _colorLinker.Add(_pulsatingRed, bigBad);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        
        _mousePointer.Update(gameTime, mousePosition);
        _objectLinker.Update(gameTime);
        _cursor.Update(gameTime);
        
        _colorLinker.Update(gameTime);
        _pulsatingRed.Update(gameTime);
        bigBad.Update(gameTime, _cursor.Hitbox[0]);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        bigBad.Draw(spriteBatch);
        _cursor.Draw(spriteBatch);
    }
}