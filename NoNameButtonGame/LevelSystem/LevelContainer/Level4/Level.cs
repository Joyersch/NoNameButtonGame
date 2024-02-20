using System;
using Microsoft.Xna.Framework;
using MonoUtils.Logic.Text;
using MonoUtils.Ui;
using MonoUtils.Ui.Objects;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level4;

public class Level : SampleLevel
{
    public Level(Display display, Vector2 window, Random random) : base(display, window, random)
    {
        var textComponent = TextProvider.GetText("Levels.Level4");
        Name = textComponent.GetValue("Name");

        Camera.Move(Display.Size / 2);

        string questions = textComponent.GetValue("Questions");
        var questionsCollection = Newtonsoft.Json.JsonConvert.DeserializeObject<QuizQuestionsCollection>(questions);
        var quiz = new Quiz(questionsCollection, Camera.Rectangle, 1F);
        quiz.Reset += Fail;
        quiz.Finish += Finish;
        AutoManaged.Add(quiz);
    }
}