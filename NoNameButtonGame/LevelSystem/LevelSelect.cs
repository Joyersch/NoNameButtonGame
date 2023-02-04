﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NoNameButtonGame.Text;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.Hitboxes;

namespace NoNameButtonGame.LevelSystem.LevelContainer
{
    class LevelSelect : SampleLevel
    {
        readonly List<MiniTextButton> _level;
        readonly List<MiniTextButton> _down;
        readonly List<MiniTextButton> _up;
        readonly Cursor mouseCursor;
        public event Action<int> LevelSelectedEventHandler;
        bool isInMove = false;
        bool moveUp = false;
        int currentTicks = 0;

        public LevelSelect(int defaultWidth, int defaultHeight, Vector2 window, Random rand, Storage storage) : base(
            defaultWidth, defaultHeight, window, rand)
        {
            Name = "Level Selection";

            mouseCursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10));

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

                down.ClickEventHandler += MoveDown;

                _down.Add(down);

                var up = new MiniTextButton(
                    new Vector2(-300, 190 + (defaultHeight / Camera.Zoom) * i)
                    , new Vector2(64, 32)
                    , ""
                    , "⬆"
                    , new Vector2(16, 16));
                up.ClickEventHandler += MoveUp;

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

                levelButton.ClickEventHandler += SelectLevel;

                _level.Add(levelButton);
            }
        }

        private void SelectLevel(object sender)
            => LevelSelectedEventHandler?.Invoke(
                _level.IndexOf((MiniTextButton) sender) + 1 /*Index starts with 0 naming starts with 1*/);

        private void MoveDown(object sender)
        {
            isInMove = true;
            moveUp = false;
            currentTicks = 40;
        }

        private void MoveUp(object sender)
        {
            isInMove = true;
            moveUp = true;
            currentTicks = 40;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Note: foreach is slower to run than for but as there aren't that many levels yet this should be fine
            foreach (var levelButton in _level)
            {
                if (levelButton.rectangle.Intersects(cameraRectangle))
                    levelButton.Draw(spriteBatch);
            }

            foreach (var down in _down)
            {
                if (down.rectangle.Intersects(cameraRectangle))
                    down.Draw(spriteBatch);
            }

            foreach (var up in _up)
            {
                if (up.rectangle.Intersects(cameraRectangle))
                    up.Draw(spriteBatch);
            }

            mouseCursor.Draw(spriteBatch);
        }

        float savedGameTime;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (isInMove)
            {
                savedGameTime += (float) gameTime.ElapsedGameTime.TotalMilliseconds;
                while (savedGameTime > 10)
                {
                    savedGameTime -= 10;
                    Vector2 sinWaveRoute =
                        new Vector2(0, 12.2F * (float) Math.Sin((float) currentTicks / 50 * Math.PI));
                    if (moveUp)
                        cameraPosition -= sinWaveRoute;
                    else
                        cameraPosition += sinWaveRoute;
                    currentTicks--;
                    if (currentTicks == 0)
                    {
                        float alignmentOffset = cameraPosition.Y % (defaultHeight / Camera.Zoom);
                        if (!moveUp)
                            cameraPosition.Y += (defaultHeight / Camera.Zoom) - alignmentOffset;
                        else
                            cameraPosition.Y -= alignmentOffset;
                        isInMove = false;
                    }
                }
            }

            mouseCursor.Update(gameTime);
            mouseCursor.Position = mousePosition - mouseCursor.Size / 2;

            // Note: foreach is lower to run than for but as there aren't that many levels yet this should be fine
            foreach (var levelButton in _level)
            {
                if (levelButton.rectangle.Intersects(cameraRectangle))
                    levelButton.Update(gameTime, mouseCursor.Hitbox[0]);
            }

            foreach (var down in _down)
            {
                if (down.rectangle.Intersects(cameraRectangle))
                    down.Update(gameTime, mouseCursor.Hitbox[0]);
            }

            foreach (var up in _up)
            {
                if (up.rectangle.Intersects(cameraRectangle))
                    up.Update(gameTime, mouseCursor.Hitbox[0]);
            }
        }
    }
}