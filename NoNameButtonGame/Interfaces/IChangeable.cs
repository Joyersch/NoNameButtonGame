using System;

namespace NoNameButtonGame.Interfaces;

public interface IChangeable
{
    public event EventHandler HasChanged;
}