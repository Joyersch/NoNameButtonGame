using System;
using Microsoft.Xna.Framework;
using Levels = NoNameButtonGame.LevelSystem.LevelContainer;

namespace NoNameButtonGame.LevelSystem;

public class LevelFactory
{
    private readonly Display.Display _display;
    private readonly Random _random;
    private Vector2 _screen;

    private readonly Storage.Storage _storage;

    public LevelFactory(Display.Display display, Vector2 screen, Random random, Storage.Storage storage)
    {
        _display = display;
        _screen = screen;
        _random = random;
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
            4 => new Levels.Level4.Level(_display, _screen, _random),
            5 => new Levels.Level5.Level(_display, _screen, _random),
            6 => new Levels.Level6.Level(_display, _screen, _random, _storage),
            7 => new Levels.Level7.Level(_display, _screen, _random),
            _ => new Levels.Level0.Level(_display, _screen, _random),
        };
}