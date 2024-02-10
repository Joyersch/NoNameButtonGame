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
    private readonly GameWindow _gameWindow;
    private Vector2 _screen;

    private readonly SettingsAndSaveManager _settingsAndSave;
    private readonly NoNameGame _game;
    private readonly Progress _progress;

    public LevelFactory(Display display, Vector2 screen, Random random, GameWindow gameWindow,
        SettingsAndSaveManager settingsAndSave, NoNameGame game, Progress progress)
    {
        _display = display;
        _screen = screen;
        _random = random;
        _gameWindow = gameWindow;
        _settingsAndSave = settingsAndSave;
        _game = game;
        _progress = progress;
    }

    public void ChangeScreenSize(Vector2 screen)
        => _screen = screen;

    public MainMenu.Level GetStartLevel()
        => new MainMenu.Level(_display, _screen, _random, _progress.FinishedGame);

    public Settings.Level GetSettingsLevel()
        => new Settings.Level(_display, _screen, _random, _settingsAndSave, _game);

    public Selection.Level GetSelectLevel()
        => new Selection.Level(_display, _screen, _random, _settingsAndSave.GetSave<Progress>());

    public SampleLevel GetLevel(int number)
        => number switch
        {
            1 => new Levels.Level1.Level(_display, _screen, _random),
            2 => new Levels.Level2.Level(_display, _screen, _random),
            3 => new Levels.Level3.Level(_display, _screen, _random),
            4 => new Levels.Level4.Level(_display, _screen, _random),
            5 => new Levels.Level5.Level(_display, _screen, _random, _settingsAndSave),
            6 => new Levels.Level6.Level(_display, _screen, _random),
            10 => new Levels.Level10.Level(_display, _screen, _random),
            _ => new Levels.Level0.Level(_display, _screen, _random)
        };

    public bool IsValidLevel(int number)
    {
        var level0 = new Levels.Level0.Level(_display, _screen, _random);
        var levelRequest = GetLevel(number);
        var isValid = levelRequest != level0;
        return isValid;
    }

    public FinishScreen.Level GetFinishScreen()
        => new FinishScreen.Level(_display, _screen, _random);

    public Credits.Level GetCredits()
        => new Credits.Level(_display, _screen, _random);

    public Bonus.Level GetBonus()
        => new Bonus.Level(_display, _screen, _gameWindow, _random);
}