using System;
using Microsoft.Xna.Framework;
using NoNameButtonGame.LevelSystem.LevelContainer;

namespace NoNameButtonGame.LevelSystem;

public class LevelFactory
{
    private readonly int _width;
    private readonly int _height;
    private readonly Vector2 _screen;
    private readonly Random _random;
    
    public LevelFactory(int width, int height, Vector2 screen, Random random)
    {
        _width = width;
        _height = height;
        _screen = screen;
        _random = random;
    }

    public StartScreen GetStartLevel()
        => new StartScreen(_width, _height, _screen, _random);
    
    public SettingsScreen GetSettingsLevel(Storage.Storage storage)
        => new SettingsScreen(_width, _height, _screen, _random, storage);

    public LevelSelect GetSelectLevel(Storage.Storage storage)
        => new LevelSelect(_width, _height, _screen, _random, storage);

    public SampleLevel GetLevel(int number)
        => number switch
        {
            1 => new Level1(_width, _height, _screen, _random),
            2 => new Level2(_width, _height, _screen, _random),
            3 => new Level3(_width, _height, _screen, _random),
            4 => new Level4(_width, _height, _screen, _random),
            _ => new LevelNull(_width, _height, _screen, _random),
        };
}