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
        => new(_display, _screen, _random, _progress, _effectsRegistry, MaxLevel(), pan, _settingsAndSave);

    public Settings.Level GetSettingsLevel()
        => new(_display, _screen, _random, _settingsAndSave, _game, _effectsRegistry);

    public Selection.Level GetSelectLevel()
        => new(_display, _screen, _random, _settingsAndSave.GetSave<Progress>(), _effectsRegistry, _settingsAndSave, this);

    public FinishScreen.Level GetFinishScreen()
        => new(_display, _screen, _random, _effectsRegistry, _settingsAndSave);

    public Credits.Level GetCredits()
        => new(_display, _screen, _random, _effectsRegistry, _settingsAndSave);

    public Endless.Level GetEndless()
        => new(_display, _screen, _random, _effectsRegistry, _settingsAndSave);

    public SampleLevel GetLevel(int number, int difficulty = 1)
        => GetLevel(ParseLevelType(number), difficulty);

    public SampleLevel GetLevel(LevelType type, int difficulty = 1)
        => type switch
        {
            LevelType.Tutorial => new Levels.TutorialLevel.Level(_display, _screen, _random, _effectsRegistry,
                _settingsAndSave),
            LevelType.ButtonGrid => new Levels.ButtonGridLevel.Level(_display, _screen, _random, _effectsRegistry,
                _settingsAndSave, difficulty),
            LevelType.SimonSays => new Levels.SimonSaysLevel.Level(_display, _screen, _random, _effectsRegistry,
                _settingsAndSave, difficulty),
            LevelType.Quiz => new Levels.QuizLevel.Level(_display, _screen, _random, _effectsRegistry,
                _settingsAndSave),
            LevelType.CookieClicker => new Levels.CookieClickerLevel.Level(_display, _screen, _random, _settingsAndSave,
                _effectsRegistry),
            LevelType.GlitchBlock => new Levels.GlitchBlockTutorial.Level(_display, _screen, _random, _effectsRegistry,
                _settingsAndSave),
            LevelType.HoldButtonChallenge => new Levels.HoldButtonChallenge.Level(_display, _screen, _random,
                _effectsRegistry, _settingsAndSave, difficulty),
            LevelType.Falling => new Levels.FallingLevel.Level(_display, _screen, _random, _effectsRegistry,
                _settingsAndSave, difficulty),
            LevelType.Running => new Levels.RunningLevel.Level(_display, _screen, _random, _effectsRegistry,
                _settingsAndSave, difficulty),
            LevelType.SuperGun => new Levels.SuperGunLevel.Level(_display, _screen, _random, _effectsRegistry,
                _settingsAndSave, difficulty),
            _ => new Levels.FallbackLevel.Level(_display, _screen, _random, _effectsRegistry, _settingsAndSave)
        };

    public static LevelType ParseLevelType(int number)
        => Enum.IsDefined(typeof(LevelType), number) ? (LevelType)number : LevelType.Fallback;

    public int MaxLevel()
        => 10;

    public int GetRandomDifficultyLevelId()
        => _random.Next(2) switch
        {
            0 => _random.Next(2, 4), /*ButtonGridLevel & SimonSaysLevel*/
            _ /*1*/ => _random.Next(7,
                11), /*GlitchBlockHoldButtonChallenge & FallingLevel & RunningLevel & SuperGunLevel*/
        };

    public enum LevelType
    {
        Fallback = 0,
        Tutorial,
        ButtonGrid,
        SimonSays,
        Quiz,
        CookieClicker,
        GlitchBlock,
        HoldButtonChallenge,
        Falling,
        Running,
        SuperGun,
    }
}