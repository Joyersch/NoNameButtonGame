using System;
using System.Collections.Generic;
using System.Text;
namespace NoNameButtonGame.Interfaces
{
    interface ILevel
    {
        public event EventHandler FailEventHandler;
        public event EventHandler ExitEventHandler;
        public event EventHandler FinishEventHandler;
    }
}
