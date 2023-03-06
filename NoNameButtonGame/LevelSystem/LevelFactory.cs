using System;
using Microsoft.Xna.Framework;
using NoNameButtonGame.LevelSystem.LevelContainer;

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

    public StartScreen GetStartLevel()
        => new StartScreen(_display, _screen, _random);
    
    public SettingsScreen GetSettingsLevel()
        => new SettingsScreen(_display, _screen, _random, _storage);

    public LevelSelect GetSelectLevel()
        => new LevelSelect(_display, _screen, _random, _storage);

    public SampleLevel GetLevel(int number)
        => number switch
        {
            1 => new Level1(_display, _screen, _random),
            2 => new Level2(_display, _screen, _random),
            3 => new Level3(_display, _screen, _random),
            4 => new Level4(_display, _screen, _random),
            5 => new Level5(_display, _screen, _random),
            6 => new Level6(_display, _screen, _random, _storage),
            _ => new LevelNull(_display, _screen, _random),
        };
}