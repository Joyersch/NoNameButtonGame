using MonoUtils.Logging;
using MonoUtils.Sound;

namespace NoNameButtonGame.Music;

public static class Trap
{
    private static LoopStation _station;

    public static void Initialize(LoopStation station)
    {
        _station = station;
    }

    public static void Play()
    {
        if (_station is null)
        {
            Log.Warning("Music \"Trap\" not initialized!");
            return;
        }
        _station.ResetVolume();
        _station.SetVolume("main", 1F);
        _station.SetVolume("drums_trap", 1F);
        _station.SetVolume("bass_trap", 0.3F);
    }
}