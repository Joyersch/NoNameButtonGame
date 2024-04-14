using System;
using Microsoft.Xna.Framework;
using MonoUtils.Settings;
using MonoUtils.Sound;
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
    private readonly EffectsRegistry _effectsRegistry;

    public LevelFactory(Display display, Vector2 screen, Random random,
        SettingsAndSaveManager settingsAndSave, NoNameGame game, Progress progress, EffectsRegistry effectsRegistry)
    {
        _display = display;
        _screen = screen;
        _random = random;
        _settingsAndSave = settingsAndSave;
        _game = game;
        _progress = progress;
        _effectsRegistry = effectsRegistry;
    }

    public void ChangeScreenSize(Vector2 screen)
        => _screen = screen;

    public MainMenu.Level GetStartLevel(bool pan)
        => new(_display, _screen, _random, _progress, _effectsRegistry, MaxLevel(), pan);

    public Settings.Level GetSettingsLevel()
        => new(_display, _screen, _random, _settingsAndSave, _game, _effectsRegistry);

    public Selection.Level GetSelectLevel()
        => new(_display, _screen, _random, _settingsAndSave.GetSave<Progress>(), _effectsRegistry);

    public FinishScreen.Level GetFinishScreen()
        => new(_display, _screen, _random, _effectsRegistry);

    public Credits.Level GetCredits()
        => new(_display, _screen, _random, _effectsRegistry);

    public Endless.Level GetEndless()
        => new(_display, _screen, _random, _effectsRegistry, _settingsAndSave);

    public SampleLevel GetLevel(int number, int difficulty = 1)
        => number switch
        {
            1 => new Levels.TutorialLevel.Level(_display, _screen, _random, _effectsRegistry),
            2 => new Levels.GlitchBlockTutorial.Level(_display, _screen, _random, _effectsRegistry),
            3 => new Levels.ButtonGridLevel.Level(_display, _screen, _random, _effectsRegistry, difficulty),
            4 => new Levels.GlitchBlockHoldButtonChallenge.Level(_display, _screen, _random, _effectsRegistry, difficulty),
            5 => new Levels.FallingLevel.Level(_display, _screen, _random, _effectsRegistry, difficulty),
            6 => new Levels.RunningLevel.Level(_display, _screen, _random, _effectsRegistry, difficulty),
            7 => new Levels.SimonSaysLevel.Level(_display, _screen, _random, _effectsRegistry, difficulty),
            8 => new Levels.QuizLevel.Level(_display, _screen, _random, _effectsRegistry), // ToDo: Difficulty
            9 => new Levels.SuperGunLevel.Level(_display, _screen, _random, _effectsRegistry), // ToDo: Difficulty
            10 => new Levels.CookieClickerLevel.Level(_display, _screen, _random, _settingsAndSave, _effectsRegistry),
            _ => new Levels.FallbackLevel.Level(_display, _screen, _random, _effectsRegistry)
        };

    public int MaxLevel()
        => 10;

    public int GetRandomDifficultyLevelId()
        => _random.Next(3, 9);
}