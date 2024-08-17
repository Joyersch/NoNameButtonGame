using MonoUtils.Logging;
using MonoUtils.Sound;

namespace NoNameButtonGame.Music;

public static class Trap
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
            Log.Warning("Music \"Trap\" not initialized!");
            return;
        }
        _station.ResetVolume();
        _station.SetVolume(Statics.Music.Melody.Main, 1F);
        _station.SetVolume(Statics.Music.Drums.Trap, 1F);
        _station.SetVolume(Statics.Music.Bass.Trap, 0.3F);
    }
}