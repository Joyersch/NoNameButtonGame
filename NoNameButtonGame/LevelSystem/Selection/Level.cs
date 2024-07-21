using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.Xna.Framework;
using MonoUtils.Logic;
using MonoUtils.Logic.Text;
using MonoUtils.Settings;
using MonoUtils.Sound;
using MonoUtils.Ui;
using MonoUtils.Ui.Logic;
using MonoUtils.Ui.Objects.TextSystem;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons;

namespace NoNameButtonGame.LevelSystem.Selection;

public class Level : SampleLevel
{
    public event Action<LevelFactory.LevelType, int> OnLevelSelect;

    private List<Dot> levels;

    private static class Colors
    {
        public static class Sidebar
        {
            public static Color Enabled = new Color(75, 75, 75);
            public static Color Disabled = new Color(55, 55, 55);
            public static Color Selected = new Color(125, 125, 125);
        }

        public static class Easy
        {
            public static Color Enabled = new Color(59, 252, 25);
            public static Color Disabled = new Color(25, 104, 11);
        }

        public static class Medium
        {
            public static Color Enabled = new Color(252, 245, 32);
            public static Color Disabled = new Color(104, 101, 16);
        }

        public static class Hard
        {
            public static Color Enabled = new Color(252, 155, 27);
            public static Color Disabled = new Color(102, 62, 11);
        }

        public static class Insane
        {
            public static Color Enabled = new Color(252, 102, 27);
            public static Color Disabled = new Color(109, 46, 14);
        }

        public static class Impossible
        {
            public static Color Enabled = new Color(252, 27, 27);
            public static Color Disabled = new Color(112, 18, 18);
        }
    }

