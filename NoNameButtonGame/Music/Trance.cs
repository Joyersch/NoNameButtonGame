using MonoUtils.Logging;
using MonoUtils.Sound;

namespace NoNameButtonGame.Music;

public static class Trance
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
            Log.Warning("Music \"Trance\" not initialized!");
            return;
        }
        _station.ResetVolume();
        _station.SetVolume("main", 0.1F);
        _station.SetVolume("main2", 1F);
        _station.SetVolume("kickdrum_trance", 1F);
        _station.SetVolume("lead_trance", 1F);
        _station.SetVolume("bass_trance", 1F);
    }
}