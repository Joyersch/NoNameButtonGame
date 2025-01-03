﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Logic.Management;
using MonoUtils.Logic.Text;
using MonoUtils.Settings;
using MonoUtils.Sound;
using MonoUtils.Ui;
using MonoUtils.Ui.Logic;
using MonoUtils.Ui.TextSystem;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.LevelSystem.Selection.Progress;
using NoNameButtonGame.Music;

namespace NoNameButtonGame.LevelSystem.Selection;

public class Level : SampleLevel
{
    public event Action<LevelFactory.LevelType, int> OnLevelSelect;

    private List<Blank> _levels;

    private List<ManagementCollection> _levelStats;

    private readonly Button _easyButton;
    private readonly Button _mediumButton;
    private readonly Button _hardButton;
    private readonly Button _insaneButton;
    private readonly Button _extremeButton;

    private int _selectedLevel;
    private PositionCalculator _titlePosition;

    private static class Colors
    {
        public static class Sidebar
        {
            public static Color Enabled = new(75, 75, 75);
            public static Color Disabled = new(55, 55, 55);
            public static Color Selected = new(125, 125, 125);
        }

        public static class Easy
        {
            public static Color Enabled = new(59, 252, 25);
            public static Color Disabled = new(25, 104, 11);
        }

        public static class Medium
        {
            public static Color Enabled = new(252, 245, 32);
            public static Color Disabled = new(104, 101, 16);
        }

        public static class Hard
        {
            public static Color Enabled = new(252, 155, 27);
            public static Color Disabled = new(102, 62, 11);
        }

        public static class Insane
        {
            public static Color Enabled = new(252, 102, 27);
            public static Color Disabled = new(109, 46, 14);
        }

        public static class Extreme
        {
            public static Color Enabled = new(252, 27, 27);
            public static Color Disabled = new(112, 18, 18);
        }
    }

