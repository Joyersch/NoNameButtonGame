using System;

namespace NoNameButtonGame.Interfaces
{
    internal interface IMouseActions
    {
        public event Action<object> Leave;
        public event Action<object> Enter;
        public event Action<object> Click;
    }
}
