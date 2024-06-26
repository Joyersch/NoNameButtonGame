using System.Collections.Generic;

namespace NoNameButtonGame.LevelSystem.LevelContainer.QuizLevel;

public class QuizQuestionsCollection : List<QuizQuestionsCollection.QuizQuestion>
{
    public record QuizQuestion(string Question, QuizAnswer[] Answers);

    public record QuizAnswer(string Answer, bool IsCorrect);
}