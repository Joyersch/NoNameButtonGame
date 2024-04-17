using MonoUtils.Logging;
using MonoUtils.Sound;

namespace NoNameButtonGame.Music;

public class Default3
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
            Log.WriteWarning("Music \"Default3\" not initialized!");
            return;
        }
        _station.ResetVolume();
        _station.SetVolume("main", 0.95F);
        _station.SetVolume("main2", 0.3F);
    }
}