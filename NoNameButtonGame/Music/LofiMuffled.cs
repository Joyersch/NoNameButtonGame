using MonoUtils.Logging;
using MonoUtils.Sound;

namespace NoNameButtonGame.Music;

public static class LofiMuffled
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
            Log.Warning("Music \"Lofi Muffled\" not initialized!");
            return;
        }
        _station.ResetVolume();
        _station.SetVolume(Statics.Music.Melody.LoFiMainMuffled, 0.8F);
        _station.SetVolume(Statics.Music.Melody.LoFiMain2Muffled, 0.8F);
        _station.SetVolume(Statics.Music.Drums.LoFiMuffled, 0.8F);
        _station.SetVolume(Statics.Music.Bass.LoFiMuffled, 0.8F);
    }
}