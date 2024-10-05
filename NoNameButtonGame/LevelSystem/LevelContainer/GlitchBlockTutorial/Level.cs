using System;
using Microsoft.Xna.Framework;
using MonoUtils;
using MonoUtils.Logic;
using MonoUtils.Logic.Text;
using MonoUtils.Settings;
using MonoUtils.Sound;
using MonoUtils.Ui;
using MonoUtils.Ui.Logic;
using MonoUtils.Ui.TextSystem;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.Glitch;
using NoNameButtonGame.LevelSystem.Settings;
using NoNameButtonGame.Music;

namespace NoNameButtonGame.LevelSystem.LevelContainer.GlitchBlockTutorial;

internal class Level : SampleLevel
{
    public Level(Scene scene, Random random, EffectsRegistry effectsRegistry,
        SettingsAndSaveManager<string> settingsAndSaveManager) : base(scene, random, effectsRegistry,
        settingsAndSaveManager)
    {
        var textComponent = TextProvider.GetText("Levels.GlitchBlockTutorial");
        Name = textComponent.GetValue("Name");

        var blockSize = new Vector2(Camera.Rectangle.Width * 0.25F, Camera.Rectangle.Height);

        var block = new GlitchBlockCollection(blockSize, 8);
        block.ChangeColor(GlitchBlock.Color);
        block.Enter += Fail;
        block.InRectangle(Camera)
            .Move();
        AutoManaged.Add(block);

        Trap2.Play();

        OverTimeMover mover = new OverTimeMover(Camera, RightOfCamera(), 666F, OverTimeMover.MoveMode.Sin);
        AutoManaged.Add(mover);

        var text = new Text(textComponent.GetValue("Info1"));
        text.InRectangle(Camera)
            .OnX(0.625F)
            .OnY(0.25F)
            .Centered()
            .Move();
        AutoManaged.Add(text);

        var button = new Button(textComponent.GetValue("Understood"));
        button.InRectangle(Camera)
            .OnX(0.625F)
            .OnY(0.5F)
            .Centered()
            .Move();
        button.Click += delegate { mover.Start(); };
        AutoManaged.Add(button);

        blockSize.X = GlitchBlock.ImageSize.X * 2;
        block = new GlitchBlockCollection(blockSize, 8);
        block.ChangeColor(GlitchBlock.Color);
        block.Enter += Fail;
        block.InRectangle(Camera)
            .OnX(0.7F)
            .BySizeX(-0.5F)
            .ByGridX(1)
            .Move();
        AutoManaged.Add(block);

        button = new Button(textComponent.GetValue("Neat"));
        button.InRectangle(Camera)
            .OnX(0.85F)
            .OnY(0.5F)
            .ByGridX(1)
            .Centered()
            .Move();
        button.Click += delegate
        {
            if (mover.IsMoving)
                return;

            mover.ChangeDestination(RightOfCamera());
            mover.Start();
        };
        AutoManaged.Add(button);

        text = new Text(textComponent.GetValue("Info2"));
        text.InRectangle(Camera)
            .OnX(0.35F)
            .OnY(0.5F)
            .Centered()
            .ByGridX(1)
            .Move();
        AutoManaged.Add(text);

        button = new Button(textComponent.GetValue("Finish"));
        button.InRectangle(Camera)
            .OnX(0.875F)
            .OnY(0.125F)
            .ByGridX(2)
            .Centered()
            .Move();
        button.Click += Finish;
        AutoManaged.Add(button);

        text = new Text(textComponent.GetValue("Info3"));
        text.InRectangle(Camera)
            .OnX(0.33F)
            .OnY(0.2F)
            .Centered()
            .ByGridX(2)
            .Move();
        AutoManaged.Add(text);

        text = new Text(textComponent.GetValue("Info4"));
        text.InRectangle(Camera)
            .OnX(0.5F)
            .OnY(0.65F)
            .Centered()
            .ByGridX(2)
            .Move();
        AutoManaged.Add(text);

        blockSize = new Vector2(GlitchBlock.ImageSize.X * 8, GlitchBlock.ImageSize.Y * 32);
        block = new GlitchBlockCollection(blockSize, 8);
        block.ChangeColor(GlitchBlock.Color);
        block.Enter += Fail;
        block.InRectangle(Camera)
            .OnX(0.725F)
            .BySizeX(-0.5F)
            .ByGridX(2)
            .Move();
        AutoManaged.Add(block);

        blockSize = new Vector2(GlitchBlock.ImageSize.X * 40, GlitchBlock.ImageSize.Y * 8);
        var block2 = new GlitchBlockCollection(blockSize, 8);
        block2.ChangeColor(GlitchBlock.Color);
        block2.Enter += Fail;
        block2.GetAnchor(block)
            .SetMainAnchor(AnchorCalculator.Anchor.BottomRight)
            .SetSubAnchor(AnchorCalculator.Anchor.BottomLeft)
            .Move();
        AutoManaged.Add(block2);
    }

    private Vector2 RightOfCamera()
        => Camera.Position + new Vector2(Camera.Size.X, 0);
}