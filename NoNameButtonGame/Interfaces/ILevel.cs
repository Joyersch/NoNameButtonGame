using System;
using System.Collections.Generic;
using System.Text;
namespace NoNameButtonGame.Interfaces
{
    interface ILevel
    {
        public event EventHandler Fail;
        public event EventHandler Reset;
        public event EventHandler Finish;
    }
}
