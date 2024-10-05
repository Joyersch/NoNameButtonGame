using System;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoUtils;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Logic.Text;
using MonoUtils.Settings;
using MonoUtils.Sound;
using MonoUtils.Ui;
using MonoUtils.Ui.TextSystem;
using NoNameButtonGame.Colors;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.LevelSystem.Settings;
using NoNameButtonGame.Music;

namespace NoNameButtonGame.LevelSystem.LevelContainer.ButtonGridLevel;

public class Level : SampleLevel
{
    private Timer _timer;

    private PositionCalculator _timerPosition;

    public Level(Scene scene, Random random, EffectsRegistry effectsRegistry,
        SettingsAndSaveManager<string> settingsAndSaveManager, int difficulty = 1) : base(scene, random,
        effectsRegistry, settingsAndSaveManager)
    {
        var textComponent = TextProvider.GetText("Levels.ButtonGridLevel");

        Name = textComponent.GetValue("Name");

        Trap.Play();

        AnchorCalculator anchorCalculator = null;
        PositionCalculator positionCalculator = null;

        ColorComponentRepository repository = new ColorComponentRepository(random);
        ColorComponent[] colors = repository.GetByColorDistance(difficulty).ToArray();
        ColorComponentShuffler shuffler = new(colors, random);

        shuffler.Shuffle();

        int selectedColor = random.Next(0, 16);
        bool useText = random.Next() % 2 == 0;

        if (difficulty > 75)
            useText = false;

        int usedText = shuffler.ResolveText(selectedColor);
        int usedColor = shuffler.ResolveColor(selectedColor);

        string infoText = textComponent.GetValue("Info");
        string t = textComponent.GetValue("Text");
        string c = textComponent.GetValue("Color");

        string infoMessage = string.Format(infoText, useText ? t : c, shuffler.GetText(usedText));
        var used = useText ? usedText : usedColor;
        var color = shuffler.GetColor(used);
        var message = $"X:{used % 4 + 1} Y: {(used - used % 4) / 4 + 1} ";

        // Log.WriteColor(message, Enumerable.Repeat(color, message.Length).ToArray());

        var info = new Text(infoMessage);
        AutoManaged.Add(info);
        DynamicScaler.Register(info);

        positionCalculator = info.InRectangle(Camera)
            .OnCenter()
            .OnY(2, 20)
            .Centered();
        CalculatorCollection.Register(positionCalculator);

        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                int index = x + y * 4;
                string text = shuffler.GetText(index);
                var button = new Button(text);
                button.Text.ChangeColor(shuffler.GetColor(index));
                if (button.Text.Rectangle.Width > button.Rectangle.Width)
                {
                    button = new Button(text, textScale: 0.75F);
                    button.Text.ChangeColor(shuffler.GetColor(index));

                    if (button.Text.Rectangle.Width > button.Rectangle.Width)
                    {
                        button = new Button(text, textScale: 0.5F);
                        button.Text.ChangeColor(shuffler.GetColor(index));
                    }
                }

                if (index == (useText ? usedText : usedColor))
                    button.Click += Finish;
                else
                    button.Click += Fail;
                AutoManaged.Add(button);
                DynamicScaler.Register(button);

                positionCalculator = button.InRectangle(Camera)
                    .OnX(x * 4 + 4, 20)
                    .OnY(y * 4 + 5, 20)
                    .Centered();
                CalculatorCollection.Register(positionCalculator);
            }
        }

        _timer = new Timer(1F, 15000D, true);
        _timer.Trigger += Fail;
        AutoManaged.Add(_timer);
        DynamicScaler.Register(_timer);

        positionCalculator = _timer.InRectangle(Camera)
            .OnX(0.1F)
            .OnY(0.1F);
        CalculatorCollection.Register(positionCalculator);
        _timerPosition = positionCalculator;

        PulsatingRed timerColor = new PulsatingRed()
        {
            GameTimeStepInterval = 32,
            NoGradient = false
        };
        AutoManaged.Add(timerColor);
        ColorListener.Add(timerColor, _timer);

        DynamicScaler.Apply(Display.Scale);
        CalculatorCollection.Apply();
    }

    public override void Update(GameTime gameTime)
    {
        _timerPosition.Apply();
        base.Update(gameTime);
    }
}