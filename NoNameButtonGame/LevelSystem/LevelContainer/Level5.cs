using System;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.Colors;
using NoNameButtonGame.GameObjects.AddOn;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.Debug;
using NoNameButtonGame.GameObjects.Text;
using NoNameButtonGame.LogicObjects.Listener;

namespace NoNameButtonGame.LevelSystem.LevelContainer;

internal class Level5 : SampleLevel
{
    private readonly Cursor _cursor;
    private readonly MousePointer _mousePointer;
    private readonly PositionListener _objectLinker;

    private readonly ColorListener _colorListener;
    private readonly Rainbow _pulsatingRed;
    private readonly GlitchBlockCollection _bigBad;

    
    private readonly DelayedText _info;


    public Level5(int defaultWidth, int defaultHeight, Vector2 window, Random random) : base(defaultWidth,
        defaultHeight,
        window, random)
    {
        Name = "Level 4 - Tutorial 3 - Button Addon: Hold";

        _cursor = new Cursor(Vector2.One);
        _mousePointer = new MousePointer();

        _objectLinker = new PositionListener();
        _objectLinker.Add(_mousePointer, _cursor);

        _colorListener = new ColorListener();
        _pulsatingRed = new Rainbow();
        
        _bigBad = new GlitchBlockCollection(Vector2.One, new Vector2(64, 64), 2F);
        _bigBad.Move(new Vector2(128, -_bigBad.Rectangle.Height / 2F));
        _bigBad.Enter += Fail;
        _bigBad.DrawColor = Color.DarkRed;
        _colorListener.Add(_pulsatingRed, _bigBad);
        
        
        _info = new DelayedText("This is a long text to test the delayed text", new Vector2(-128,-64), 0.5F)
        {
            StartAfter = 5000
        };
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        _mousePointer.Update(gameTime, MousePosition);
        _objectLinker.Update(gameTime);
        _cursor.Update(gameTime);

        _colorListener.Update(gameTime);
        _pulsatingRed.Update(gameTime);
        _info.Update(gameTime);
        _bigBad.Update(gameTime, _cursor.Hitbox[0]);
        _bigBad.Move(_bigBad.Position - new Vector2(1, 0));
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _bigBad.Draw(spriteBatch);
        _info.Draw(spriteBatch);
        _cursor.Draw(spriteBatch);
    }

    private void FakeReset(object sender)
    {
        SetMousePositionToCenter();
        if (_info.IsPlaying || _info.HasPlayed)
            return;
        _info.Start();
    }
}