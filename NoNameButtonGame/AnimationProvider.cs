using Microsoft.Xna.Framework;
using NoNameButtonGame;

namespace Joyersch.Monogame;

public sealed class AnimationProvider : IUpdateable
{
    private readonly Vector2 _imageSize;
    private readonly Rectangle _framePosition;
    private readonly bool _animatedFromTop;
    private readonly int _animationFrames;
    private int _currentAnimationFrame;

    public Rectangle ImageLocation { get; private set; }

    private readonly OverTimeInvoker _animationInvoker;

    public AnimationProvider(Vector2 imageSize, double animationSpeed, int animationFrames,
        bool animatedFromTop = true) : this(imageSize, animationSpeed, animationFrames,
        new Rectangle(Vector2.Zero.ToPoint(), imageSize.ToPoint()), animatedFromTop)
    {
    }

    public AnimationProvider(Vector2 imageSize, double animationSpeed, int animationFrames, Rectangle framePosition,
        bool animatedFromTop = true, int startingFrame = 0)
    {
        _imageSize = imageSize;
        _animationFrames = animationFrames;
        _animatedFromTop = animatedFromTop;
        _animationInvoker = new OverTimeInvoker(animationSpeed);
        _framePosition = framePosition;
        CalculateImageLocation();
        if (animationSpeed != 0F)
            _animationInvoker.Trigger += CalculateImageLocation;
        _currentAnimationFrame = startingFrame;
    }

    public void Update(GameTime gameTime)
    {
        _animationInvoker.Update(gameTime);
    }

    private void CalculateImageLocation()
    {
        if (_animationFrames < 1)
            return;

        _currentAnimationFrame++;
        if (_currentAnimationFrame >= _animationFrames)
            _currentAnimationFrame = 0;

        ImageLocation = new Rectangle(
            !_animatedFromTop
                ? (int)(_imageSize.X * _currentAnimationFrame + _framePosition.X)
                : 0,
            _animatedFromTop ? (int)(_imageSize.Y * _currentAnimationFrame + _framePosition.Y) : 0,
            _framePosition.Width,
            _framePosition.Height);
    }

    public int GetCurrentFrame()
        => _currentAnimationFrame;
}