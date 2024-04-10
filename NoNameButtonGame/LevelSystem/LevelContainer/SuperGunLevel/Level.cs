using System;
using Microsoft.Xna.Framework;
using MonoUtils.Logic.Text;
using MonoUtils.Sound;
using MonoUtils.Ui;

namespace NoNameButtonGame.LevelSystem.LevelContainer.SuperGunLevel;

internal class Level : SampleLevel
{
    public Level(Display display, Vector2 window, Random random, EffectsRegistry effectsRegistry) : base(display, window, random, effectsRegistry)
    {
        var textComponent = TextProvider.GetText("Levels.SuperGunLevel");

        Name = textComponent.GetValue("Name");
    }
}