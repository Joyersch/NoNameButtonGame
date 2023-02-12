using System;
using System.Collections.Generic;
using System.Text;
using NoNameButtonGame.Input;
using NoNameButtonGame.Camera;
using NoNameButtonGame.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NoNameButtonGame.Cache;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.Text;

namespace NoNameButtonGame.LevelSystem.LevelContainer;

internal class Level9 : SampleLevel
{
    
    private Cursor _cursor;

    private TextButton yellowButton;
    private TextButton redButton;
    private TextButton greenButton;
    private TextButton blueButton;
    private TextButton purpleButton;
    private readonly string block;

    private TextBuilder sequenzDisplay;
    private int[] sequenz;
    private bool playingSequence = false;
    private int clickedSequenceCount = 0;
    private int playedSequenceCount = 0;

    private const int maximumSequenceLength = 5;

    public Level9(int defaultWidth, int defaultHeight, Vector2 window, Random random) : base(defaultWidth,
        defaultHeight, window, random)
    {
        Name = "Level 9 - I HATE THIS LEVEL";
        _cursor = new Cursor(Vector2.One, Cursor.DefaultSize);
        sequenz = new int[maximumSequenceLength];
        for (int i = 0; i < maximumSequenceLength; i++)
        {
            sequenz[i] = random.Next(0, 5);
        }

        sequenzDisplay = new TextBuilder(string.Empty, new Vector2(-40, 128), TextButton.DefaultTextSize, 0);
        block  = Letter.ReverseParse(Letter.Character.Full).ToString();
        yellowButton = new TextButton(new Vector2(-320, -32), "0", block);
        yellowButton.Text[0].ChangeColor(Color.Orange);
        redButton = new TextButton(new Vector2(-192, -32), "1", block);
        redButton.Text[0].ChangeColor(Color.DarkRed);
        greenButton = new TextButton(new Vector2(-64, -32), "2", block);
        greenButton.Text[0].ChangeColor(Color.Green);
        blueButton = new TextButton(new Vector2(64, -32), "3", block);
        blueButton.Text[0].ChangeColor(Color.Blue);
        purpleButton = new TextButton(new Vector2(192, -32), "4", block);
        purpleButton.Text[0].ChangeColor(Color.Purple);
    }

    private void PlaySequence()
    {
        
    }
    
    public override void Update(GameTime gameTime)
    {
        if (playingSequence)
            PlaySequence();
        else
        {
            yellowButton.Update(gameTime);
            redButton.Update(gameTime);
            greenButton.Update(gameTime);
            blueButton.Update(gameTime);
            purpleButton.Update(gameTime);
        }

        _cursor.Update(gameTime);
        _cursor.Position = mousePosition - _cursor.Size / 2;
    }
    
    public override void Draw(SpriteBatch spriteBatch)
    {
        yellowButton.Draw(spriteBatch);
        redButton.Draw(spriteBatch);
        greenButton.Draw(spriteBatch);
        blueButton.Draw(spriteBatch);
        purpleButton.Draw(spriteBatch);
        _cursor.Draw(spriteBatch);
    }
}