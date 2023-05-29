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
using NoNameButtonGame.LogicObjects;
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

    private bool _hideStartButton;

    private bool _isPlaying;
    private int _hits;
    private int _maxPlayed = 1;

    private SimonSequence _sequence;
    private int[] _playedSequence;

    private OverTimeInvoker _invoker;

    private readonly Dictionary<int, Color> _values = new Dictionary<int, Color>()
    {
        {0, Color.White},
        {1, Color.Yellow},
        {2, Color.Red},
        {3, Color.Green},
        {4, Color.Blue},
        {5, Color.Purple}
    };

    public SimonSays(Rectangle area, Random random, int length)
    {
        _random = random;
        Rectangle = area;
        _length = length;
        _sequence = new SimonSequence(1, 5, length, random);
        _playedSequence = new int[length];
        for (int j = 0; j < length; j++)
            _playedSequence[j] = -1;

        _buttons[0] = new SimonSaysButton(_values[1], Color.Black, Speed);
        _buttons[1] = new SimonSaysButton(_values[2], Color.Black, Speed);
        _buttons[2] = new SimonSaysButton(_values[3], Color.Black, Speed);
        _buttons[3] = new SimonSaysButton(_values[4], Color.Black, Speed);
        _buttons[4] = new SimonSaysButton(_values[5], Color.Black, Speed);
        _enteredSequenceDisplay = new Text(string.Empty, 2F);
        _enteredSequenceDisplay.GetCalculator(area).OnCenter().OnY(4, 5).Centered().Move();
        
        _invoker = new OverTimeInvoker(500F, false);
        _invoker.Trigger += PlayNext;
        _invoker.Trigger += _invoker.Stop;
        
        int i = 0;
        foreach (var button in _buttons)
        {
            button.GetCalculator(area).OnCenter().OnX(i++ * 0.2F + 0.1F).Centered().Move();
            button.Finished += _invoker.Start;
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
        var id = _values.First(v => v.Value == button.Color).Key - 1;

        bool hit = _sequence.Compare(_hits, id);

        if (!hit)
        {
            Reset();
            PlayNext();
            return;
        }

        _playedSequence[_hits] = id + 1;
        _hits++;
        if (_hits == _maxPlayed)
        {
            _hits = 0;
            _maxPlayed++;
            _playedSequence = new int[_length];
            for (int i = 0; i < _length; i++)
                _playedSequence[i] = -1;
            _invoker.Start();
        }
    }

    private void StartClick()
    {
        Reset();
        _hideStartButton = true;
        _isPlaying = true;
        _invoker.Start();
    }

    private void PlayNext()
    {
        if (!_isPlaying)
            return;
        var last = !_sequence.Next(_maxPlayed, out int button);
        _buttons[button].Highlight();
        if (last)
            _isPlaying = false;
    }

    private void Reset()
    {
        _maxPlayed = 1;
        _hits = 0;
        _hideStartButton = false;
        _isPlaying = false;
        _playedSequence = new int[_length];
        for (int i = 0; i < _length; i++)
            _playedSequence[i] = -1;
    }

    private void SetEnteredText()
    {
        var relevant = _playedSequence.Where(c => c != -1);
        Color[] color = new Color[relevant.Count()];
        int j = 0;
        foreach (var i in relevant)
            color[j++] = _values[i];

        string value = relevant.Aggregate(string.Empty, (current, i) => current + Letter.Full);
        _enteredSequenceDisplay.ChangeText(value);
        _enteredSequenceDisplay.ChangeColor(color);
    }

    public void Update(GameTime gameTime)
    {
        foreach (var button in _buttons)
            button.Update(gameTime);
        SetEnteredText();
        _enteredSequenceDisplay.GetCalculator(Rectangle).OnCenter().OnY(4, 5).Centered().Move();
        _enteredSequenceDisplay.Update(gameTime);
        _start.Update(gameTime);
        _invoker.Update(gameTime);
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
        if (!_hideStartButton)
            _start.Draw(spriteBatch);
        _enteredSequenceDisplay.Draw(spriteBatch);
    }

    public void DrawStatic(SpriteBatch spriteBatch)
    {
        foreach (var button in _buttons)
            button.DrawStatic(spriteBatch);
        if (!_hideStartButton)
            _start.DrawStatic(spriteBatch);
        _enteredSequenceDisplay.DrawStatic(spriteBatch);
    }
}