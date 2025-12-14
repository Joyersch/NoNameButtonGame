using System;
using Joyersch.Monogame.Ui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.Ui.Text;

namespace NoNameButtonGame.Ui;

public sealed class Timer : IManageable, IMoveable, IColorable, IScaleable
{
    private readonly OverTimeInvoker _invoker;
    private readonly BasicText _display;

    private readonly double _time;
    private bool _invoked;

    public event Action Trigger;

    private string _prefix;

    public float Scale => _display.Scale;

    public Timer(double time, bool start = false) : this(3F, time, string.Empty, start)
    {
    }
    public Timer(double time, string prefix, bool start = false) : this(3F, time, prefix, start)
    {
    }

    public Timer(float scale, double time, bool start = false) : this(Vector2.Zero, scale, time, string.Empty, start)
    {
    }
    public Timer(float scale, double time, string prefix, bool start = false) : this(Vector2.Zero, scale, time, prefix, start)
    {
    }

    public Timer(Vector2 position, float scale, double time, bool start = false): this(Vector2.Zero, scale, time, string.Empty, start)
    {

    }
    public Timer(Vector2 position, float scale, double time, string prefix, bool start = false)
    {
        _time = time;
        _prefix = prefix;
        _invoker = new OverTimeInvoker(time, start)
        {
            InvokeOnce = true
        };
        _invoker.Trigger += delegate
        {
            _invoked = true;
            Trigger?.Invoke();
        };

        _display = new BasicText($"{time/1000:n2}", position, scale * 2f);
    }

    public Rectangle Rectangle => _invoker.Rectangle;

    public void Update(GameTime gameTime)
    {
        if (_invoked)
        {
            _display.ChangeText(_prefix + "0.00");
        }
        else
        {
            var difference = (_time - _invoker.ExecutedTime) / 1000;
            _display.ChangeText($"{_prefix}{difference:n2}");
        }

        _invoker.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _display.Draw(spriteBatch);
    }

    public Vector2 GetPosition()
        => _display.GetPosition();

    public Vector2 GetSize()
        => _display.GetSize();

    public void Move(Vector2 newPosition)
        => _display.Move(newPosition);

    public void Start()
        => _invoker.Start();

    public void ChangeColor(Microsoft.Xna.Framework.Color[] input)
        => _display.ChangeColor(input);

    public int ColorLength()
        => _display.ColorLength();

    public Microsoft.Xna.Framework.Color[] GetColor()
    => _display.GetColor();

    public void Stop()
    {
        _invoked = false;
        _invoker.Stop();
    }

    public void Reset()
    {
        _invoked = false;
        _invoker.Reset();
    }

    public void SetScale(ScaleProvider provider)
    {
        _display.SetScale(provider);
    }
}