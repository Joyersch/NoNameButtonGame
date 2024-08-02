using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logging;
using MonoUtils.Settings;
using MonoUtils.Sound;
using MonoUtils.Ui;
using NoNameButtonGame.LevelSystem.Endless;
using NoNameButtonGame.LevelSystem.Selection;

namespace NoNameButtonGame.LevelSystem;

internal class LevelManager
{
    private readonly SettingsAndSaveManager<string> _settingsAndSaveManager;
    private readonly NoNameGame _game;
    private readonly Progress _progress;
    private readonly Selection.Progress.Save _selectionProgress;

    private LevelFactory _levelFactory;

    private SampleLevel _currentLevel;

    private FinishScreen.Level _finishScreen;

    private bool _onFinishScreen;
    private int _levelId;
    private LevelState _levelState;
    private int _difficulty = 1;
    // 950 / 50 => 19
    private int _difficultyStep = 19;

    public int EndlessLevel => _currentDifficulty / _difficultyStep;

    private EndlessRun _endlessRun;
    private bool _endlessEndTracking;

    public event Action CloseGame;

    public event Action<string> ChangeTitle;

    private int _currentDifficulty = 1;
    private int _currentEndlessLevelId = -1;
    private bool _starting = true;

    private enum LevelState
    {
        Menu,
        Settings,
        Credits,
        Select,
        SelectLevel,
        Level,
        Endless,
        EndlessLevel
    }

    public LevelManager(Display display, SettingsAndSaveManager<string> settingsAndSaveManager,
        NoNameGame game, EffectsRegistry effectsRegistry,
        int? seed = null)
    {
        _settingsAndSaveManager = settingsAndSaveManager;
        _game = game;
        var videoSettings = _settingsAndSaveManager.GetSetting<Settings.VideoSettings>();
        _progress = _settingsAndSaveManager.GetSave<Progress>();
        _selectionProgress = _settingsAndSaveManager.GetSave<Selection.Progress.Save>();
        _levelId = _progress.MaxLevel + 1;
        var random = new Random(seed ?? DateTime.Now.Millisecond);
        _levelFactory = new LevelFactory(display, videoSettings.Resolution.ToVector2(), random,
            settingsAndSaveManager, game, _progress, effectsRegistry);
        _finishScreen = _levelFactory.GetFinishScreen();
        _finishScreen.OnFinish += FinishScreenDisplayed;
        _levelState = LevelState.Menu;

        // In the case for people upgrading from prior versions
        if (!_progress.FinishedLevels && _progress.MaxLevel == _levelFactory.MaxLevel())
        {
            Log.Information($"Reached max level. Giving Star!");
            _progress.FinishedLevels = true;
            settingsAndSaveManager.SaveSave();
        }

        for (int i = 0; i < _progress.MaxLevel; i++)
            UpdateSelectionSave(i + 1, Selection.Level.ResolveDifficulty(Difficulty.Easy));

        var endless = settingsAndSaveManager.GetSave<EndlessProgress>();
        var challenges = settingsAndSaveManager.GetSave<Challenges>();
        challenges.Score10 = endless.HighestLevel >= 10;
        challenges.Score25 =endless.HighestLevel >= 25;
        challenges.Score50 = endless.HighestLevel >= 50;

        _settingsAndSaveManager.SaveSave();

        if (_progress.MaxLevel == 0)
        {
            _levelState = LevelState.Level;
            ChangeLevel(_progress.MaxLevel + 1, 1);
            _starting = false;
        }
        else
            ChangeLevel();
    }

    public void Update(GameTime gameTime)
    {
        if (_levelState is LevelState.EndlessLevel && _endlessRun is not null && !_endlessRun.StartedTimeTracking)
            _endlessRun.StartTimeTracking(gameTime.TotalGameTime.TotalMilliseconds);

        if (_levelState is LevelState.EndlessLevel && _endlessEndTracking && _endlessRun is not null &&
            !_endlessRun.EndedimeTracking)
        {
            _endlessRun.EndTimeTracking(gameTime.TotalGameTime.TotalMilliseconds);
            double timeInMinutes = _endlessRun.GetTime() / 1000 / 60;
            var challenges = _settingsAndSaveManager.GetSave<Challenges>();

            if (timeInMinutes <= 30)
                challenges.Time30min = true;

            if (timeInMinutes <= 60)
                challenges.Time1h = true;

            var endless = _settingsAndSaveManager.GetSave<EndlessProgress>();
            endless.BestTimeTo50 = _endlessRun.GetTime();


            if (challenges.Score10 && challenges.Score25 && challenges.Score50 && challenges.Time30min &&
                challenges.Time1h)
                _progress.FinishedEndless = true;
            _settingsAndSaveManager.SaveSave();
        }

        if (!_onFinishScreen)
        {
            _currentLevel.Update(gameTime);
            return;
        }

        _finishScreen.Update(gameTime);
    }