    public Level(Display display, Vector2 window, Random rand, Progress progress, EffectsRegistry effectsRegistry,
        SettingsAndSaveManager settingsAndSaveManager, LevelFactory factory) : base(display, window, rand,
        effectsRegistry, settingsAndSaveManager)
    {
        var textComponent = TextProvider.GetText("Levels.Select");
        Name = textComponent.GetValue("Name");

        var selectionState = settingsAndSaveManager.GetSave<SelectionState>();


        var showcase = new Showcase(selectionState.Level, Display.SimpleScale * 50F);
        showcase.InRectangle(Display.Window)
            .OnCenter()
            .OnX(0.5F)
            .Centered()
            .Move();
        AutoManagedStaticBack.Add(showcase);

        var name = new Text(textComponent.GetValue(showcase.Level.ToString()));
        name.InRectangle(Camera.Rectangle)
            .OnCenter()
            .OnY(0.1F)
            .Centered()
            .Move();
        AutoManaged.Add(name);

        levels = new List<Dot>();
        int maxLevel = progress.MaxLevel;
        for (int i = 0; i < 10; i++)
        {
            var dot = new Dot(Vector2.Zero, Vector2.One * 20F, (i + 1).ToString())
            {
                Color = maxLevel > i ? Colors.Sidebar.Enabled : Colors.Sidebar.Disabled,
            };
            if ((int)selectionState.Level == i + 1)
                dot.Color = Colors.Sidebar.Selected;

            dot.InRectangle(Camera.Rectangle)
                .OnX(0.1F)
                .OnY(0.2175F + 0.0625F * i)
                .Centered()
                .Move();
            levels.Add(dot);

            var mat = new MouseActionsMat(dot);
            mat.Click += delegate(object obj)
            {
                var clicked = (Dot)obj;
                if (clicked.Color == Colors.Sidebar.Disabled)
                    return;

                levels.First(l => l.Color == Colors.Sidebar.Selected).Color = Colors.Sidebar.Enabled;
                clicked.Color = Colors.Sidebar.Selected;

                var level = (LevelFactory.LevelType)int.Parse(clicked.Identifier);
                name.ChangeText(textComponent.GetValue(level.ToString()));
                selectionState.Level = level;
                showcase.ChangeLevel(level);
                settingsAndSaveManager.SaveSave();
            };
            AutoManaged.Add(mat);
            AutoManaged.Add(dot);
        }


        var button = new Button(textComponent.GetValue("Play"), 3F);
        button.InRectangle(Camera.Rectangle)
            .OnX(0.9F)
            .OnY(0.9F)
            .Centered()
            .Move();
        button.Click += delegate
        {
            settingsAndSaveManager.SaveSave();
            OnLevelSelect?.Invoke(showcase.Level, ResolveDifficulty(selectionState.Difficulty));
        };
        AutoManaged.Add(button);

        var down = new MiniButton("[down]");
        down.InRectangle(Camera.Rectangle)
            .OnX(0.1F)
            .OnY(0.9F)
            .Centered()
            .Move();
        down.Click += delegate
        {
            int newLevel = (int)showcase.Level + 1;
            var level = LevelFactory.ParseLevelType(newLevel);
            if (level is LevelFactory.LevelType.Fallback || (int)level > maxLevel)
                return;
            showcase.ChangeLevel(level);
            name.ChangeText(textComponent.GetValue(level.ToString()));
            levels.First(l => l.Color == Colors.Sidebar.Selected).Color = Colors.Sidebar.Enabled;
            levels[newLevel - 1].Color = Colors.Sidebar.Selected;
            selectionState.Level = level;
            settingsAndSaveManager.SaveSave();
        };
        AutoManaged.Add(down);

        var up = new MiniButton("[up]");
        up.InRectangle(Camera.Rectangle)
            .OnX(0.1F)
            .OnY(1.1F)
            .ByGridY(-1F)
            .Centered()
            .Move();
        up.Click += delegate
        {
            int newLevel = (int)showcase.Level - 1;
            var level = LevelFactory.ParseLevelType(newLevel);
            if (level is LevelFactory.LevelType.Fallback)
                return;
            showcase.ChangeLevel(level);
            name.ChangeText(textComponent.GetValue(level.ToString()));
            levels.First(l => l.Color == Colors.Sidebar.Selected).Color = Colors.Sidebar.Enabled;
            levels[newLevel - 1].Color = Colors.Sidebar.Selected;
            selectionState.Level = level;
            settingsAndSaveManager.SaveSave();
        };
        AutoManaged.Add(up);

        var easyButton = new Button(textComponent.GetValue("Easy"), 2.5F);
        easyButton.InRectangle(Camera.Rectangle)
            .OnX(0.25F)
            .OnY(0.9F)
            .Centered()
            .Move();
        var mediumButton = new Button(textComponent.GetValue("Medium"), 2.5F);
        mediumButton.InRectangle(Camera.Rectangle)
            .OnX(0.375F)
            .OnY(0.9F)
            .Centered()
            .Move();
        var hardButton = new Button(textComponent.GetValue("Hard"), 2.5F);
        hardButton.InRectangle(Camera.Rectangle)
            .OnX(0.5F)
            .OnY(0.9F)
            .Centered()
            .Move();
        var insaneButton = new Button(textComponent.GetValue("Insane"), 2.5F);
        insaneButton.InRectangle(Camera.Rectangle)
            .OnX(0.625F)
            .OnY(0.9F)
            .Centered()
            .Move();
        var impossibleButton = new Button(textComponent.GetValue("Extreme"), 2.5F);
        impossibleButton.InRectangle(Camera.Rectangle)
            .OnX(0.75F)
            .OnY(0.9F)
            .Centered()
            .Move();

        void ResetButton()
        {
            easyButton.Text.ChangeColor(Colors.Easy.Disabled);
            mediumButton.Text.ChangeColor(Colors.Medium.Disabled);
            hardButton.Text.ChangeColor(Colors.Hard.Disabled);
            insaneButton.Text.ChangeColor(Colors.Insane.Disabled);
            impossibleButton.Text.ChangeColor(Colors.Impossible.Disabled);
        }

        easyButton.Click += delegate
        {
            selectionState.Difficulty = Difficulty.Easy;
            ResetButton();
            easyButton.Text.ChangeColor(Colors.Easy.Enabled);
            settingsAndSaveManager.SaveSave();
        };

        mediumButton.Click += delegate
        {
            selectionState.Difficulty = Difficulty.Medium;
            ResetButton();
            mediumButton.Text.ChangeColor(Colors.Medium.Enabled);
            settingsAndSaveManager.SaveSave();
        };

        hardButton.Click += delegate
        {
            selectionState.Difficulty = Difficulty.Hard;
            ResetButton();
            hardButton.Text.ChangeColor(Colors.Hard.Enabled);
            settingsAndSaveManager.SaveSave();
        };

        insaneButton.Click += delegate
        {
            selectionState.Difficulty = Difficulty.Insane;
            ResetButton();
            insaneButton.Text.ChangeColor(Colors.Insane.Enabled);
            settingsAndSaveManager.SaveSave();
        };

        impossibleButton.Click += delegate
        {
            selectionState.Difficulty = Difficulty.Impossible;
            ResetButton();
            impossibleButton.Text.ChangeColor(Colors.Impossible.Enabled);
            settingsAndSaveManager.SaveSave();
        };

        ResetButton();

        switch (selectionState.Difficulty)
        {
            case Difficulty.Easy:
                easyButton.Text.ChangeColor(Colors.Easy.Enabled);
                break;
            case Difficulty.Medium:
                mediumButton.Text.ChangeColor(Colors.Medium.Enabled);
                break;
            case Difficulty.Hard:
                hardButton.Text.ChangeColor(Colors.Hard.Enabled);
                break;
            case Difficulty.Insane:
                insaneButton.Text.ChangeColor(Colors.Insane.Enabled);
                break;
            case Difficulty.Impossible:
                impossibleButton.Text.ChangeColor(Colors.Impossible.Enabled);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        AutoManaged.Add(easyButton);
        AutoManaged.Add(mediumButton);
        AutoManaged.Add(hardButton);
        AutoManaged.Add(insaneButton);
        AutoManaged.Add(impossibleButton);
    }

    public static int ResolveDifficulty(Difficulty difficulty)
        => difficulty switch
        {
            Difficulty.Easy => 1,
            Difficulty.Medium => 400,
            Difficulty.Hard => 600,
            Difficulty.Insane => 800,
            Difficulty.Impossible => 950,
            _ => 1
        };
}