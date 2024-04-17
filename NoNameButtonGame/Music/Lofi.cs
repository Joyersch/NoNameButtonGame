using MonoUtils.Logging;
using MonoUtils.Sound;

namespace NoNameButtonGame.Music;

public static class Lofi
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
            Log.WriteWarning("Music \"Lofi\" not initialized!");
            return;
        }
        _station.ResetVolume();
        _station.SetVolume("lofi_main", 1F);
        _station.SetVolume("lofi_main2", 1F);
        _station.SetVolume("lofi_drums", 1F);
        _station.SetVolume("lofi_bass", 1F);
    }
}