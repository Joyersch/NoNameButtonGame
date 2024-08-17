using MonoUtils.Logging;
using MonoUtils.Sound;

namespace NoNameButtonGame.Music;

public static class Memphis
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
            Log.Warning("Music \"Memphis\" not initialized!");
            return;
        }
        _station.ResetVolume();
        _station.SetVolume("main", 1F);
        _station.SetVolume("ride_memphis", 1F);
        _station.SetVolume("percussion_memphis", 1F);
        _station.SetVolume("drums_memphis", 1F);
        _station.SetVolume("bass_memphis", 1F);
    }
}