using MonoUtils.Logging;
using MonoUtils.Sound;

namespace NoNameButtonGame.Music;

public static class DnB4
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
            Log.Warning("Music \"DnB4\" not initialized!");
            return;
        }
        _station.ResetVolume();
        _station.SetVolume(Statics.Music.Melody.Main, 1F);
        _station.SetVolume(Statics.Music.Melody.Main2, 0.5F);
        _station.SetVolume(Statics.Music.Drums.DnB, 0.8F);
        _station.SetVolume(Statics.Music.Bass.SynthwaveDnB, 0.4F);
    }
}