using System;
using System.IO;
using System.Text.Json;
using Microsoft.Xna.Framework;
using NoNameButtonGame.Logging;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level7;

public class Level : SampleLevel
{
    public const string QuestionPath = "Data\\Levels\\Level7\\Questions.json";

    private readonly Quiz quiz;
    
    public Level(Display.Display display, Vector2 window, Random random) : base(display, window, random)
    {
        if (!File.Exists(QuestionPath))
        {
            Log.WriteLine("Missing questions file!");
            return;
        }

        string questions = string.Empty;
        QuizQuestionsCollection questionsCollection = null;
        using (StreamReader streamReader = new StreamReader(QuestionPath))
        {
            questions = streamReader.ReadToEnd();
        }

        try
        {
            questionsCollection = Newtonsoft.Json.JsonConvert.DeserializeObject<QuizQuestionsCollection>(questions);
        }
        catch (Exception exception)
        {
            Log.WriteLine(exception.Message);
            return;
        }
        
        quiz = new Quiz(questionsCollection);
    }
}