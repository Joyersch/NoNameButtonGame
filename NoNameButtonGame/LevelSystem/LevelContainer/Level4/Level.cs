using System;
using Microsoft.Xna.Framework;
using MonoUtils;
using MonoUtils.Logic.Hitboxes.Collision;
using MonoUtils.Ui;
using MonoUtils.Ui.Objects;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level4;

public class Level : SampleLevel
{
    private GameObject _stationary;
    private VelocityAdapter _dynamic;

    private Cursor _cursor;
    
    public Level(Display display, Vector2 window, Random random) : base(display, window, random)
    {
        Name = "Level 4 - RPG";


        _stationary = new GameObject(new Vector2(100,0), new Vector2(100,100));
        AutoManaged.Add(_stationary);
        
        _dynamic = new VelocityAdapter(new GameObject(new Vector2(-100,0), new Vector2(100,100)));
        AutoManaged.Add(_dynamic);

        _cursor = new Cursor();
        Actuator = _stationary;
        AutoManaged.Add(_cursor);
        PositionListener.Add(Mouse, _cursor);
    }

    public override void Update(GameTime gameTime)
    {
        if (InputReaderMouse.CheckKey(InputReaderMouse.MouseKeys.Left, false))
            _dynamic.AddVelocity(_cursor.Position - _dynamic.Object.Position);
        base.Update(gameTime);
    }
}