using System;

namespace NoNameButtonGame.LevelSystem
{
    internal interface ILevel
    {
        public event Action FailEventHandler;
        public event Action ExitEventHandler;
        public event Action FinishEventHandler;
    }
}
