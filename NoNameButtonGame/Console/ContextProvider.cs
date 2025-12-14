using System.Collections.Generic;
using System.Linq;

namespace NoNameButtonGame.Console;

public sealed class ContextProvider
{
    private Dictionary<string, object> Context { get; set; } = new();

    public void RegisterContext(string index, object context)
    {
        if (Context.All(d => d.Key != index))
            Context.Add(index, context);
        else
            Context[index] = context;
    }

    public T? GetValue<T> (string index)
        => (T?) Context
            .Where(d => d.Key == index)
            .Select(d => d.Value).FirstOrDefault();
}