using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logic;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Logic.Management;
using MonoUtils.Ui.Objects.Buttons;
using MonoUtils.Ui.Objects.TextSystem;
using NoNameButtonGame.GameObjects.Buttons;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace NoNameButtonGame.LevelSystem.LevelContainer.SimonSaysLevel;

public class SimonSays : IManageable, IInteractable
{
    public Rectangle Rectangle { get; private set; }
    public event Action Finished;
    private readonly Random _random;


    private readonly SimonSaysButton[] _buttons = new SimonSaysButton[5];
    private readonly TextButton<SampleButton> _start;
    private readonly Text _enteredSequenceDisplay;
    private readonly OverTimeInvoker _displaySequenceOverTimeInvoker;
    private SimonSequence _sequence;
    private SimonAction _state;

    public SimonSequence Sequence => _sequence;

    private readonly int _length;
    private readonly Dictionary<string, string> _text;

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

    private readonly List<Color> _infoDisplayColors = new()
    {
        Color.Yellow,
        Color.Red,
        Color.Green,
        Color.Blue,
        Color.Purple
    };

    public SimonSays(Rectangle area, Random random, Dictionary<string, string> text, int length,
        float waitBetweenColors, float buttonDisplaySpeed)
    {
        _random = random;
        Rectangle = area;
        _state = SimonAction.Before;
        _length = length;
        _text = text;
        _sequence = new SimonSequence(1, 5, length, random);

        _buttons[0] = new SimonSaysButton(SimonColors.DarkYellow, SimonColors.LightYellow, buttonDisplaySpeed);
        _buttons[1] = new SimonSaysButton(SimonColors.DarkRed, SimonColors.LightRed, buttonDisplaySpeed);
        _buttons[2] = new SimonSaysButton(SimonColors.DarkGreen, SimonColors.LightGreen, buttonDisplaySpeed);
        _buttons[3] = new SimonSaysButton(SimonColors.DarkBlue, SimonColors.LightBlue, buttonDisplaySpeed);
        _buttons[4] = new SimonSaysButton(SimonColors.DarkPurple, SimonColors.LightPurple, buttonDisplaySpeed);
        _enteredSequenceDisplay = new Text(string.Empty, 1F);
        _enteredSequenceDisplay.GetCalculator(area)
            .OnCenter()
            .OnY(4, 5)
            .Centered()
            .Move();

        _displaySequenceOverTimeInvoker = new OverTimeInvoker(waitBetweenColors, false)
        {
            InvokeOnce = true
        };
        _displaySequenceOverTimeInvoker.Trigger += HandleNextSequencePart;

        int i = 0;
        foreach (var button in _buttons)
        {
            button.GetCalculator(area).OnCenter().OnX(i++ * 0.2F + 0.1F).Centered().Move();
            button.Finished += delegate
            {
                if (_state == SimonAction.Played)
                    _state = SimonAction.UserCanInput;

                _displaySequenceOverTimeInvoker.Start();
            };
            button.Click += SimonButtonClick;
        }

        _start = new Button(text["start"]);
        _start.GetCalculator(area).OnCenter().OnY(1, 3).Centered().Move();
        _start.Click += _ => StartClick();
    }

    private void HandleNextSequencePart()
    {
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
        var button = (SimonSaysButton)obj;


        // first should always find something in the range of the values dict since the button color is assigned using said dict
        int input = _buttons.ToList().IndexOf((SimonSaysButton)obj);

        if (!_sequence.Compare(_played, input))
        {
            Reset();
            return;
        }

        _played++;

        if (_played != _maxPlayed)
            return;

        _state = SimonAction.UserFinishedInput;

        _displaySequenceOverTimeInvoker.Start();
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
        _displaySequenceOverTimeInvoker.Start();
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
                    color[j++] = _infoDisplayColors[i];

                string value = played.Aggregate(string.Empty, (current, i) => current + "[block]");
                _enteredSequenceDisplay.ChangeText(value);
                _enteredSequenceDisplay.ChangeColor(color);
                break;
            }
            case SimonAction.Before:
                _enteredSequenceDisplay.ChangeColor(Color.White);
                _enteredSequenceDisplay.ChangeText(_text["clickStart"]);
                break;
            case SimonAction.Playing:
            case SimonAction.Played:
                _enteredSequenceDisplay.ChangeColor(Color.White);
                _enteredSequenceDisplay.ChangeText(_text["playingSequence"]);
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
        _displaySequenceOverTimeInvoker.Update(gameTime);
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
}