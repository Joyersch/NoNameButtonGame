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
        _station.SetVolume(Statics.Music.Melody.Main, 0.1F);
        _station.SetVolume(Statics.Music.Melody.Main2, 1F);
        _station.SetVolume(Statics.Music.Kickdrum.Trance, 1F);
        _station.SetVolume(Statics.Music.Lead.Trance, 1F);
        _station.SetVolume(Statics.Music.Bass.Trance, 1F);
    }
}