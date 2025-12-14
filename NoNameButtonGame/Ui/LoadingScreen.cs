using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.Ui.Color;
using NoNameButtonGame.Ui.Text;

namespace NoNameButtonGame.Ui;

public sealed class LoadingScreen : IManageable
{
    private long _max;
    private string _goal;
    private long _current;

    private int _dots;

    public Rectangle Rectangle => _area;
    private Rectangle _area;

    private readonly BasicText _loading;
    private readonly BasicText _progress;
    private readonly BasicText _bar;
    private int _loadbarLength = 196;

    private OverTimeInvoker _lazyDots;
    private Rainbow _rainbowColor;

    public bool ProgressEnabled = true;


    public LoadingScreen(Vector2 position, Vector2 size, long max, float scale, string goal)
    {
        _max = max;
        _goal = goal;
        _area = new Rectangle(position.ToPoint(), size.ToPoint());


        _loading = new BasicText("Loading", 5F * scale);
        _loading.InRectangle(this)
            .OnCenter()
            .OnY(0.25F)
            .Centered()
            .Apply();

        _progress = new BasicText(goal, 2F * scale);
        _progress.InRectangle(this)
            .OnCenter()
            .OnY(0.45F)
            .Centered()
            .Apply();

        StringBuilder builder = new();
        for (int i = 0; i < _loadbarLength; i++)
            builder.Append('|');

        _bar = new BasicText(builder.ToString(), Vector2.Zero, 2F * scale, 0);
        _bar.InRectangle(this)
            .OnCenter()
            .OnY(0.6F)
            .Centered()
            .Apply();

        _lazyDots = new OverTimeInvoker(666F);
        _lazyDots.Trigger += () =>
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Loading");

            for (int i = 0; i < _dots; i++)
                builder.Append('.');

            _loading.ChangeText(builder.ToString());
            _dots++;
            if (_dots > 3)
                _dots = 0;
        };

        _rainbowColor = new Rainbow() { GameTimeStepInterval = 6F, Increment = 2, Offset = 64 };
    }

    public void SetCurrent(long current)
    {
        _current = current;
    }

    public void SetMax(long max)
    {
        _max = max;
    }

    public void SetGoal(string goal)
    {
        _goal = goal;
    }

    public void Update(GameTime gameTime)
    {

        _lazyDots.Update(gameTime);

        _loading.Update(gameTime);
        _loading.InRectangle(this)
            .OnCenter()
            .OnY(0.25F)
            .Centered()
            .Apply();

        if (ProgressEnabled)
        {
            _progress.ChangeText($"{_goal}: {_current}/{_max}");
            _progress.Update(gameTime);
            _progress.InRectangle(this)
                .OnCenter()
                .OnY(0.45F)
                .Centered()
                .Apply();
        }

        if (_max == 0)
            return;

        // if this crashes do to int long cast than something else is broken because this should be a percentage (0..1 * 30)
        int done = (int)(_loadbarLength * _current / _max);
        ColorBuilder colorBuilder = new ColorBuilder();

        _rainbowColor.Update(gameTime);

        colorBuilder.AddColor(_rainbowColor.GetColor(done));
        colorBuilder.AddColor(Microsoft.Xna.Framework.Color.White, _loadbarLength - done);

        _bar.ChangeColor(colorBuilder.GetColor());
        _bar.Update(gameTime);
        _bar.InRectangle(this)
            .OnCenter()
            .OnY(0.6F)
            .Centered()
            .Apply();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _bar.Draw(spriteBatch);
        if (ProgressEnabled)
            _progress.Draw(spriteBatch);
        _loading.Draw(spriteBatch);
    }
}