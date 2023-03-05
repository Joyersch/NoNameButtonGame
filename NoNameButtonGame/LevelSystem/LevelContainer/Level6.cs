using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NoNameButtonGame.Extensions;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.Debug;
using NoNameButtonGame.GameObjects.TextSystem;
using NoNameButtonGame.LogicObjects;
using NoNameButtonGame.LogicObjects.Listener;

namespace NoNameButtonGame.LevelSystem.LevelContainer;

public class Level6 : SampleLevel
{
    private Storage.Storage _storage;

    private readonly Cursor _cursor;

    private readonly PositionListener _positionListener;

    private readonly TextButton _clickerButton;
    private readonly Text _counter;

    private readonly TextButton _moveToShopButton;
    private readonly TextButton _moveToClickerButton;
    private readonly OverTimeMover _overTimeMover;


    private int _bakedBeansCounter;

    private BeanState _state;

    private enum BeanState : ushort
    {
        Started = 0,
        Reached100 = 1,
        Reached1k = 2,
        Reached5k = 3,
        Reached10k = 4,
        Reached25k = 5,
        Reached100k = 6,
        Reached250k = 7,
        Reached1m = 8
    }

    public Level6(int defaultWidth, int defaultHeight, Vector2 window, Random random, Storage.Storage storage) : base(
        defaultWidth,
        defaultHeight, window, random)
    {
        Name = "Level 6 - Just like Cookie Clicker but with BEANS!";

        _state = BeanState.Started;

        _storage = storage;
        _bakedBeansCounter = storage.GameData.CurrentBackedBeans;

        _clickerButton = new TextButton("Bake a bean");
        _clickerButton.Move(-_clickerButton.Size / 2);
        _clickerButton.Click += IncreaseBeanCounter;
        _clickerButton.Click += UpdateState;
        _clickerButton.Click += WriteBeanCounterToStorage;

        _cursor = new Cursor();
        _positionListener = new PositionListener();
        _positionListener.Add(_mouse, _cursor);

        _moveToClickerButton = new TextButton("Return", "return", 0.75F);
        _moveToClickerButton.Move(new Vector2(320, 180) - new Vector2(0, _moveToClickerButton.Size.Y));
        _moveToClickerButton.Click += MoveCamera;
        
        _moveToShopButton = new TextButton("Shop", "shop", 0.75F);
        _moveToShopButton.Move(_moveToClickerButton.Position - new Vector2(_moveToShopButton.Size.X, 0));
        _moveToShopButton.Click += MoveCamera;


        
        _overTimeMover = new OverTimeMover(Camera, new Vector2(640, 0), 500F, OverTimeMover.MoveMode.Sin);

        _bakedBeansCounter = _storage.GameData.CurrentBackedBeans;
        _counter = new Text(string.Empty);

        UpdateState();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        _positionListener.Update(gameTime);
        _cursor.Update(gameTime);

        _clickerButton.Update(gameTime, _cursor.Hitbox[0]);

        _overTimeMover.Update(gameTime);

        if (_state >= BeanState.Reached100)
            _moveToShopButton.Update(gameTime, _cursor.Hitbox[0]);
        
        _moveToClickerButton.Update(gameTime, _cursor.Hitbox[0]);

        if (_bakedBeansCounter > 0)
            _counter.ChangeText(_bakedBeansCounter.ToString() + Letter.ReverseParse(Letter.Character.AmongUsBean));
        _counter.Move(-_counter.Rectangle.Size.ToVector2() / 2 + new Vector2(0, -64));
        _counter.Update(gameTime);
        
        Console.WriteLine(Camera.Rectangle);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _clickerButton.Draw(spriteBatch);
        _counter.Draw(spriteBatch);

        if (_state >= BeanState.Reached100)
            _moveToShopButton.Draw(spriteBatch);
        
        _moveToClickerButton.Draw(spriteBatch);

        _cursor.Draw(spriteBatch);
    }

    private void MoveCamera(object obj)
    {
        _overTimeMover.Start();
        _overTimeMover.ChangeDestination(((TextButton)obj).Name == "return" ? Vector2.Zero : new Vector2(640, 0));
    }

    private void WriteBeanCounterToStorage(object obj)
    {
        _storage.GameData.CurrentBackedBeans = _bakedBeansCounter;
        _storage.Save();
    }

    private void IncreaseBeanCounter(object obj)
    {
        _bakedBeansCounter++;
    }

    private void UpdateState(object obj)
        => UpdateState();

    private void UpdateState()
    {
        while (ShouldStateIncrease())
            _state++;
    }

    private bool ShouldStateIncrease()
    {
        switch (_state)
        {
            case BeanState.Started:
                if (_bakedBeansCounter >= 100)
                    return true;
                break;
            case BeanState.Reached100:
                if (_bakedBeansCounter >= 1000)
                    return true;
                break;
            case BeanState.Reached1k:
                if (_bakedBeansCounter >= 5000)
                    return true;
                break;
            case BeanState.Reached5k:
                if (_bakedBeansCounter >= 10000)
                    return true;
                break;
            case BeanState.Reached10k:
                if (_bakedBeansCounter >= 25000)
                    return true;
                break;
            case BeanState.Reached25k:
                if (_bakedBeansCounter >= 100000)
                    return true;
                break;
            case BeanState.Reached100k:
                if (_bakedBeansCounter >= 250000)
                    return true;
                break;
            case BeanState.Reached250k:
                if (_bakedBeansCounter >= 1000000)
                    return true;
                break;
        }

        return false;
    }
}