using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoUtils;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Logic.Listener;
using MonoUtils.Logic.Text;
using MonoUtils.Ui;
using MonoUtils.Ui.Objects;
using MonoUtils.Ui.Objects.Buttons;
using MonoUtils.Ui.Objects.TextSystem;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level8;

internal class Level : SampleLevel
{
    private readonly RelativePositionListener _positionListener;
    private readonly DodgeScreen _screenOne;

    private bool focusOnOne = true;

    public Level(Display display, Vector2 window, Random random) : base(display, window, random)
    {
        var textComponent = TextProvider.GetText("Levels.Level8");

        Name = textComponent.GetValue("Name");

        _screenOne = new DodgeScreen(Camera);

        _positionListener = new RelativePositionListener();
        //_positionListener.Add(Cursor, _screenOne);

        AutoManaged.Add(_positionListener);
        AutoManaged.Add(_screenOne);

        void debug()
        {
            Log.WriteLine(Cursor.GetPosition().ToString(), 2);
        }

        AutoManaged.Add(debug);
    }


    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (InputReaderKeyboard.CheckKey(Keys.S))
        {
            Camera.Move(Camera.Position + new Vector2(0, 1));
        }
    }
}