using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Joyersch.Monogame.Console;
using Joyersch.Monogame.Ui;
using NoNameButtonGame.Ui;

namespace NoNameButtonGame.Console;

public sealed class CommandProcessor : IProcessor
{
    public List<(CommandAttribute Attribute, CommandOptionsAttribute[] Options, ICommand Command)> Commands { get; } =
        new();

    public void Initialize()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        string @return = null;
        IEnumerable<Type> commands = null;
        foreach (var assembly in assemblies)
        {
            var classes = assembly.GetTypes().Where(t =>
                t.IsClass && t.GetMethods()
                    .Any(m => m.GetCustomAttribute<CommandAttribute>() is not null));
            commands = commands is null ? classes : commands.Concat(classes);
        }

        foreach (var command in commands)
        {
            var commandInstance = (ICommand)Activator.CreateInstance(command)!;
            var methods = command.GetMethods();

            foreach (var method in methods)
            {
                var attribute = method.GetCustomAttribute<CommandAttribute>();
                var options = method.GetCustomAttributes<CommandOptionsAttribute>().ToArray();
                if (attribute is null)
                    continue;

                Commands.Add((attribute, options, commandInstance));
            }
        }
    }

    public IEnumerable<string> Process(DevConsole caller, string fullCommand, ContextProvider context)
    {
        var commandSplit = fullCommand.Split(" ");

        if (Commands.All(c => c.Attribute.Name != commandSplit[0]))
            return new[] { "This command does not exist!" };

        var command = Commands.FirstOrDefault(c => c.Attribute.Name == commandSplit[0]);

        var options = new object[commandSplit.Length - 1];
        for (int i = 0; i < options.Length; i++)
            options[i] = commandSplit[i + 1];

        return command.Command.Execute(caller, options, context);
    }

    public string? PossibleMatch(string search)
    {
        if (!search.Contains(' '))
        {
            return Commands.FirstOrDefault(c => c.Attribute.Name.ToLower().StartsWith(search.ToLower()),
                (new CommandAttribute() { Name = string.Empty }, [],
                    null)!).Attribute.Name.ToLower();
        }

        string[] split = search.Split(" ");

        if (Commands.All(c => !c.Attribute.Name.ToLower().StartsWith(split[0].ToLower())))
            return null;

        var entry = Commands.First(c => c.Attribute.Name.ToLower().StartsWith(split[0].ToLower()));

        foreach (var options in entry.Options)
        {
            if (split.Length - 1 != options.Depth
                || !options.Name.ToLower().StartsWith(split[options.Depth].ToLower())
                || split[options.Depth].Length >= options.Name.Length
                || (options.RootOptionName is not null && split[options.Depth - 1].ToLower() != options.RootOptionName.ToLower()))
                continue;

            string[] @return = new string[split.Length - 1];
            Array.Copy(split, @return, split.Length - 1);
            return string.Join(' ', @return) + ' ' + options.Name;
        }

        return null;
    }
}