using Microsoft.Xna.Framework;

namespace NoNameButtonGame.Text;

public class DelayedText : TextBuilder
{
    private string _toDisplayText;
    private string _currentlyDisplayed = string.Empty;
    private int _textPointer = int.MaxValue;

    private float _savedGameTime;
    private int DisplayDelay = 125;
    public new static Vector2 DefaultLetterSize => new Vector2(16, 16);

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
        _savedGameTime += (float) gameTime.ElapsedGameTime.TotalMilliseconds;
        while (_savedGameTime > DisplayDelay && _textPointer < _toDisplayText.Length)
        {
            _savedGameTime -= DisplayDelay;
            _currentlyDisplayed += _toDisplayText[_textPointer];
            _textPointer++;
        }

        if (Text != _currentlyDisplayed)
            ChangeText(_currentlyDisplayed);
        
        base.Update(gameTime);
    }

    public void Start()
    {
        _textPointer = 0;
        _currentlyDisplayed = string.Empty;
    }
}