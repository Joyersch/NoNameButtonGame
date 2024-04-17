using MonoUtils.Sound;

namespace NoNameButtonGame.Music;

public class None
{
    private static LoopStation _station = null;

    public static void Initialize(LoopStation station)
    {
        _station = station;
    }

    public static void Play()
    {
        if (_station is null)
            return;

        _station.ResetVolume();
    }
}