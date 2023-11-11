using System;
using Microsoft.Xna.Framework;
using MonoUtils.Ui;
using Levels = NoNameButtonGame.LevelSystem.LevelContainer;

namespace NoNameButtonGame.LevelSystem;

public class LevelFactory
{
    private readonly Display _display;
    private readonly Random _random;
    private readonly GameWindow _gameWindow;
    private Vector2 _screen;

    private readonly Storage.Storage _storage;

    public LevelFactory(Display display, Vector2 screen, Random random, GameWindow gameWindow, Storage.Storage storage)
    {
        _display = display;
        _screen = screen;
        _random = random;
        _gameWindow = gameWindow;
        _storage = storage;
    }

    public void ChangeScreenSize(Vector2 screen)
        => _screen = screen;

    public MainMenu.Level GetStartLevel()
        => new MainMenu.Level(_display, _screen, _random);

    public Settings.Level GetSettingsLevel()
        => new Settings.Level(_display, _screen, _random, _storage);

    public Selection.Level GetSelectLevel()
        => new Selection.Level(_display, _screen, _random, _storage);

    public SampleLevel GetLevel(int number)
        => number switch
        {
            1 => new Levels.Level1.Level(_display, _screen, _random),
            2 => new Levels.Level2.Level(_display, _screen, _random),
            3 => new Levels.Level3.Level(_display, _screen, _random),
            4 => new Levels.Level12.Level(_display, _screen, _gameWindow, _random),
            11 => new Levels.Level11.Level(_display, _screen, _random, _storage),
            _ => new Levels.Level0.Level(_display, _screen, _random)
        };

    public bool IsValidLevel(int number)
    {
        var level0 = new Levels.Level0.Level(_display, _screen, _random);
        var levelRequest = GetLevel(number);
        var isValid = levelRequest != level0;
        return isValid;
    }
}