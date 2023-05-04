using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using NoNameButtonGame.Logging;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level7;

public class QuizQuestionsCollection : List<QuizQuestionsCollection.QuizQuestion>
{
    public record QuizQuestion(string Question, string[] Answers, int CorrectAnswer);

    public QuizQuestionsCollection()
    {
        Add(new QuizQuestion(
                "5 + 12"
                , new [] {
                    "17",
                    "19",
                    "18"
                }
                , 0
            ));
    }
}