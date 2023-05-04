using System;
using System.Reflection;
using NoNameButtonGame.Logging;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level7;

public class Quiz
{
    public static QuizQuestionsCollection questions = new();
    public Quiz()
    {
        foreach (var question in questions)
        {
            Log.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(question));
        }
    }
}