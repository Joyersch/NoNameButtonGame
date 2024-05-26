using MonoUtils.Logging;
using MonoUtils.Sound;

namespace NoNameButtonGame.Music;

public class LofiMuffled
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
            Log.Warning("Music \"Lofi Muffled\" not initialized!");
            return;
        }
        _station.ResetVolume();
        _station.SetVolume("lofi_main_muffled", 0.8F);
        _station.SetVolume("lofi_main2_muffled", 0.8F);
        _station.SetVolume("lofi_drums_muffled", 0.8F);
        _station.SetVolume("lofi_bass_muffled", 0.8F);
    }
}