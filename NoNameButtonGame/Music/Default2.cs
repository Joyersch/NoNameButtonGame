using MonoUtils.Logging;
using MonoUtils.Sound;

namespace NoNameButtonGame.Music;

public class Default2
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
            Log.WriteWarning("Music \"Default2\" not initialized!");
            return;
        }
        _station.ResetVolume();
        _station.SetVolume("main2", 1F);
    }
}