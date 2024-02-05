using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoUtils;
using MonoUtils.Logging;
using MonoUtils.Logic.Text;
using MonoUtils.Ui;
using MonoUtils.Ui.Objects;
using MonoUtils.Ui.Objects.TextSystem;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level6;

internal class Level : SampleLevel
{

    public Level(Display display, Vector2 window, Random random) : base(display, window, random)
    {
        var textComponent = TextProvider.GetText("Levels.Level6");

        Name = textComponent.GetValue("Name");

        Action update = delegate
        {
            if (InputReaderKeyboard.CheckKey(Keys.D))
                Camera.Move(new Vector2(Camera.Position.X + 1, Camera.Position.Y));

            Log.WriteLine(Camera.Position.ToString(), 2);

        };
        AutoManaged.Add(update);
    }
}