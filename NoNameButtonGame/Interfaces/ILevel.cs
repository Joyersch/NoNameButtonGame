using System;
using System.Collections.Generic;
using System.Text;
namespace NoNameButtonGame.Interfaces
{
    interface ILevel
    {
        public event Action FailEventHandler;
        public event Action ExitEventHandler;
        public event Action FinishEventHandler;
    }
}
