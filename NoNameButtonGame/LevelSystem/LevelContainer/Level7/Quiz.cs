using System;
using System.Data;
using System.Linq;
using System.Net.Mime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.Extensions;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.TextSystem;
using NoNameButtonGame.Interfaces;
using NoNameButtonGame.Logging;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level7;

public class Quiz : IManageable, IInteractable
{
    private QuizQuestionsCollection _questions;
    private Rectangle _area;

    private TextButton _buttonOne;
    private TextButton _buttonTwo;
    private TextButton _buttonThree;

    private Text _question;

    private int _questionsPointer;

    private QuizQuestionsCollection.QuizQuestion CurrentQuestion => _questions[_questionsPointer];
    private QuizQuestionsCollection.QuizAnswer[] CurrentAnswers => CurrentQuestion.Answers;

    private bool ButtonOneIsCorrect => CurrentAnswers[0].IsCorrect;
    private bool ButtonTwoIsCorrect => CurrentAnswers[1].IsCorrect;
    private bool ButtonThreeIsCorrect => CurrentAnswers[2].IsCorrect;


    public event Action Reset;
    public event Action Finish;

    public Quiz(QuizQuestionsCollection questions, Rectangle area, float scale)
    {
        _questions = questions;
        _area = area;
        if (questions.Any(q => q.Answers.Length > 3))
            throw new DataException("To many answers");

        _question = new Text(string.Empty, scale);
        _buttonOne = new TextButton(string.Empty, scale, 0.5F * scale);
        _buttonOne.Click += ButtonOneClick;
        _buttonTwo = new TextButton(string.Empty, scale, 0.5F * scale);
        _buttonTwo.Click += ButtonTwoClick;
        _buttonThree = new TextButton(string.Empty, scale, 0.5F * scale);
        _buttonThree.Click += ButtonThreeClick;
        _question.GetCalculator(_area).OnCenter().OnY(3,10).Centered().Move();
        _buttonOne.GetCalculator(_area).OnCenter().OnX(5,20).Centered().Move();
        _buttonOne.GetCalculator(_area).OnCenter().Centered().Move();
        _buttonThree.GetCalculator(_area).OnCenter().OnY(15,20).Centered().Move();
    }

    private void ButtonOneClick(object obj)
    {
        if (ButtonOneIsCorrect)
            _questionsPointer++;
        else
            Reset?.Invoke();
    }
    
    private void ButtonTwoClick(object obj)
    {
        if (ButtonTwoIsCorrect)
            _questionsPointer++;
        else
            Reset?.Invoke();
    }
    
    private void ButtonThreeClick(object obj)
    {
        if (ButtonThreeIsCorrect)
            _questionsPointer++;
        else
            Reset?.Invoke();
    }

    public void Update(GameTime gameTime)
    {
        if (_questionsPointer >= _questions.Count)
        {
            Finish?.Invoke();
            _questionsPointer = 0;
        }
        
        SetQuestionText();
        _question.Update(gameTime);
        _question.GetCalculator(_area).OnCenter().OnY(3,10).Centered().Move();
        _buttonOne.Update(gameTime);
        _buttonOne.GetCalculator(_area).OnCenter().OnX(5,20).Centered().Move();
        _buttonTwo.Update(gameTime);
        _buttonTwo.GetCalculator(_area).OnCenter().Centered().Move();
        _buttonThree.Update(gameTime);
        _buttonThree.GetCalculator(_area).OnCenter().OnX(15,20).Centered().Move();
    }

    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        _buttonOne.UpdateInteraction(gameTime, toCheck);
        _buttonTwo.UpdateInteraction(gameTime, toCheck);
        _buttonThree.UpdateInteraction(gameTime, toCheck);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _question.Draw(spriteBatch);
        _buttonOne.Draw(spriteBatch);
        _buttonTwo.Draw(spriteBatch);
        _buttonThree.Draw(spriteBatch);
    }

    public void DrawStatic(SpriteBatch spriteBatch)
    {
        _question.DrawStatic(spriteBatch);
        _buttonOne.DrawStatic(spriteBatch);
        _buttonTwo.DrawStatic(spriteBatch);
        _buttonThree.DrawStatic(spriteBatch);
    }

    private void SetQuestionText()
    {
        _question.ChangeText(CurrentQuestion.Question);
        _buttonOne.Text.ChangeText(CurrentAnswers[0].Answer);
        _buttonTwo.Text.ChangeText(CurrentAnswers[1].Answer);
        _buttonThree.Text.ChangeText(CurrentAnswers[2].Answer);
    }
}