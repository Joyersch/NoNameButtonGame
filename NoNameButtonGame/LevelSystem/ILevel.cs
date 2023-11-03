using System;

namespace NoNameButtonGame.LevelSystem
{
    internal interface ILevel
    {
        public event Action OnFail;
        public event Action OnExit;
        public event Action OnFinish;
    }
}
