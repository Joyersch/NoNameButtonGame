using System;
using Microsoft.Xna.Framework;
using MonoUtils.Logic;
using MonoUtils.Logic.Text;
using MonoUtils.Settings;
using MonoUtils.Sound;
using MonoUtils.Ui;
using MonoUtils.Ui.Logic;
using MonoUtils.Ui.TextSystem;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.Glitch;
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

        PositionCalculator positionCalculator;

        var blockSize = new Vector2(Camera.Rectangle.Width * 0.25F, Camera.Rectangle.Height);
        var singleScale = 8 * Display.Scale;

        var block = new GlitchBlockCollection(blockSize, singleScale);
        block.ChangeColor(GlitchBlock.Color);
        block.Enter += Fail;
        AutoManaged.Add(block);

        positionCalculator = block.InRectangle(Camera)
            .OnX(0)
            .OnY(0);
        CalculatorCollection.Register(positionCalculator);

        Trap2.Play();

        OverTimeMover mover = new OverTimeMover(Camera, RightOfCamera(), 666F, OverTimeMover.MoveMode.Sin);
        AutoManaged.Add(mover);


        var text = new Text(textComponent.GetValue("Info1"));
        AutoManaged.Add(text);
        DynamicScaler.Register(text);

        positionCalculator = text.InRectangle(Camera)
            .OnX(0.625F)
            .OnY(0.25F)
            .Centered();
        CalculatorCollection.Register(positionCalculator);


        var button = new Button(textComponent.GetValue("Understood"));
        button.Click += delegate { mover.Start(); };
        AutoManaged.Add(button);
        DynamicScaler.Register(button);

        positionCalculator = button.InRectangle(Camera)
            .OnX(0.625F)
            .OnY(0.5F)
            .Centered();
        CalculatorCollection.Register(positionCalculator);

        blockSize.X = GlitchBlock.ImageSize.X * 2 * Display.Scale;
        block = new GlitchBlockCollection(blockSize, singleScale);
        block.ChangeColor(GlitchBlock.Color);
        block.Enter += Fail;
        AutoManaged.Add(block);

        positionCalculator = block.InRectangle(Camera)
            .OnX(0.7F)
            .OnY(0)
            .BySizeX(-0.5F)
            .ByGridX(1);
        CalculatorCollection.Register(positionCalculator);

        button = new Button(textComponent.GetValue("Neat"));
        button.Click += delegate
        {
            if (mover.IsMoving)
                return;

            mover.ChangeDestination(RightOfCamera());
            mover.Start();
        };
        AutoManaged.Add(button);
        DynamicScaler.Register(button);

        positionCalculator = button.InRectangle(Camera)
            .OnX(0.85F)
            .OnY(0.5F)
            .ByGridX(1)
            .Centered();
        CalculatorCollection.Register(positionCalculator);

        text = new Text(textComponent.GetValue("Info2"));
        AutoManaged.Add(text);
        DynamicScaler.Register(text);

        positionCalculator = text.InRectangle(Camera)
            .OnX(0.35F)
            .OnY(0.5F)
            .Centered()
            .ByGridX(1);
        CalculatorCollection.Register(positionCalculator);

        button = new Button(textComponent.GetValue("Finish"));
        button.Click += Finish;
        AutoManaged.Add(button);
        DynamicScaler.Register(button);

        positionCalculator = button.InRectangle(Camera)
            .OnX(0.875F)
            .OnY(0.125F)
            .ByGridX(2)
            .Centered();
        CalculatorCollection.Register(positionCalculator);

        text = new Text(textComponent.GetValue("Info3"));
        AutoManaged.Add(text);
        DynamicScaler.Register(text);

        positionCalculator = text.InRectangle(Camera)
            .OnX(0.33F)
            .OnY(0.2F)
            .Centered()
            .ByGridX(2);
        CalculatorCollection.Register(positionCalculator);

        text = new Text(textComponent.GetValue("Info4"));
        AutoManaged.Add(text);
        DynamicScaler.Register(text);

        positionCalculator = text.InRectangle(Camera)
            .OnX(0.5F)
            .OnY(0.65F)
            .Centered()
            .ByGridX(2);
        CalculatorCollection.Register(positionCalculator);


        blockSize = new Vector2(GlitchBlock.ImageSize.X * 8 * Display.Scale, GlitchBlock.ImageSize.Y * 32 * Display.Scale);
        block = new GlitchBlockCollection(blockSize, singleScale);
        block.ChangeColor(GlitchBlock.Color);
        block.Enter += Fail;
        AutoManaged.Add(block);

        positionCalculator = block.InRectangle(Camera)
            .OnX(0.725F)
            .OnY(0)
            .BySizeX(-0.5F)
            .ByGridX(2);
        CalculatorCollection.Register(positionCalculator);

        blockSize = new Vector2(GlitchBlock.ImageSize.X * 40 * Display.Scale, GlitchBlock.ImageSize.Y * 8 * Display.Scale);
        var block2 = new GlitchBlockCollection(blockSize, singleScale);
        block2.ChangeColor(GlitchBlock.Color);
        block2.Enter += Fail;
        AutoManaged.Add(block2);

        var anchorCalculator = block2.GetAnchor(block)
            .SetMainAnchor(AnchorCalculator.Anchor.BottomRight)
            .SetSubAnchor(AnchorCalculator.Anchor.BottomLeft);
        CalculatorCollection.Register(anchorCalculator);

        DynamicScaler.Apply(Display.Scale);
        CalculatorCollection.Apply();
    }

    private Vector2 RightOfCamera()
        => Camera.Position + new Vector2(Camera.Size.X, 0);
}