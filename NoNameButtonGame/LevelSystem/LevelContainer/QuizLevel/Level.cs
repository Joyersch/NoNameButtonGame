using System;
using Microsoft.Xna.Framework;
using MonoUtils;
using MonoUtils.Logic.Text;
using MonoUtils.Settings;
using MonoUtils.Sound;
using MonoUtils.Ui;
using NoNameButtonGame.LevelSystem.Settings;
using NoNameButtonGame.Music;

namespace NoNameButtonGame.LevelSystem.LevelContainer.QuizLevel;

public class Level : SampleLevel
{
    public Level(Scene scene, Random random, EffectsRegistry effectsRegistry,
        SettingsAndSaveManager<string> settingsAndSaveManager) : base(scene, random, effectsRegistry,
        settingsAndSaveManager)
    {
        var textComponent = TextProvider.GetText("Levels.QuizLevel");
        Name = textComponent.GetValue("Name");

        Default.Play();

        Camera.Move(Display.Size / 2);

        string questions = textComponent.GetValue("Questions");
        var questionsCollection = Newtonsoft.Json.JsonConvert.DeserializeObject<QuizQuestionsCollection>(questions);
        var quiz = new Quiz(questionsCollection, Camera.Rectangle, 1F);
        quiz.Reset += Fail;
        quiz.Finish += Finish;
        AutoManaged.Add(quiz);
        DynamicScaler.Register(quiz);
        DynamicScaler.Apply(Display.Scale);
    }
}