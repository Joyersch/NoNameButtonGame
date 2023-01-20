using System;
using System.Collections.Generic;
using System.Text;

namespace NoNameButtonGame.Interfaces
{
    interface IMouseActions
    {
        public event EventHandler LeaveEventHandler;
        public event EventHandler EnterEventHandler;
        public event EventHandler ClickEventHandler;
    }
}