    public void Draw(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
    {
        if (!_onFinishScreen)
            _currentLevel.Draw(graphicsDevice, spriteBatch);
        else
            _finishScreen.Draw(graphicsDevice, spriteBatch);
    }

    public void Exit()
        => CloseGame?.Invoke();

    public void SetAsLevelSelect()
        => _levelState = LevelState.SelectLevel;

    public void ChangeLevel(int level, int difficulty)
        => ChangeLevel(LevelFactory.ParseLevelType(level), difficulty);

    public void ChangeLevel(LevelFactory.LevelType level, int difficulty)
    {
        _currentLevel = _levelFactory.GetLevel(level, difficulty);
        _levelId = (int)level;
        ChangeTitle?.Invoke(_currentLevel.Name);
        RegisterEvents();
    }

    private void ChangeLevel()
    {
        switch (_levelState)
        {
            case LevelState.Menu:
                _currentLevel = _levelFactory.GetStartLevel(_starting);
                _starting = false;
                break;
            case LevelState.Settings:
                _currentLevel = _levelFactory.GetSettingsLevel();
                break;
            case LevelState.Select:
                _currentLevel = _levelFactory.GetSelectLevel();
                break;
            case LevelState.SelectLevel:
            case LevelState.Level:
                ChangeLevel(_levelId, _difficulty);
                return;
            case LevelState.Endless:
                _currentLevel = _levelFactory.GetEndless();
                _currentEndlessLevelId = -1;
                _currentDifficulty = 2;
                break;
            case LevelState.EndlessLevel:
                if (_currentEndlessLevelId == -1)
                    _currentEndlessLevelId = _levelFactory.GetRandomDifficultyLevelId();
                _currentLevel = _levelFactory.GetLevel(_currentEndlessLevelId, _currentDifficulty);
                break;
            case LevelState.Credits:
                _currentLevel = _levelFactory.GetCredits();
                break;
        }

        ChangeTitle?.Invoke(_currentLevel.Name);
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        _currentLevel.OnFinish += LevelFinishes;
        _currentLevel.OnFail += FailLevel;

        if (_currentLevel is MainMenu.Level mainMenu)
        {
            mainMenu.OnExit += Exit;
            mainMenu.StartClicked += delegate
            {
                Log.Information($"Starting level {_levelId}");
                _levelState = LevelState.Level;
                _difficulty = 1;
                ChangeLevel(_progress.MaxLevel + 1, _difficulty);
            };

            mainMenu.SelectClicked += delegate
            {
                _levelState = LevelState.Select;
                ChangeLevel();
            };

            mainMenu.SettingsClicked += delegate
            {
                _levelState = LevelState.Settings;
                ChangeLevel();
            };

            mainMenu.CreditsClicked += delegate
            {
                _levelState = LevelState.Credits;
                ChangeLevel();
            };

            mainMenu.EndlessClicked += delegate
            {
                _levelState = LevelState.Endless;
                ChangeLevel();
            };
        }
        else if (_currentLevel is Selection.Level selectLevel)
        {
            selectLevel.OnExit += ExitLevel;
            selectLevel.OnLevelSelect += delegate(LevelFactory.LevelType level, int difficulty)
            {
                Log.Information($"Selecting level {level}");
                _levelState = LevelState.SelectLevel;
                _difficulty = difficulty;
                ChangeLevel(level, difficulty);
            };
        }
        else if (_currentLevel is Settings.Level settingsLevel)
        {
            settingsLevel.OnDiscard += delegate
            {
                _settingsAndSaveManager.LoadSettings();
                _game.ApplySettings();
                var videoSettings = _settingsAndSaveManager.GetSetting<Settings.VideoSettings>();
                _levelFactory.ChangeScreenSize(videoSettings.Resolution.ToVector2());
                _finishScreen = _levelFactory.GetFinishScreen();
                ExitLevel();
            };
            settingsLevel.OnSave += delegate
            {
                _settingsAndSaveManager.SaveSettings();
                _finishScreen = _levelFactory.GetFinishScreen();
                _finishScreen.OnFinish += FinishScreenDisplayed;
                ExitLevel();
            };
            settingsLevel.OnWindowResize += delegate(Vector2 screen) { _levelFactory.ChangeScreenSize(screen); };
            settingsLevel.OnNameChange += delegate { ChangeTitle?.Invoke(settingsLevel.Name); };
        }
        else if (_currentLevel is Endless.Level endlessLevel)
        {
            _endlessEndTracking = false;
            endlessLevel.OnExit += ExitLevel;
            endlessLevel.Selected += delegate()
            {
                _levelState = LevelState.EndlessLevel;
                _endlessRun = new EndlessRun();
                ChangeLevel();
            };
        }
        else
        {
            _currentLevel.OnExit += ExitLevel;
        }
    }

    private void LevelFinishes()
    {
        Log.Information("On finish screen");
        if (_levelState != LevelState.EndlessLevel)
        {
            _onFinishScreen = true;
            return;
        }

        FinishScreenDisplayed();
    }

    private void FinishScreenDisplayed()
    {
        _onFinishScreen = false;
        Log.Information("Finished level");
        switch (_levelState)
        {
            case LevelState.Level:
                int max = _progress.MaxLevel;
                if (_levelId > max)
                {
                    _progress.MaxLevel = _levelId;
                    Log.Information($"Updated max level value to {_levelId}");

                    if (_progress.MaxLevel == _levelFactory.MaxLevel())
                    {
                        Log.Information($"Reached max level. Giving Star!");
                        _progress.FinishedLevels = true;
                    }

                    _settingsAndSaveManager.SaveSave();
                    Log.Information("Saved progress!");
                }

                if (_levelId == _levelFactory.MaxLevel())
                {
                    _levelState = LevelState.Menu;
                }

                UpdateSelectionSave();
                _levelId++;
                Log.Information($"Increased level id to {_levelId}");
                break;

            case LevelState.SelectLevel:
                UpdateSelectionSave();
                Log.Information($"Changing level to select screen.");
                _levelState = LevelState.Select;
                break;
            case LevelState.EndlessLevel:
                _currentEndlessLevelId = -1;
                if (_currentDifficulty < _difficultyStep)
                    _currentDifficulty = 0;
                _currentDifficulty += _difficultyStep;
                var endlessProgress = _settingsAndSaveManager.GetSave<EndlessProgress>();
                var challenges = _settingsAndSaveManager.GetSave<Challenges>();
                var progress = _currentDifficulty / _difficultyStep;
                if (endlessProgress.HighestLevel < progress)
                {
                    endlessProgress.HighestLevel = progress;
                    challenges.Score10 = progress >= 10;
                    challenges.Score25 = progress >= 25;
                    challenges.Score50 = progress >= 50;
                    _settingsAndSaveManager.SaveSave();
                }

                if (progress == 50)
                    _endlessEndTracking = true;

                break;
        }

        ChangeLevel();
    }

    private void UpdateSelectionSave()
        => UpdateSelectionSave(_levelId, _difficulty);

    private void UpdateSelectionSave(int level, int difficulty)
    {
        Log.Information($"Marking difficulty as beaten.");
        if (!LevelFactory.HasLevelDifficulty(level))
        {
            _selectionProgress.Levels[level - 1].BeatEasy = true;
            _selectionProgress.Levels[level - 1].BeatMedium = true;
            _selectionProgress.Levels[level - 1].BeatHard = true;
            _selectionProgress.Levels[level - 1].BeatInsane = true;
            _selectionProgress.Levels[level - 1].BeatExtreme = true;
        }

        switch (Selection.Level.ResolveDifficulty(difficulty))
        {
            default:
            case Difficulty.Easy:
                _selectionProgress.Levels[level - 1].BeatEasy = true;
                break;
            case Difficulty.Medium:
                _selectionProgress.Levels[level - 1].BeatMedium = true;
                break;
            case Difficulty.Hard:
                _selectionProgress.Levels[level - 1].BeatHard = true;
                break;
            case Difficulty.Insane:
                _selectionProgress.Levels[level - 1].BeatInsane = true;
                break;
            case Difficulty.Extreme:
                _selectionProgress.Levels[level - 1].BeatExtreme = true;
                break;
        }

        _progress.FinishedSelect = _selectionProgress.Levels.All(l =>
            l.BeatEasy && l.BeatMedium && l.BeatHard && l.BeatInsane && l.BeatExtreme);

        _settingsAndSaveManager.SaveSave();
    }

    private void ExitLevel()
    {
        _levelState = _levelState switch
        {
            LevelState.SelectLevel => LevelState.Select,
            LevelState.EndlessLevel => LevelState.Endless,
            _ => LevelState.Menu
        };

        ChangeLevel();
    }

    private void FailLevel()
    {
        Log.Information($"Level failed. Current level: {_levelId}");
        ChangeLevel();
    }

    public int GetCurrentLevelId()
        => _levelId;

    public SampleLevel GetCurrentLevel()
        => _currentLevel;
}