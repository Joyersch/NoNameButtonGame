using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using NoNameButtonGame.Logging;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level7;

public class QuizQuestionsCollection : List<QuizQuestionsCollection.QuizQuestion>
{
    public record QuizQuestion(string Question, QuizAnswer[] Answers);
    public record QuizAnswer(string Answer, bool IsCorrect);
}