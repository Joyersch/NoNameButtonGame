using MonoUtils.Logging;
using MonoUtils.Sound;

namespace NoNameButtonGame.Music;

public static class Lofi
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
            Log.Warning("Music \"Lofi\" not initialized!");
            return;
        }
        _station.ResetVolume();
        _station.SetVolume(Statics.Music.Melody.LoFiMain, 1F);
        _station.SetVolume(Statics.Music.Melody.LoFiMain2, 1F);
        _station.SetVolume(Statics.Music.Drums.LoFi, 1F);
        _station.SetVolume(Statics.Music.Bass.LoFi, 1F);
    }
}