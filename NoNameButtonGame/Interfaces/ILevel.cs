using System;

namespace NoNameButtonGame.Interfaces
{
    internal interface ILevel
    {
        public event Action FailEventHandler;
        public event Action ExitEventHandler;
        public event Action FinishEventHandler;
    }
}
