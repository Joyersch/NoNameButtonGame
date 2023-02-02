using System;
using System.Collections.Generic;
using System.Text;

namespace NoNameButtonGame.Interfaces
{
    interface IMouseActions
    {
        public event Action<object> LeaveEventHandler;
        public event Action<object> EnterEventHandler;
        public event Action<object> ClickEventHandler;
    }
}
