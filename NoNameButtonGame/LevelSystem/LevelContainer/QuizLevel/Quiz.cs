using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Logic.Management;
using MonoUtils.Ui;
using MonoUtils.Ui.TextSystem;
using NoNameButtonGame.GameObjects.Buttons;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace NoNameButtonGame.LevelSystem.LevelContainer.QuizLevel;

public class Quiz : IManageable, IInteractable, IScaleable
{
    private QuizQuestionsCollection _questions;
    public Rectangle Rectangle { get; private set; }

    private Button _buttonOne;
    private Button _buttonTwo;
    private Button _buttonThree;

    private Text _question;

    private int _questionsPointer;

    private QuizQuestionsCollection.QuizQuestion CurrentQuestion => _questions[_questionsPointer];
    private QuizQuestionsCollection.QuizAnswer[] CurrentAnswers => CurrentQuestion.Answers;

    private bool ButtonOneIsCorrect => CurrentAnswers[0].IsCorrect;
    private bool ButtonTwoIsCorrect => CurrentAnswers[1].IsCorrect;
    private bool ButtonThreeIsCorrect => CurrentAnswers[2].IsCorrect;


    public event Action Reset;
    public event Action Finish;

    public Quiz(QuizQuestionsCollection questions, Rectangle rectangle, float scale)
    {
        _questions = questions;
        Rectangle = rectangle;
        if (questions.Any(q => q.Answers.Length > 3))
            Log.Warning("A question has more answers that supported!");

        _question = new Text(string.Empty, scale * Text.DefaultLetterScale);
        _question.InRectangle(this)
            .OnCenter()
            .OnY(3, 10)
            .Centered()
            .Apply();

        _buttonOne = new Button(string.Empty);
        _buttonOne.Click += ButtonOneClick;
        _buttonOne.InRectangle(this)
            .OnCenter()
            .OnX(5, 20)
            .Centered()
            .Apply();

        _buttonTwo = new Button(string.Empty);
        _buttonTwo.Click += ButtonTwoClick;
        _buttonOne.InRectangle(this)
            .OnCenter()
            .Centered()
            .Apply();

        _buttonThree = new Button(string.Empty);
        _buttonThree.Click += ButtonThreeClick;
        _buttonThree.InRectangle(this)
            .OnCenter()
            .OnY(15, 20)
            .Centered()
            .Apply();
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

        _question.InRectangle(this)
            .OnCenter()
            .OnY(3, 10)
            .Centered()
            .Apply();
        _question.Update(gameTime);

        _buttonOne.InRectangle(this)
            .OnCenter()
            .OnX(5, 20)
            .Centered()
            .Apply();
        _buttonOne.Update(gameTime);

        _buttonTwo.InRectangle(this)
            .OnCenter()
            .Centered()
            .Apply();
        _buttonTwo.Update(gameTime);

        _buttonThree.InRectangle(this)
            .OnCenter()
            .OnX(15, 20)
            .Centered()
            .Apply();
        _buttonThree.Update(gameTime);
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

    private void SetQuestionText()
    {
        _question.ChangeText(CurrentQuestion.Question);
        _buttonOne.Text.ChangeText(CurrentAnswers[0].Answer);
        _buttonTwo.Text.ChangeText(CurrentAnswers[1].Answer);
        _buttonThree.Text.ChangeText(CurrentAnswers[2].Answer);
    }

    public float Scale { private set; get; }
    public void SetScale(float scale)
    {
        Scale = scale;
        _question.SetScale(scale);
        _buttonOne.SetScale(scale);
        _buttonTwo.SetScale(scale);
        _buttonThree.SetScale(scale);
    }
}