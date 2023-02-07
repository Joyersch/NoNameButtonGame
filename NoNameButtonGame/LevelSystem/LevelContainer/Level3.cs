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
using NoNameButtonGame.Text;
using NoNameButtonGame.Colors;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.Debug;
using NoNameButtonGame.LogicObjects;

namespace NoNameButtonGame.LevelSystem.LevelContainer;

internal class Level3 : SampleLevel
{
    private Random _random;

    private readonly Cursor _cursor;
    private readonly MousePointer _mousePointer;
    private readonly GameObjectLinker _objectLinker;

    private readonly TextButton _magicButton;
    private readonly Rainbow _rainbowMagicColor;
    private readonly ColorLinker _colorLinker;

    private readonly TextButton _lockButton;
    private readonly Rainbow _rainbowWinColor;
    private readonly ButtonLock _buttonLock;

    private readonly TextBuilder _info1;
    private readonly TextBuilder _info2;

    public Level3(int defaultWidth, int defaultHeight, Vector2 window, Random random) : base(defaultWidth,
        defaultHeight,
        window, random)
    {
        Name = "Level 3 - Tutorial 2 - Button type: locked";
        _random = random;
        _rainbowMagicColor = new Rainbow();

        _cursor = new Cursor(Vector2.One);
        _mousePointer = new MousePointer();

        _objectLinker = new GameObjectLinker();
        _objectLinker.Add(_mousePointer, _cursor);

        _magicButton = new TextButton(new Vector2(-316,112), "magicUnlockButton", "Magic");
        _magicButton.ClickEventHandler += MagicButtonOnClickEventHandler;

        _rainbowMagicColor = new Rainbow
        {
            Increment = 10,
            GameTimeStepInterval = 25
        };

        _colorLinker = new ColorLinker();
        _colorLinker.Add(_rainbowMagicColor, _magicButton.Text);

        _lockButton = new TextButton(new Vector2(-64,-96), "win", "Finish Level");
        _rainbowWinColor = new Rainbow()
        {
            Increment = 10,
            GameTimeStepInterval = 25,
            Offset = 255
        };
        _colorLinker.Add(_rainbowWinColor, _lockButton.Text);
        
        _buttonLock = new ButtonLock(_lockButton);
        _buttonLock.Callback += Finish;

        _info1 = new TextBuilder("This button here is locked!",
            new Vector2(-160, -128));
        
        _info2 = new TextBuilder(Letter.ReverseParse(Letter.Character.Down) + "This button will unlock the other button",
            new Vector2(-256, 86));
    }

    private void MagicButtonOnClickEventHandler(object obj)
    {
        if (_buttonLock.IsLocked)
            _buttonLock.Unlock();
        else
            _buttonLock.Lock();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        _mousePointer.Update(gameTime, mousePosition);
        _objectLinker.Update(gameTime);
        _cursor.Update(gameTime);

        _rainbowMagicColor.Update(gameTime);
        _rainbowWinColor.Update(gameTime);
        _colorLinker.Update(gameTime);
        _magicButton.Update(gameTime, _cursor.rectangle);

        _buttonLock.Update(gameTime, _cursor.rectangle);
        _info1.Update(gameTime);
        _info2.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _magicButton.Draw(spriteBatch);
        _buttonLock.Draw(spriteBatch);
        
        _info1.Draw(spriteBatch);
        _info2.Draw(spriteBatch);
        
        _cursor.Draw(spriteBatch);
    }
}