using System;
using System.Collections.Generic;
using System.Text;

namespace NoNameButtonGame.Interfaces
{
    interface IMouseActions
    {
        public event EventHandler Leave;
        public event EventHandler Enter;
        public event EventHandler Click;
    }
}
