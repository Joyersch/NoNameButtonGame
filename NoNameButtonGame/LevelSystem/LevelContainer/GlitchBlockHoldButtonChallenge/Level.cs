using System;
using Microsoft.Xna.Framework;
using MonoUtils.Logic.Text;
using MonoUtils.Ui;

namespace NoNameButtonGame.LevelSystem.LevelContainer.GlitchBlockHoldButtonChallenge;

internal class Level : SampleLevel
{
    public Level(Display display, Vector2 window, Random random) : base(display, window, random)
    {
        var textComponent = TextProvider.GetText("Levels.GlitchBlockHoldButtonChallenge");

        Name = textComponent.GetValue("Name");
    }
}