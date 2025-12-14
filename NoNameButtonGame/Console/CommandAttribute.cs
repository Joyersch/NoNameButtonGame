using System;

namespace NoNameButtonGame.Console;

[AttributeUsage(AttributeTargets.Method)]
public sealed class CommandAttribute : Attribute
{
    public string Name { get; set; }
    public string Description { get; set; }
}