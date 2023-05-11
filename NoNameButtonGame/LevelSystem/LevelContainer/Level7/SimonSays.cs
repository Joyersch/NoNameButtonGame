using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.Extensions;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.TextSystem;
using NoNameButtonGame.Interfaces;
using NoNameButtonGame.Logging;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level7;

public class SimonSays : IManageable, IInteractable
{
    public Rectangle Rectangle { get; private set; }
    public event Action Finished;
    public float Speed { get; set; } = 750F;
    private readonly Random _random;


    private readonly SimonSaysButton[] _buttons = new SimonSaysButton[5];
    private readonly TextButton _start;
    private readonly Text _enteredSequenceDisplay;
    private readonly int _length;

    private bool _started;
    private bool _isPlaying;
    private int _played;
    private int _maxPlayed = 1;

    private int _clicked;
    private int[] _playedSequence;
    private int[] _enteredSequence;

    private readonly Dictionary<int, Color> _values = new Dictionary<int, Color>()
    {
        {0, Color.White},
        {1, Color.Gold},
        {2, Color.DarkRed},
        {3, Color.Green},
        {4, Color.DarkBlue},
        {5, Color.Purple}
    };

    public SimonSays(Rectangle area, Random random, int length)
    {
        _random = random;
        Rectangle = area;
        _length = length;
        _playedSequence = new int[length];
        _enteredSequence = new int[1];
        for (int j = 0; j < length; j++)
            _playedSequence[j] = _random.Next(0, _buttons.Length);

        _buttons[0] = new SimonSaysButton(_values[1], Color.Yellow);
        _buttons[1] = new SimonSaysButton(_values[2], Color.Red);
        _buttons[2] = new SimonSaysButton(_values[3], Color.DarkGreen);
        _buttons[3] = new SimonSaysButton(_values[4], Color.Blue);
        _buttons[4] = new SimonSaysButton(_values[5], Color.Magenta);
        _enteredSequenceDisplay = new Text(string.Empty, 2F);
        _enteredSequenceDisplay.GetCalculator(area).OnCenter().OnY(4, 5).Centered().Move();
        int i = 0;
        foreach (var button in _buttons)
        {
            button.GetCalculator(area).OnCenter().OnX(i++ * 0.2F + 0.1F).Centered().Move();
            button.Finished += PlayNext;
            button.Click += ButtonClick;
        }

        _start = new TextButton("Start", 2F);
        _start.GetCalculator(area).OnCenter().OnY(1, 3).Centered().Move();
        _start.Click += _ => StartClick();
    }

    private void ButtonClick(object obj)
    {
        if (_isPlaying)
            return;

        var button = (SimonSaysButton) obj;
        var id = _values.First(v => v.Value == button.Color).Key;

        if (_playedSequence[_clicked] != id)
            Reset();

        _enteredSequence[_clicked++] = id;

        if (_clicked != _maxPlayed)
            return;
        
        _played = 0;
        _clicked = 0;
        _enteredSequence = new int[_maxPlayed++];
        StartClick();
    }

    private void StartClick()
    {
        _started = true;
        _isPlaying = true;
        PlayNext();
    }

    private void PlayNext()
    {
        if (_played == _maxPlayed || !_started)
        {
            _isPlaying = false;
            return;
        }
        _buttons[_played++].Highlight(Speed);
    }

    private void Reset()
    {
        _played = 0;
        _maxPlayed = 1;
        _clicked = 0;
        _started = false;
        _isPlaying = false;
        _enteredSequence = new int[1];
    }

    private void SetEnteredText()
    {
        Color[] color = new Color[_enteredSequence.Length];
        int j = 0;
        foreach (var i in _enteredSequence)
            color[j++] = _values[i];
        string value = _enteredSequence.Aggregate(string.Empty, (current, i) => current + Letter.Full);
        _enteredSequenceDisplay.ChangeText(value);
        _enteredSequenceDisplay.ChangeColor(color);
    }
    public void Update(GameTime gameTime)
    {
        Log.WriteLine($"Played:{_played}/{_maxPlayed}");
        Log.WriteLine($"Clicked:{_clicked}/{_maxPlayed}");
        foreach (var button in _buttons)
            button.Update(gameTime);
        SetEnteredText();
        _enteredSequenceDisplay.GetCalculator(Rectangle).OnCenter().OnY(4, 5).Centered().Move();
        _enteredSequenceDisplay.Update(gameTime);
        _start.Update(gameTime);
    }

    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        foreach (var button in _buttons)
            button.UpdateInteraction(gameTime, toCheck);
        _start.UpdateInteraction(gameTime, toCheck);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var button in _buttons)
            button.Draw(spriteBatch);
        if (!_started)
            _start.Draw(spriteBatch);
        _enteredSequenceDisplay.Draw(spriteBatch);
    }

    public void DrawStatic(SpriteBatch spriteBatch)
    {
        foreach (var button in _buttons)
            button.DrawStatic(spriteBatch);
        if (!_started)
            _start.DrawStatic(spriteBatch);
        _enteredSequenceDisplay.DrawStatic(spriteBatch);
    }
}