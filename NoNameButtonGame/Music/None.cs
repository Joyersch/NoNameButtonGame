using MonoUtils.Sound;

namespace NoNameButtonGame.Music;

public static class None
{
    private static LoopStation _station;

    public static void Initialize(LoopStation station)
    {
        _station = station;
    }

    public static void Play()
        => _station?.ResetVolume();
}