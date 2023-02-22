using System;
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

internal class Level2 : SampleLevel
{
    private readonly Cursor _cursor;
    private readonly MousePointer _mousePointer;
    private readonly PositionListener _objectLinker;

    private readonly TextButton _magicButton;
    private readonly Rainbow _rainbowMagicColor;
    private readonly ColorListener _colorListener;

    private readonly Rainbow _rainbowWinColor;
    private readonly LockButtonAddon _lockButtonAddon;

    private readonly TextBuilder _info1;
    private readonly TextBuilder _info2;

    public Level2(int defaultWidth, int defaultHeight, Vector2 window, Random random) : base(defaultWidth,
        defaultHeight,
        window, random)
    {
        Name = "Level 2 - Tutorial 1 - Button Addon: Lock";
        _rainbowMagicColor = new Rainbow();

        _cursor = new Cursor(Vector2.One);
        _mousePointer = new MousePointer();

        _objectLinker = new PositionListener();
        _objectLinker.Add(_mousePointer, _cursor);

        _magicButton = new TextButton(-TextButton.DefaultSize / 2 + new Vector2(0, TextButton.DefaultSize.Y),
            "magicUnlockButton", "Magic");
        _magicButton.Click += MagicButtonOnClick;

        _rainbowMagicColor = new Rainbow
        {
            Increment = 10,
            GameTimeStepInterval = 25
        };

        _colorListener = new ColorListener();
        _colorListener.Add(_rainbowMagicColor, _magicButton.Text);

        var lockButton = new TextButton(-TextButton.DefaultSize / 2 + new Vector2(0, -TextButton.DefaultSize.Y), "win",
            "Finish Level");
        _rainbowWinColor = new Rainbow()
        {
            Increment = 10,
            GameTimeStepInterval = 25,
            Offset = 255
        };
        _colorListener.Add(_rainbowWinColor, lockButton.Text);

        _lockButtonAddon = new LockButtonAddon(lockButton);
        _lockButtonAddon.Callback += Finish;

        _info1 = new TextBuilder("This button here is locked!",
            new Vector2(-160, -128));
        _info1.ChangePosition(new Vector2(-_info1.Rectangle.Width / 2F, -128));

        _info2 = new TextBuilder("The button below will unlock the button above!", Vector2.One);
        _info2.ChangePosition(new Vector2(-_info2.Rectangle.Width / 2F, -TextBuilder.DefaultLetterSize.Y / 2));
    }

    private void MagicButtonOnClick(object obj)
    {
        if (_lockButtonAddon.IsLocked)
            _lockButtonAddon.Unlock();
        else
            _lockButtonAddon.Lock();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        _mousePointer.Update(gameTime, MousePosition);
        _objectLinker.Update(gameTime);
        _cursor.Update(gameTime);

        _rainbowMagicColor.Update(gameTime);
        _rainbowWinColor.Update(gameTime);
        _colorListener.Update(gameTime);
        _magicButton.Update(gameTime, _cursor.Hitbox[0]);

        _lockButtonAddon.Update(gameTime, _cursor.Hitbox[0]);
        _info1.Update(gameTime);
        _info2.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _magicButton.Draw(spriteBatch);
        _lockButtonAddon.Draw(spriteBatch);

        _info1.Draw(spriteBatch);
        _info2.Draw(spriteBatch);

        _cursor.Draw(spriteBatch);
    }
}