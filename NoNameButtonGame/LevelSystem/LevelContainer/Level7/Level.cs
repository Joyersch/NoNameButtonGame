using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using Microsoft.Xna.Framework;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.Logging;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level7;

public class Level : SampleLevel
{
    public const string QuestionPath = "Levels.Level7.Questions";

    private readonly Quiz quiz;

    public Level(Display.Display display, Vector2 window, Random random) : base(display, window, random)
    {
        Name = "Level 7 - Old school";
        
        var oneScreen = NoNameButtonGame.Display.Display.Size;
        Camera.Zoom = 1F;
        var scale = 2F;
        Camera.Move(oneScreen / 2);
        string questions = Globals.ReadFromResources(QuestionPath);
        QuizQuestionsCollection questionsCollection =
            Newtonsoft.Json.JsonConvert.DeserializeObject<QuizQuestionsCollection>(questions);

        quiz = new Quiz(questionsCollection, Camera.Rectangle, scale);
        quiz.Reset += Fail;
        quiz.Finish += QuizFinish;
        AutoManaged.Add(quiz);

        var cursor = new Cursor(scale);
        Actuator = cursor;
        PositionListener.Add(Mouse, cursor);
        AutoManaged.Add(cursor);
    }

    private void QuizFinish()
    {
        Log.WriteLine("Next thing here");
    }
}