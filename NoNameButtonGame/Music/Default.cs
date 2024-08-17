using MonoUtils.Logging;
using MonoUtils.Sound;

namespace NoNameButtonGame.Music;

public static class Default
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
            Log.Warning("Music \"Default\" not initialized!");
            return;
        }
        _station.ResetVolume();
        _station.SetVolume(Statics.Music.Melody.Main, 1F);
    }
}