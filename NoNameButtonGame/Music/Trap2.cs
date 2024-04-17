using MonoUtils.Logging;
using MonoUtils.Sound;

namespace NoNameButtonGame.Music;

public class Trap2
{
    private static LoopStation _station = null;

    public static void Initialize(LoopStation station)
    {
        _station = station;
    }

    public static void Play()
    {
        if (_station is null)
        {
            Log.WriteWarning("Music \"Trap2\" not initialized!");
            return;
        }
        _station.ResetVolume();
        _station.SetVolume("main", 1F);
        _station.SetVolume("main2", 0.5F);
        _station.SetVolume("drums_trap", 1F);
        _station.SetVolume("bass_trap", 0.3F);
    }
}