using System;
using Microsoft.Xna.Framework;

namespace NoNameButtonGame.GameObjects.TextSystem;

public class DelayedText : Text
{
    public event Action FinishedPlaying;
    
    private readonly string _toDisplayText;
    private string _currentlyDisplayed = string.Empty;
    private int _textPointer = int.MaxValue;

    private float _savedGameTime;
    private float _waitedStartTime;
    public float StartAfter = 0;

    public bool IsPlaying { get; private set; }

    public bool HasPlayed { get; private set; }
    public int DisplayDelay { get; set; } = 125;

    public new static Vector2 DefaultLetterSize => new Vector2(16, 16);

    public DelayedText(string text) : this(text, Vector2.Zero, DefaultLetterSize, 1)
    {
    }
    
    public DelayedText(string text, Vector2 position) : this(text, position, DefaultLetterSize, 1)
    {
    }

    public DelayedText(string text, Vector2 position, float scale) : this(text, position, DefaultLetterSize * scale, 1)
    {
    }

    public DelayedText(string text, Vector2 position, float scale, int spacing) : this(text, position,
        DefaultLetterSize * scale, spacing)
    {
    }

    public DelayedText(string text, Vector2 position, Vector2 letterSize, int spacing) : base(string.Empty, position,
        letterSize, spacing)
    {
        _toDisplayText = text;
    }

    public override void Update(GameTime gameTime)
    {
        var passedGameTime =  (float) gameTime.ElapsedGameTime.TotalMilliseconds;
        bool canDisplay = true;
        if (_waitedStartTime > 0)
        {
            _waitedStartTime -= passedGameTime;
            canDisplay = false;
        }
        if (_textPointer < _toDisplayText.Length && canDisplay)
            _savedGameTime += passedGameTime;


        while (_savedGameTime > DisplayDelay && canDisplay)
        {
            _savedGameTime -= DisplayDelay;
            _currentlyDisplayed += _toDisplayText[_textPointer];
            _textPointer++;
        }

        if (Value != _currentlyDisplayed)
            ChangeText(_currentlyDisplayed);
        
        base.Update(gameTime);
        
        if (IsPlaying && _textPointer == _toDisplayText.Length)
        {
            IsPlaying = false;
            HasPlayed = true;
            FinishedPlaying?.Invoke();
        }
    }

    public void Start()
    {
        _textPointer = 0;
        _waitedStartTime = StartAfter;
        _currentlyDisplayed = string.Empty;
        IsPlaying = true;
    }

    public Text GetBaseCopy()
        => new Text(_toDisplayText, Position, Size, _spacing);
}