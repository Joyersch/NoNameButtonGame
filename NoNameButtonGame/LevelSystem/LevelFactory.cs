using System;
using Microsoft.Xna.Framework;
using MonoUtils.Settings;
using MonoUtils.Ui;
using Levels = NoNameButtonGame.LevelSystem.LevelContainer;

namespace NoNameButtonGame.LevelSystem;

public class LevelFactory
{
    private readonly Display _display;
    private readonly Random _random;
    private Vector2 _screen;

    private readonly SettingsAndSaveManager _settingsAndSave;
    private readonly NoNameGame _game;
    private readonly Progress _progress;

    public LevelFactory(Display display, Vector2 screen, Random random,
        SettingsAndSaveManager settingsAndSave, NoNameGame game, Progress progress)
    {
        _display = display;
        _screen = screen;
        _random = random;
        _settingsAndSave = settingsAndSave;
        _game = game;
        _progress = progress;
    }

    public void ChangeScreenSize(Vector2 screen)
        => _screen = screen;

    public MainMenu.Level GetStartLevel()
        => new (_display, _screen, _random);

    public Settings.Level GetSettingsLevel()
        => new (_display, _screen, _random, _settingsAndSave, _game);

    public Selection.Level GetSelectLevel()
        => new (_display, _screen, _random, _settingsAndSave.GetSave<Progress>());

    public SampleLevel GetLevel(int number)
        => number switch
        {
            1 => new Levels.TutorialLevel.Level(_display, _screen, _random),
            2 => new Levels.GlitchBlockTutorial.Level(_display, _screen, _random),
            3 => new Levels.ButtonGridLevel.Level(_display, _screen, _random),
            4 => new Levels.GlitchBlockHoldButtonChallenge.Level(_display, _screen, _random),
            5 => new Levels.FallingLevel.Level(_display, _screen, _random),
            6 => new Levels.RunningLevel.Level(_display, _screen, _random),
            7 => new Levels.SimonSaysLevel.Level(_display, _screen, _random),
            8 => new Levels.QuizLevel.Level(_display, _screen, _random),
            9 => new Levels.SuperGunLevel.Level(_display, _screen, _random),
            10 => new Levels.CookieClickerLevel.Level(_display, _screen, _random, _settingsAndSave),
            _ => new Levels.FallbackLevel.Level(_display, _screen, _random)
        };

    public FinishScreen.Level GetFinishScreen()
        => new (_display, _screen, _random);

    public Credits.Level GetCredits()
        => new (_display, _screen, _random);
}