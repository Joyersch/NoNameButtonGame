using System;

namespace NoNameButtonGame.Console;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class CommandOptionsAttribute : Attribute
{
    public string Name { get; set; }
    public string RootOptionName { get; set; }
    public int Depth { get; set; }
    public string Description { get; set; }
}