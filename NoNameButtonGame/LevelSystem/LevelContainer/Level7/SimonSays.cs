using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.Extensions;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.Interfaces;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level7;

public class SimonSays : IManageable, IInteractable
{
    public Rectangle Rectangle { get; private set; }

    private SimonSaysButton[] _buttons = new SimonSaysButton[5];
    private TextButton _start;

    public SimonSays(Rectangle area)
    {
        Rectangle = area;
        _buttons[0] = new SimonSaysButton(Color.Yellow);
        _buttons[1] = new SimonSaysButton(Color.Red);
        _buttons[2] = new SimonSaysButton(Color.Green);
        _buttons[3] = new SimonSaysButton(Color.Blue);
        _buttons[4] = new SimonSaysButton(Color.Purple);
        int i = 0;
        foreach(var button in _buttons)
            button.GetCalculator(area).OnCenter().OnX(i++ * 0.2F + 0.1F).Centered().Move();

        _start = new TextButton("Start", 2F);
        _start.GetCalculator(area).OnCenter().OnY(1,3).Centered().Move();
        _start.Click += StartClick;
    }

    private void StartClick(object obj)
    {
        throw new System.NotImplementedException();
    }

    public void Update(GameTime gameTime)
    {
        foreach(var button in _buttons)
            button.Update(gameTime);
        _start.Update(gameTime);
    }
    
    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        foreach(var button in _buttons)
            button.UpdateInteraction(gameTime, toCheck);
        _start.UpdateInteraction(gameTime, toCheck);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach(var button in _buttons)
            button.Draw(spriteBatch);
        _start.Draw(spriteBatch);
    }

    public void DrawStatic(SpriteBatch spriteBatch)
    {
        foreach(var button in _buttons)
            button.DrawStatic(spriteBatch);
        _start.DrawStatic(spriteBatch);
    }
}