    public Level(Scene scene, Random rand, LevelSystem.Progress progress,
        EffectsRegistry effectsRegistry, SettingsAndSaveManager<string> settingsAndSaveManager,
        LevelFactory factory) : base(scene, rand, effectsRegistry, settingsAndSaveManager)
    {
        var textComponent = TextProvider.GetText("Levels.Select");
        Name = textComponent.GetValue("Name");

        Default.Play();

        var selectionState = settingsAndSaveManager.GetSave<SelectionState>();
        var selectProgress = settingsAndSaveManager.GetSave<Save>();

        _levelStats = new List<ManagementCollection>();

        AnchorCalculator anchorCalculator = null;
        PositionCalculator positionCalculator = null;

        var showcase = new Showcase(selectionState.Level, Display.Scale * 50F);
        AutoManaged.Add(showcase);

        positionCalculator = showcase.InRectangle(Camera)
            .OnCenter()
            .Centered();
        CalculatorCollection.Register(positionCalculator);

        var name = new Text(textComponent.GetValue(showcase.Level.ToString()));
        AutoManaged.Add(name);
        DynamicScaler.Register(name);

        positionCalculator = name.InRectangle(Camera)
            .OnCenter()
            .OnY(0.1F)
            .Centered();
        CalculatorCollection.Register(positionCalculator);
        _titlePosition = positionCalculator;

        _levels = new List<Blank>();
        int maxLevel = progress.MaxLevel;

        void easyButtonClick()
        {
            selectionState.Difficulty = Difficulty.Easy;
            ResetButton();
            _easyButton.Text.ChangeColor(Colors.Easy.Enabled);
            settingsAndSaveManager.SaveSave();
        }

        _selectedLevel = (int)selectionState.Level - 1;

        for (int i = 0; i < 10; i++)
        {
            var dot = new Blank(Vector2.Zero, Vector2.One * 40F, (i + 1).ToString())
            {
                Color = maxLevel > i ? Colors.Sidebar.Enabled : Colors.Sidebar.Disabled,
            };
            DynamicScaler.Register(dot);

            if ((int)selectionState.Level == i + 1)
                dot.Color = Colors.Sidebar.Selected;

            _levels.Add(dot);

            positionCalculator = dot.InRectangle(Camera)
                .OnX(0.1F)
                .OnY(0.2175F + 0.0625F * i)
                .Centered();
            CalculatorCollection.Register(positionCalculator);

            var mat = new MouseActionsMat(dot);
            mat.Click += delegate(object obj)
            {
                var clicked = (Blank)obj;
                if (clicked.Color == Colors.Sidebar.Disabled)
                    return;

                _levels.First(l => l.Color == Colors.Sidebar.Selected).Color = Colors.Sidebar.Enabled;
                clicked.Color = Colors.Sidebar.Selected;

                var level = (LevelFactory.LevelType)int.Parse(clicked.Identifier);
                name.ChangeText(textComponent.GetValue(level.ToString()));
                selectionState.Level = level;
                showcase.ChangeLevel(level);
                _selectedLevel = (int)level - 1;
                selectionState.Level = level;
                if (!LevelFactory.HasLevelDifficulty(level))
                    easyButtonClick();
                settingsAndSaveManager.SaveSave();
                _titlePosition.Apply();
            };
            AutoManaged.Add(mat);
            AutoManaged.Add(dot);
        }


        var button = new Button(textComponent.GetValue("Play"), 6F);
        button.Click += delegate
        {
            settingsAndSaveManager.SaveSave();
            OnLevelSelect?.Invoke(showcase.Level, ResolveDifficulty(selectionState.Difficulty));
        };
        AutoManaged.Add(button);
        DynamicScaler.Register(button);

        positionCalculator = button.InRectangle(Camera)
            .OnX(0.9F)
            .OnY(0.9F)
            .Centered();
        CalculatorCollection.Register(positionCalculator);

        var down = new MiniButton("[down]");
        down.Click += delegate
        {
            int newLevel = (int)showcase.Level + 1;
            var level = LevelFactory.ParseLevelType(newLevel);
            if (level is LevelFactory.LevelType.Fallback || (int)level > maxLevel)
                return;
            showcase.ChangeLevel(level);
            name.ChangeText(textComponent.GetValue(level.ToString()));
            _levels.First(l => l.Color == Colors.Sidebar.Selected).Color = Colors.Sidebar.Enabled;
            _levels[newLevel - 1].Color = Colors.Sidebar.Selected;
            _selectedLevel = (int)level - 1;
            selectionState.Level = level;
            if (!LevelFactory.HasLevelDifficulty(level))
                easyButtonClick();
            settingsAndSaveManager.SaveSave();
            _titlePosition.Apply();
        };
        AutoManaged.Add(down);
        DynamicScaler.Register(down);

        positionCalculator = down.InRectangle(Camera)
            .OnX(0.1F)
            .OnY(0.9F)
            .Centered();
        CalculatorCollection.Register(positionCalculator);

        var up = new MiniButton("[up]");
        up.Click += delegate
        {
            int newLevel = (int)showcase.Level - 1;
            var level = LevelFactory.ParseLevelType(newLevel);
            if (level is LevelFactory.LevelType.Fallback)
                return;
            showcase.ChangeLevel(level);
            name.ChangeText(textComponent.GetValue(level.ToString()));
            _levels.First(l => l.Color == Colors.Sidebar.Selected).Color = Colors.Sidebar.Enabled;
            _levels[newLevel - 1].Color = Colors.Sidebar.Selected;
            _selectedLevel = (int)level - 1;
            selectionState.Level = level;
            if (!LevelFactory.HasLevelDifficulty(level))
                easyButtonClick();
            settingsAndSaveManager.SaveSave();
            _titlePosition.Apply();
        };
        AutoManaged.Add(up);
        DynamicScaler.Register(up);

        positionCalculator = up.InRectangle(Camera)
            .OnX(0.1F)
            .OnY(1.1F)
            .ByGridY(-1F)
            .Centered();
        CalculatorCollection.Register(positionCalculator);

        _easyButton = new Button(textComponent.GetValue("Easy"), 5F);
        DynamicScaler.Register(_easyButton);

        positionCalculator = _easyButton.InRectangle(Camera)
            .OnX(0.25F)
            .OnY(0.9F)
            .Centered();
        CalculatorCollection.Register(positionCalculator);

        _mediumButton = new Button(textComponent.GetValue("Medium"), 5F);
        DynamicScaler.Register(_mediumButton);

        positionCalculator = _mediumButton.InRectangle(Camera)
            .OnX(0.375F)
            .OnY(0.9F)
            .Centered();
        CalculatorCollection.Register(positionCalculator);

        _hardButton = new Button(textComponent.GetValue("Hard"), 5F);
        DynamicScaler.Register(_hardButton);

        positionCalculator = _hardButton.InRectangle(Camera)
            .OnX(0.5F)
            .OnY(0.9F)
            .Centered();
        CalculatorCollection.Register(positionCalculator);

        _insaneButton = new Button(textComponent.GetValue("Insane"), 5F);
        DynamicScaler.Register(_insaneButton);

        positionCalculator = _insaneButton.InRectangle(Camera)
            .OnX(0.625F)
            .OnY(0.9F)
            .Centered();
        CalculatorCollection.Register(positionCalculator);

        _extremeButton = new Button(textComponent.GetValue("Extreme"), 5F);
        DynamicScaler.Register(_extremeButton);

        positionCalculator = _extremeButton.InRectangle(Camera)
            .OnX(0.75F)
            .OnY(0.9F)
            .Centered();
        CalculatorCollection.Register(positionCalculator);

        void ResetButton()
        {
            _easyButton.Text.ChangeColor(Colors.Easy.Disabled);
            _mediumButton.Text.ChangeColor(Colors.Medium.Disabled);
            _hardButton.Text.ChangeColor(Colors.Hard.Disabled);
            _insaneButton.Text.ChangeColor(Colors.Insane.Disabled);
            _extremeButton.Text.ChangeColor(Colors.Extreme.Disabled);
        }

        _easyButton.Click += _ => easyButtonClick();

        _mediumButton.Click += delegate
        {
            selectionState.Difficulty = Difficulty.Medium;
            ResetButton();
            _mediumButton.Text.ChangeColor(Colors.Medium.Enabled);
            settingsAndSaveManager.SaveSave();
        };

        _hardButton.Click += delegate
        {
            selectionState.Difficulty = Difficulty.Hard;
            ResetButton();
            _hardButton.Text.ChangeColor(Colors.Hard.Enabled);
            settingsAndSaveManager.SaveSave();
        };

        _insaneButton.Click += delegate
        {
            selectionState.Difficulty = Difficulty.Insane;
            ResetButton();
            _insaneButton.Text.ChangeColor(Colors.Insane.Enabled);
            settingsAndSaveManager.SaveSave();
        };

        _extremeButton.Click += delegate
        {
            selectionState.Difficulty = Difficulty.Extreme;
            ResetButton();
            _extremeButton.Text.ChangeColor(Colors.Extreme.Enabled);
            settingsAndSaveManager.SaveSave();
        };

        ResetButton();

        switch (selectionState.Difficulty)
        {
            case Difficulty.Easy:
                _easyButton.Text.ChangeColor(Colors.Easy.Enabled);
                break;
            case Difficulty.Medium:
                _mediumButton.Text.ChangeColor(Colors.Medium.Enabled);
                break;
            case Difficulty.Hard:
                _hardButton.Text.ChangeColor(Colors.Hard.Enabled);
                break;
            case Difficulty.Insane:
                _insaneButton.Text.ChangeColor(Colors.Insane.Enabled);
                break;
            case Difficulty.Extreme:
                _extremeButton.Text.ChangeColor(Colors.Extreme.Enabled);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        var completed = new Text(textComponent.GetValue("Completed"));
        AutoManaged.Add(completed);
        DynamicScaler.Register(completed);

        positionCalculator = completed.InRectangle(Camera)
            .OnX(0.9F)
            .OnY(0.25F)
            .Centered();
        CalculatorCollection.Register(positionCalculator);

        for (int i = 0; i < factory.MaxLevel(); i++)
        {
            var collection = new ManagementCollection();

            var text = new Text(textComponent.GetValue("Easy"));
            text.ChangeColor(selectProgress.Levels[i].BeatEasy ? Colors.Easy.Enabled : Colors.Easy.Disabled);
            collection.Add(text);
            DynamicScaler.Register(text);

            positionCalculator = text.InRectangle(Camera)
                .OnX(0.9F)
                .OnY(0.33F)
                .Centered();
            CalculatorCollection.Register(positionCalculator);

            if (!LevelFactory.HasLevelDifficulty(i + 1))
            {
                _levelStats.Add(collection);
                continue;
            }

            text = new Text(textComponent.GetValue("Medium"));
            text.ChangeColor(selectProgress.Levels[i].BeatMedium ? Colors.Medium.Enabled : Colors.Medium.Disabled);
            collection.Add(text);
            DynamicScaler.Register(text);
            positionCalculator = text.InRectangle(Camera)
                .OnX(0.9F)
                .OnY(0.4F)
                .Centered();
            CalculatorCollection.Register(positionCalculator);

            text = new Text(textComponent.GetValue("Hard"));
            text.ChangeColor(selectProgress.Levels[i].BeatHard ? Colors.Hard.Enabled : Colors.Hard.Disabled);
            collection.Add(text);
            DynamicScaler.Register(text);

            positionCalculator = text.InRectangle(Camera)
                .OnX(0.9F)
                .OnY(0.466F)
                .Centered();
            CalculatorCollection.Register(positionCalculator);

            text = new Text(textComponent.GetValue("Insane"));
            text.ChangeColor(selectProgress.Levels[i].BeatInsane ? Colors.Insane.Enabled : Colors.Insane.Disabled);
            collection.Add(text);
            DynamicScaler.Register(text);

            positionCalculator = text.InRectangle(Camera)
                .OnX(0.9F)
                .OnY(0.533F)
                .Centered();
            CalculatorCollection.Register(positionCalculator);

            text = new Text(textComponent.GetValue("Extreme"));
            text.ChangeColor(selectProgress.Levels[i].BeatExtreme ? Colors.Extreme.Enabled : Colors.Extreme.Disabled);
            collection.Add(text);
            DynamicScaler.Register(text);

            positionCalculator = text.InRectangle(Camera)
                .OnX(0.9F)
                .OnY(0.6F)
                .Centered();
            CalculatorCollection.Register(positionCalculator);

            _levelStats.Add(collection);
        }

        DynamicScaler.Apply(Display.Scale);
        CalculatorCollection.Apply();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        _easyButton.Update(gameTime);
        _easyButton.UpdateInteraction(gameTime, Cursor);

        if (LevelFactory.HasLevelDifficulty(_selectedLevel + 1))
        {
            _mediumButton.Update(gameTime);
            _mediumButton.UpdateInteraction(gameTime, Cursor);

            _hardButton.Update(gameTime);
            _hardButton.UpdateInteraction(gameTime, Cursor);

            _insaneButton.Update(gameTime);
            _insaneButton.UpdateInteraction(gameTime, Cursor);

            _extremeButton.Update(gameTime);
            _extremeButton.UpdateInteraction(gameTime, Cursor);
        }

        _levelStats[_selectedLevel].Update(gameTime);
    }

    protected override void Draw(SpriteBatch spriteBatch)
    {
        _easyButton.Draw(spriteBatch);

        if (LevelFactory.HasLevelDifficulty(_selectedLevel + 1))
        {
            _mediumButton.Draw(spriteBatch);
            _hardButton.Draw(spriteBatch);
            _insaneButton.Draw(spriteBatch);
            _extremeButton.Draw(spriteBatch);
        }

        _levelStats[_selectedLevel].Draw(spriteBatch);
        base.Draw(spriteBatch);
    }

    public static int ResolveDifficulty(Difficulty difficulty)
        => difficulty switch
        {
            Difficulty.Easy => 1,
            Difficulty.Medium => 400,
            Difficulty.Hard => 600,
            Difficulty.Insane => 800,
            Difficulty.Extreme => 950,
            _ => 1
        };

    public static Difficulty ResolveDifficulty(int difficulty)
        => difficulty switch
        {
            1 => Difficulty.Easy,
            400 => Difficulty.Medium,
            600 => Difficulty.Hard,
            800 => Difficulty.Insane,
            950 => Difficulty.Extreme,
            _ => Difficulty.Easy
        };
}