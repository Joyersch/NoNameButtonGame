using System;
using System.Collections.Generic;
using MonoUtils.Console;
using NoNameButtonGame.Music;

namespace NoNameButtonGame.Commands;

public sealed class MusicCommand : ICommand
{
    [Command(Description = "Play specific music", Name = "music")]
    public IEnumerable<string> Execute(DevConsole caller, object[] options, ContextProvider context)
    {
        switch (options.Length)
        {
            case 0:
                return new[] { "music (name)" };
            case >= 2:
                return new[] { "music (name)" };
        }

        var sound = options[0].ToString();

        if (sound == "list")
            return Enum.GetNames<Sounds>();

        if (!Enum.TryParse<Sounds>(sound, true, out var expression))
            return new[] { "That sound does not exist!" };

        Action audio = expression switch
        {
            Sounds.Default => Default.Play,
            Sounds.Default2 => Default2.Play,
            Sounds.Default3 => Default3.Play,
            Sounds.DnB => DnB.Play,
            Sounds.DnB2 => DnB2.Play,
            Sounds.DnB3 => DnB3.Play,
            Sounds.DnB4 => DnB4.Play,
            Sounds.Lofi => Lofi.Play,
            Sounds.LofiMuffled => LofiMuffled.Play,
            Sounds.None => None.Play,
            Sounds.Memphis => Memphis.Play,
            Sounds.Synthwave => Synthwave.Play,
            Sounds.Trap => Trap.Play,
            Sounds.Trap2 => Trap2.Play,
            Sounds.Trance => Trance.Play,
            _ => None.Play
        };
        audio.Invoke();
        return new[] { $"Now playing {sound}" };
    }

    private enum Sounds
    {
        Default,
        Default2,
        Default3,
        DnB,
        DnB2,
        DnB3,
        DnB4,
        Lofi,
        LofiMuffled,
        None,
        Memphis,
        Synthwave,
        Trap,
        Trap2,
        Trance
    }
}