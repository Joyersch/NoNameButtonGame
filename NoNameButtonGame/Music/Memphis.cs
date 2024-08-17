using MonoUtils.Logging;
using MonoUtils.Sound;

namespace NoNameButtonGame.Music;

public static class Memphis
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
            Log.Warning("Music \"Memphis\" not initialized!");
            return;
        }
        _station.ResetVolume();
        _station.SetVolume(Statics.Music.Melody.Main, 1F);
        _station.SetVolume(Statics.Music.Ride.Memphis, 1F);
        _station.SetVolume(Statics.Music.Percussion.Memphis, 1F);
        _station.SetVolume(Statics.Music.Drums.Memphis, 1F);
        _station.SetVolume(Statics.Music.Bass.Memphis, 1F);
    }
}