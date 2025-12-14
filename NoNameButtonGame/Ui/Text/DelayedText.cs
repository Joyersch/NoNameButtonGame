using System;
using Microsoft.Xna.Framework;

namespace NoNameButtonGame.Ui.Text;

public sealed class DelayedText : BasicText
{
    public event Action FinishedPlaying;

    private readonly string _toDisplayText;
    private string _currentlyDisplayed = string.Empty;
    private int _textPointer = int.MaxValue;

    private Vector2 _fullSize;
    private float _scale;
    private float _extendedScale = 1F;

    private float _savedGameTime;
    private float _waitedStartTime;
    public float StartAfter = 0;

    public float Scale => _scale * _extendedScale;

    public bool IsPlaying { get; private set; }

    public bool HasPlayed { get; private set; }
    public int DisplayDelay { get; set; } = 125;

    public new static float DefaultScale { get; set; } = 2F;

    public DelayedText(string text) : this(text, true, Vector2.Zero, DefaultScale,
        1)
    {
    }

    public DelayedText(string text, bool automaticStart) : this(text, automaticStart, Vector2.Zero, DefaultScale,
        1)
    {
    }

    public DelayedText(string text, bool automaticStart, Vector2 position) : this(text, automaticStart, position,
        DefaultScale, 1)
    {
    }

    public DelayedText(string text, bool automaticStart, Vector2 position, float scale) : this(text, automaticStart,
        position, scale, 1)
    {
    }

    public DelayedText(string text, bool automaticStart, Vector2 position, float scale, int spacing) : base(text,
        position, scale, spacing)
    {
        _toDisplayText = text;
        _scale = scale;
        _fullSize = GetFullBaseCopy().GetSize();
        if (automaticStart)
            Start();
        else
            ChangeText(_currentlyDisplayed);
    }

    public override void Update(GameTime gameTime)
    {
        var passedGameTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        bool canDisplay = true;
        if (_waitedStartTime > 0)
        {
            _waitedStartTime -= passedGameTime;
            canDisplay = false;
        }

        if (_textPointer < _toDisplayText.Length && canDisplay)
            _savedGameTime += passedGameTime;


        while (_savedGameTime > DisplayDelay && canDisplay && _textPointer < _toDisplayText.Length)
        {
            _savedGameTime -= DisplayDelay;
            _currentlyDisplayed += _toDisplayText[_textPointer];
            _textPointer++;
        }

        if (ToString() != _currentlyDisplayed)
            ChangeText(_currentlyDisplayed);

        base.Update(gameTime);

        if (IsPlaying && _textPointer == _toDisplayText.Length)
        {
            IsPlaying = false;
            HasPlayed = true;
            FinishedPlaying?.Invoke();
        }
    }

    public override Vector2 GetSize()
        => _fullSize;

    public void Start()
    {
        _textPointer = 0;
        _waitedStartTime = StartAfter;
        _currentlyDisplayed = string.Empty;
        IsPlaying = true;
    }

    public BasicText GetFullBaseCopy()
        => new BasicText(_toDisplayText, Position, Scale, Spacing);

    public override void SetScale(ScaleProvider provider)
    {
        base.SetScale(provider);
        _extendedScale = provider.Scale;
        _fullSize = GetFullBaseCopy().GetSize();
    }
}