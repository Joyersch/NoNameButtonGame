using System;

namespace NoNameButtonGame;

public interface IChangeable
{
    public event EventHandler HasChanged;
}