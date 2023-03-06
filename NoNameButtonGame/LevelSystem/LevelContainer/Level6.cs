using System;
using System.Net.Security;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NoNameButtonGame.Extensions;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.AddOn;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.Debug;
using NoNameButtonGame.GameObjects.TextSystem;
using NoNameButtonGame.LogicObjects;
using NoNameButtonGame.LogicObjects.Listener;

namespace NoNameButtonGame.LevelSystem.LevelContainer;

public class Level6 : SampleLevel
{
    private Storage.Storage _storage;

    private readonly Text _counter;

    private readonly LockButtonAddon _shopButtonLock;
    
    private readonly OverTimeMover _overTimeMover;

    private readonly Text shopOne;
    private readonly Text shopTwo;
    private readonly Text shopThree;
    private readonly Text shopFour;

    private int _bakedBeansCounter;
    private int _bakedBeansIncrement = 1;
    private int _bakedBeansCriticalIncrementChance;
    private int _bakedBeansRelativeIncrement;

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

    public Level6(Display.Display display, Vector2 window, Random random, Storage.Storage storage) : base(display,
        window, random)
    {
        Name = "Level 6 - Just like Cookie Clicker but with BEANS!";

        _state = BeanState.Started;

        _storage = storage;
        _bakedBeansCounter = storage.GameData.CurrentBackedBeans;

        var shopScreen = new Vector2(640, 0);

        var clickerButton = new TextButton("Bake a bean");
        clickerButton.Move(-clickerButton.Size / 2);
        clickerButton.Click += IncreaseBeanCounter;
        clickerButton.Click += UpdateState;
        clickerButton.Click += WriteBeanCounterToStorage;
        AutoManaged.Add(clickerButton);


        var moveToClickerButton = new TextButton("Return", "return", 0.75F);
        moveToClickerButton.Move(new Vector2(320, 180) - new Vector2(0, moveToClickerButton.Size.Y));
        moveToClickerButton.Click += MoveCamera;
        AutoManaged.Add(moveToClickerButton);

        var moveToShopButton = new TextButton("Shop", "shop", 0.75F);
        moveToShopButton.Move(moveToClickerButton.Position - new Vector2(moveToShopButton.Size.X, 0));
        moveToShopButton.Click += MoveCamera;

        _shopButtonLock = new LockButtonAddon(moveToShopButton);
        AutoManaged.Add(_shopButtonLock);

        var shopOptionOne = new TextButton("Canned Beans");
        shopOptionOne.Move(shopScreen);

        var shopLockOne = new LockButtonAddon(shopOptionOne);
        AutoManaged.Add(shopLockOne);

        var shopOptionTwo = new TextButton("Jellybeans");
        shopOptionTwo.Move(shopScreen);

        var shopLockTwo = new LockButtonAddon(shopOptionTwo);
        AutoManaged.Add(shopLockTwo);

        var shopOptionThree = new TextButton("Genetically modified");
        shopOptionThree.Move(shopScreen);

        var shopLockThree = new LockButtonAddon(shopOptionThree);
        AutoManaged.Add(shopLockThree);

        var shopOptionFour = new TextButton("Magic Beans");
        shopOptionFour.Move(shopScreen);

        var shopLockFour = new LockButtonAddon(shopOptionFour);
        AutoManaged.Add(shopLockFour);

        shopOne = new Text(string.Empty);
        shopOne.Move(shopScreen);
        AutoManaged.Add(shopOne);

        shopTwo = new Text(string.Empty);
        shopTwo.Move(shopScreen);
        AutoManaged.Add(shopTwo);

        shopThree = new Text(string.Empty);
        shopThree.Move(shopScreen);
        AutoManaged.Add(shopThree);

        shopFour = new Text(string.Empty);
        shopFour.Move(shopScreen);
        AutoManaged.Add(shopFour);

        _overTimeMover = new OverTimeMover(Camera, new Vector2(640, 0), 500F, OverTimeMover.MoveMode.Sin);

        _bakedBeansCounter = _storage.GameData.CurrentBackedBeans;
        _counter = new Text(string.Empty);

        UpdateState();

        var cursor = new Cursor();
        Interactable = cursor;
        PositionListener.Add(_mouse, cursor);
        AutoManaged.Add(cursor);
    }

    public override void Update(GameTime gameTime)
    {
        shopOne.ChangeText(_bakedBeansIncrement.ToString());
        shopTwo.ChangeText(_bakedBeansIncrement.ToString());
        shopThree.ChangeText(_bakedBeansIncrement.ToString());
        shopFour.ChangeText(_bakedBeansIncrement.ToString());
        base.Update(gameTime);
        PositionListener.Update(gameTime);
        _overTimeMover.Update(gameTime);

        if (_state >= BeanState.Reached100)
            _shopButtonLock.Unlock();

        if (_bakedBeansCounter > 0)
            _counter.ChangeText(_bakedBeansCounter.ToString() + Letter.ReverseParse(Letter.Character.AmongUsBean));
        _counter.Move(-_counter.Rectangle.Size.ToVector2() / 2 + new Vector2(0, -64));
        _counter.Update(gameTime);

        Console.WriteLine(Camera.Rectangle);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _counter.Draw(spriteBatch);
        base.Draw(spriteBatch);
    }

    private void MoveCamera(object obj)
    {
        _overTimeMover.Start();
        _overTimeMover.ChangeDestination(((TextButton) obj).Name == "return" ? Vector2.Zero : new Vector2(640, 0));
    }

    private void WriteBeanCounterToStorage(object obj)
    {
        _storage.GameData.CurrentBackedBeans = _bakedBeansCounter;
        _storage.Save();
    }

    private void IncreaseBeanCounter(object obj)
    {
        int increment = _bakedBeansIncrement;
        increment += _bakedBeansCounter / 1000 * _bakedBeansRelativeIncrement;
        var crit = Random.Next(0, 100);
        if (crit >= _bakedBeansCriticalIncrementChance)
            increment *= 10;

        _bakedBeansCounter += increment;
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