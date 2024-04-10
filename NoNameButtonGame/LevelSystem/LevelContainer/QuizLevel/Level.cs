using System;
using Microsoft.Xna.Framework;
using MonoUtils.Logic.Text;
using MonoUtils.Sound;
using MonoUtils.Ui;

namespace NoNameButtonGame.LevelSystem.LevelContainer.QuizLevel;

public class Level : SampleLevel
{
    public Level(Display display, Vector2 window, Random random, EffectsRegistry effectsRegistry) : base(display, window, random, effectsRegistry)
    {
        var textComponent = TextProvider.GetText("Levels.QuizLevel");
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