using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoUtils;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Logic.Text;
using MonoUtils.Ui;
using MonoUtils.Ui.Objects;
using MonoUtils.Ui.Objects.TextSystem;
using NoNameButtonGame.GameObjects.Glitch;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level6;

internal class Level : SampleLevel
{
    private GlitchBlockCollection _block;
    private DelayedText _text;
    private int _interactions;
    private bool _interactionsCutscenePlayed;


    public Level(Display display, Vector2 window, Random random) : base(display, window, random)
    {
        var textComponent = TextProvider.GetText("Levels.Level6");

        Name = textComponent.GetValue("Name");

        _text = new DelayedText(textComponent.GetValue("FirstText"), false)
        {
            StartAfter = 1111F,
            DisplayDelay = 32

        };
        _text.Start();
        _text.GetCalculator(Camera.Rectangle)
            .OnCenter()
            .Centered()
            .Move();

        var delayedSpawn = new OverTimeInvoker(2000, false)
        {
            InvokeOnce = true
        };
        AutoManaged.Add(delayedSpawn);

        var delayedFinish = new OverTimeInvoker(2000, false);
        delayedFinish.Trigger += Finish;
        AutoManaged.Add(delayedFinish);

        _text.FinishedPlaying += delayedSpawn.Start;

        delayedSpawn.Trigger += delegate
        {
            _text = null;
            _block = new GlitchBlockCollection(GlitchBlock.DefaultSize * 2);
            _block.ChangeColor(GlitchBlock.Color);
            _block.Enter += delegate
            {
                Mouse.SetMousePointerPositionToCenter();
                _interactions++;
            };
            _block.GetCalculator(Camera.Rectangle)
                .OnX(0.333F)
                .OnY(0.5F)
                .Centered()
                .Move();
        };

        void IncrementCheck()
        {
            if (_interactions != 5 || _interactionsCutscenePlayed)
                return;
            _interactionsCutscenePlayed = true;
            _text = new DelayedText(textComponent.GetValue("SecondText"))
            {
                DisplayDelay = 32
            };
            _text.GetCalculator(Camera.Rectangle)
                .OnX(0.5F)
                .OnY(0.333F)
                .Centered()
                .Move();
            _text.FinishedPlaying += delegate
            {
                delayedFinish.Start();
            };
        }

        AutoManaged.Add((Action)IncrementCheck);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        _text?.Update(gameTime);
        _block?.Update(gameTime);
        _block?.UpdateInteraction(gameTime, Cursor);
        Log.WriteInformation(_interactions.ToString());
    }

    protected override void Draw(SpriteBatch spriteBatch)
    {
        _block?.Draw(spriteBatch);
        _text?.Draw(spriteBatch);
        base.Draw(spriteBatch);
    }
}