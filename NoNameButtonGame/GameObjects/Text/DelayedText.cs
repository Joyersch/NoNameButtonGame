using Microsoft.Xna.Framework;
using NoNameButtonGame.Text;

namespace NoNameButtonGame.GameObjects.Text;

public class DelayedText : TextBuilder
{
    private readonly string toDisplayText;
    private string currentlyDisplayed = string.Empty;
    private int textPointer = int.MaxValue;

    private float savedGameTime;
    private float waitedStartTime;
    private const int DisplayDelay = 125;
    public float StartAfter = 0;

    public bool IsPlaying { get; private set; }

    public bool HasPlayed { get; private set; }

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
        toDisplayText = text;
    }

    public override void Update(GameTime gameTime)
    {
        var passedGameTime =  (float) gameTime.ElapsedGameTime.TotalMilliseconds;
        bool canDisplay = true;
        if (waitedStartTime > 0)
        {
            waitedStartTime -= passedGameTime;
            canDisplay = false;
        }
        if (textPointer < toDisplayText.Length && canDisplay)
            savedGameTime += passedGameTime;
        else if (IsPlaying)
        {
            IsPlaying = false;
            HasPlayed = true;
        }

        while (savedGameTime > DisplayDelay && canDisplay)
        {
            savedGameTime -= DisplayDelay;
            currentlyDisplayed += toDisplayText[textPointer];
            textPointer++;
        }

        if (Text != currentlyDisplayed)
            ChangeText(currentlyDisplayed);

        base.Update(gameTime);
    }

    public void Start()
    {
        textPointer = 0;
        waitedStartTime = StartAfter;
        currentlyDisplayed = string.Empty;
        IsPlaying = true;
    }
}