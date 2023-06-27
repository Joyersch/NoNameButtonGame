using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Logic.Management;
using MonoUtils.Objects.Buttons;
using MonoUtils.Objects.TextSystem;
using MonoUtils.Ui.TextSystem;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level3;

public class SimonSays : IManageable, IInteractable
{
    public Rectangle Rectangle { get; private set; }
    public event Action Finished;
    public float Speed { get; set; } = 750F;
    private readonly Random _random;


    private readonly SimonSaysButton[] _buttons = new SimonSaysButton[5];
    private readonly TextButton _start;
    private readonly Text _enteredSequenceDisplay;
    private readonly OverTimeInvoker _waitBetweenColorHighlightInvoker;
    private SimonSequence _sequence;
    private SimonAction _state;

    public SimonSequence Sequence => _sequence;
    
    private readonly int _length;

    private int _maxPlayed = 1;
    private int _played;

    public enum SimonAction
    {
        Before,
        Playing,
        Played,
        UserCanInput,
        UserFinishedInput
    }

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
        _state = SimonAction.Before;
        _length = length;
        _sequence = new SimonSequence(1, 5, length, random);

        _buttons[0] = new SimonSaysButton(_values[1], Color.Black, Speed);
        _buttons[1] = new SimonSaysButton(_values[2], Color.Black, Speed);
        _buttons[2] = new SimonSaysButton(_values[3], Color.Black, Speed);
        _buttons[3] = new SimonSaysButton(_values[4], Color.Black, Speed);
        _buttons[4] = new SimonSaysButton(_values[5], Color.Black, Speed);
        _enteredSequenceDisplay = new Text(string.Empty, 2F);
        _enteredSequenceDisplay.GetCalculator(area)
            .OnCenter()
            .OnY(4, 5)
            .Centered()
            .Move();

        _waitBetweenColorHighlightInvoker = new OverTimeInvoker(500F, false)
        {
            InvokeOnce = true
        };
        _waitBetweenColorHighlightInvoker.Trigger += WaitBetweenColorHighlightInvokerOnTrigger;

        int i = 0;
        foreach (var button in _buttons)
        {
            button.GetCalculator(area).OnCenter().OnX(i++ * 0.2F + 0.1F).Centered().Move();
            button.Finished += _waitBetweenColorHighlightInvoker.Start;
            button.Click += SimonButtonClick;
        }

        _start = new TextButton("Start", 2F);
        _start.GetCalculator(area).OnCenter().OnY(1, 3).Centered().Move();
        _start.Click += _ => StartClick();
    }

    private void WaitBetweenColorHighlightInvokerOnTrigger()
    {
        if (_state == SimonAction.Played)
        {
            _state = SimonAction.UserCanInput;
            return;
        }

        if (_state == SimonAction.UserFinishedInput)
        {
            if (_length == _maxPlayed)
            {
                Finished?.Invoke();
                return;
            }
            
            _played = 0;
            _maxPlayed++;
            _state = SimonAction.Playing;
        }

        PlayNext();
    }

    private void SimonButtonClick(object obj)
    {
        if (_state != SimonAction.UserCanInput)
            return;
        var button = (SimonSaysButton) obj;


        // first should always find something in the range of the values dict since the button color is assigned using said dict
        int input = _values.First(v => v.Value == button.Color).Key - 1;
        
        if (!_sequence.Compare(_played, input))
        {
            Reset();
            return;
        }

        _played++;

        if (_played != _maxPlayed)
            return;

        _state = SimonAction.UserFinishedInput;
        
        _waitBetweenColorHighlightInvoker.Start();
    }

    private void Reset()
    {
        _state = SimonAction.Before;
        _played = 0;
        _maxPlayed = 1;
        _sequence = new SimonSequence(1, 5, _length, _random);
    }

    private void StartClick()
    {
        _state = SimonAction.Playing;
        _waitBetweenColorHighlightInvoker.Start();
    }

    private void PlayNext()
    {
        if (_state != SimonAction.Playing)
            return;
        var last = !_sequence.Next(_maxPlayed, out int button);
        
        _buttons[button].Highlight();
        if (last)
            _state = SimonAction.Played;
    }
    
    private void SetEnteredText()
    {
        switch (_state)
        {
            case SimonAction.UserCanInput:
            case SimonAction.UserFinishedInput:
            {
                var played = _sequence.GetRange(0, _played);
                var color = new Color[played.Length];
        
                int j = 0;
                foreach (var i in played)
                    color[j++] = _values[i + 1];
        
                string value = played.Aggregate(string.Empty, (current, i) => current + Letter.Full);
                _enteredSequenceDisplay.ChangeText(value);
                _enteredSequenceDisplay.ChangeColor(color);
                break;
            }
            case SimonAction.Before:
                _enteredSequenceDisplay.ChangeText("Click Start!");
                break;
            case SimonAction.Playing:
            case SimonAction.Played:
                _enteredSequenceDisplay.ChangeText("Playing Sequence");
                break;
        }
    }

    public void Update(GameTime gameTime)
    {
        foreach (var button in _buttons)
            button.Update(gameTime);

        SetEnteredText();
        
        _enteredSequenceDisplay.GetCalculator(Rectangle)
            .OnCenter()
            .OnY(4, 5)
            .Centered()
            .Move();
        _enteredSequenceDisplay.Update(gameTime);
        
        _start.Update(gameTime);
        _waitBetweenColorHighlightInvoker.Update(gameTime);
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
        if (_state == SimonAction.Before)
            _start.Draw(spriteBatch);
        _enteredSequenceDisplay.Draw(spriteBatch);
    }

    public void DrawStatic(SpriteBatch spriteBatch)
    {
        foreach (var button in _buttons)
            button.DrawStatic(spriteBatch);
        if (_state == SimonAction.Before)
            _start.DrawStatic(spriteBatch);
        _enteredSequenceDisplay.DrawStatic(spriteBatch);
    }
}