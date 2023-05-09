using System;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.Interfaces;
using NoNameButtonGame.Logging;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level7;

public class Quiz : IManageable
{
    public Quiz(QuizQuestionsCollection questions)
    {
        foreach (var question in questions)
        {
            Log.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(question));
        }
    }

    public void Update(GameTime gameTime)
    {
        throw new NotImplementedException();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        throw new NotImplementedException();
    }

    public void DrawStatic(SpriteBatch spriteBatch)
    {
       Draw(spriteBatch);
    }
}