using System;
using Microsoft.Xna.Framework;
using MonoUtils.Logic;
using MonoUtils.Logic.Text;
using MonoUtils.Ui;
using MonoUtils.Ui.Objects.TextSystem;
using NoNameButtonGame.GameObjects.Glitch;

namespace NoNameButtonGame.LevelSystem.LevelContainer.GlitchBlockTutorial;

internal class Level : SampleLevel
{
    private GlitchBlockCollection _block;
    private int _interactions;

    public Level(Display display, Vector2 window, Random random) : base(display, window, random)
    {

        var textComponent = TextProvider.GetText("Levels.GlitchBlockTutorial");
        Name = textComponent.GetValue("Name");

        _block = new GlitchBlockCollection(GlitchBlock.ImageSize * 8, 4);
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

        AutoManaged.Add(_block);

        var text = new Text(textComponent.GetValue("Info1"));
        text.GetCalculator(Camera.Rectangle).OnX(0.666F)
            .OnY(0.5F)
            .Centered()
            .Move();

        AutoManaged.Add((Action)IncrementCheck);
    }

    private void IncrementCheck()
    {
        if (_interactions == 3)
        {
            Finish();
        }
    }
}