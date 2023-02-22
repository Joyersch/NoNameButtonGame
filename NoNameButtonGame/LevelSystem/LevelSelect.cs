using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons.TexturedButtons;
using NoNameButtonGame.GameObjects.Debug;
using NoNameButtonGame.LogicObjects.Listener;

namespace NoNameButtonGame.LevelSystem
{
    internal class LevelSelect : SampleLevel
    {
        private readonly List<MiniTextButton> _level;
        private readonly List<MiniTextButton> _down;
        private readonly List<MiniTextButton> _up;
        private readonly Cursor _mouseCursor;
        private readonly MousePointer _mousePointer;
        private readonly PositionListener _linker;
        public event Action<int> LevelSelectedEventHandler;
        private bool _isInMove;
        private bool _moveUp;
        private int _currentTicks;

        public LevelSelect(int defaultWidth, int defaultHeight, Vector2 window, Random rand, Storage.Storage storage) : base(
            defaultWidth, defaultHeight, window, rand)
        {
            Name = "Level Selection";

            _mouseCursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10));
            _mousePointer = new MousePointer();
            _linker = new PositionListener();
            _linker.Add(_mousePointer, _mouseCursor);
            
            int maxLevel = storage.GameData.MaxLevel;
            int screens = maxLevel / 30;
            _level = new List<MiniTextButton>();
            _down = new List<MiniTextButton>();
            _up = new List<MiniTextButton>();


            for (int i = 0; i < screens; i++)
            {
                var down = new MiniTextButton(
                    new Vector2(-300, 138 + (defaultHeight / Camera.Zoom) * i)
                    , new Vector2(64, 32)
                    , ""
                    , "⬇"
                    , new Vector2(16, 16));

                down.Click += MoveDown;

                _down.Add(down);

                var up = new MiniTextButton(
                    new Vector2(-300, 190 + (defaultHeight / Camera.Zoom) * i)
                    , new Vector2(64, 32)
                    , ""
                    , "⬆"
                    , new Vector2(16, 16));
                up.Click += MoveUp;

                _up.Add(up);
            }

            for (int i = 0; i < maxLevel; i++)
            {
                var levelButton = new MiniTextButton(
                    new Vector2(-200 + 100 * (i % 5), -140 + 50 * (i / 5) + 60 * (i / 30))
                    , new Vector2(64, 32)
                    , (i + 1).ToString()
                    , (i + 1).ToString()
                    , new Vector2(16, 16));

                levelButton.Click += SelectLevel;

                _level.Add(levelButton);
            }
        }

        private void SelectLevel(object sender)
            => LevelSelectedEventHandler?.Invoke(
                _level.IndexOf((MiniTextButton) sender) + 1 /*Index starts with 0 naming starts with 1*/);

        private void MoveDown(object sender)
        {
            _isInMove = true;
            _moveUp = false;
            _currentTicks = 40;
        }

        private void MoveUp(object sender)
        {
            _isInMove = true;
            _moveUp = true;
            _currentTicks = 40;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Note: foreach is slower to run than for but as there aren't that many levels yet this should be fine
            foreach (var levelButton in _level)
            {
                if (levelButton.Rectangle.Intersects(CameraRectangle))
                    levelButton.Draw(spriteBatch);
            }

            foreach (var down in _down)
            {
                if (down.Rectangle.Intersects(CameraRectangle))
                    down.Draw(spriteBatch);
            }

            foreach (var up in _up)
            {
                if (up.Rectangle.Intersects(CameraRectangle))
                    up.Draw(spriteBatch);
            }

            _mouseCursor.Draw(spriteBatch);
        }

        private float _savedGameTime;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            base.CurrentMusic(string.Empty);
            if (_isInMove)
            {
                _savedGameTime += (float) gameTime.ElapsedGameTime.TotalMilliseconds;
                while (_savedGameTime > 10)
                {
                    _savedGameTime -= 10;
                    Vector2 sinWaveRoute =
                        new Vector2(0, 12.2F * (float) Math.Sin((float) _currentTicks / 50 * Math.PI));
                    if (_moveUp)
                        CameraPosition -= sinWaveRoute;
                    else
                        CameraPosition += sinWaveRoute;
                    _currentTicks--;
                    if (_currentTicks == 0)
                    {
                        float alignmentOffset = CameraPosition.Y % (DefaultHeight / Camera.Zoom);
                        if (!_moveUp)
                            CameraPosition.Y += (DefaultHeight / Camera.Zoom) - alignmentOffset;
                        else
                            CameraPosition.Y -= alignmentOffset;
                        _isInMove = false;
                    }
                }
            }
            
            _mousePointer.Update(gameTime, MousePosition);
            _linker.Update(gameTime);
            _mouseCursor.Update(gameTime);
            
            // Note: foreach is lower to run than for but as there aren't that many levels yet this should be fine
            foreach (var levelButton in _level)
            {
                if (levelButton.Rectangle.Intersects(CameraRectangle))
                    levelButton.Update(gameTime, _mouseCursor.Hitbox[0]);
            }

            foreach (var down in _down)
            {
                if (down.Rectangle.Intersects(CameraRectangle))
                    down.Update(gameTime, _mouseCursor.Hitbox[0]);
            }

            foreach (var up in _up)
            {
                if (up.Rectangle.Intersects(CameraRectangle))
                    up.Update(gameTime, _mouseCursor.Hitbox[0]);
            }
        }
    }
}