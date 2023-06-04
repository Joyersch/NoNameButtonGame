using System;
using Microsoft.Xna.Framework;
using MonoUtils;
using MonoUtils.Logic;
using MonoUtils.Objects;
using MonoUtils.Ui;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level3;

public class Level : SampleLevel
{
    public const string QuestionPath = "Levels.Level7.Questions";

    private readonly Quiz quiz;

    private readonly OverTimeMover _toSimonMover;

    public Level(Display display, Vector2 window, Random random) : base(display, window, random)
    {
        Name = "Level 7 - Old school";

        var oneScreen = Display.Size;
        Camera.Zoom = 1F;
        var scale = 2F;
        Camera.Move(oneScreen / 2);
        string questions = Global.ReadFromResources(QuestionPath);
        QuizQuestionsCollection questionsCollection =
            Newtonsoft.Json.JsonConvert.DeserializeObject<QuizQuestionsCollection>(questions);

        quiz = new Quiz(questionsCollection, Camera.Rectangle, scale);
        quiz.Reset += Fail;
        quiz.Finish += QuizFinish;
        AutoManaged.Add(quiz);

        var simonArea = Camera.Rectangle;
        simonArea.X += Camera.Rectangle.Width;

        _toSimonMover = new OverTimeMover(Camera, simonArea.Center.ToVector2(), 555F,
            OverTimeMover.MoveMode.Sin);
        AutoManaged.Add(_toSimonMover);
        
        var simon = new SimonSays(simonArea, random, 10);
        AutoManaged.Add(simon);
        
        var cursor = new Cursor(scale);
        Actuator = cursor;
        PositionListener.Add(Mouse, cursor);
        AutoManaged.Add(cursor);
    }

    private void QuizFinish()
    {
        _toSimonMover.Start();
    }
}