using MonoUtils.Logging;
using MonoUtils.Sound;

namespace NoNameButtonGame.Music;

public static class DnB
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
            Log.Warning("Music \"DnB\" not initialized!");
            return;
        }
        _station.ResetVolume();
        _station.SetVolume("main", 1F);
        _station.SetVolume("lead_dnb", 0.3F);
        _station.SetVolume("drums_dnb", 0.8F);
        _station.SetVolume("bass_synthwave_DnB", 0.4F);
    }
}