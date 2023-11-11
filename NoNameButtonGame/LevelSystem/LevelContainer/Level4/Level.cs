using System;
using Microsoft.Xna.Framework;
using MonoUtils;
using MonoUtils.Logic.Text;
using MonoUtils.Ui;
using MonoUtils.Ui.Logic;
using MonoUtils.Ui.Objects;
using NoNameButtonGame.LevelSystem.LevelContainer.Level3;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level4;

public class Level : SampleLevel
{
    private readonly Quiz quiz;

    public Level(Display display, Vector2 window, Random random) : base(display, window, random)
    {
        var textComponent = TextProvider.GetText("Levels.Level4");
        Name = "Level 4 - Old school";

        Camera.Zoom = 1F;
        var scale = 2F;
        Camera.Move(Display.Size / 2);

        string questions = textComponent.GetValue("Questions");
        var questionsCollection = Newtonsoft.Json.JsonConvert.DeserializeObject<QuizQuestionsCollection>(questions);
        quiz = new Quiz(questionsCollection, Camera.Rectangle, scale);
        quiz.Reset += Fail;
        quiz.Finish += Finish;
        AutoManaged.Add(quiz);

        var cursor = new Cursor(scale);
        Actuator = cursor;
        PositionListener.Add(Mouse, cursor);
        AutoManaged.Add(cursor);
    }
}