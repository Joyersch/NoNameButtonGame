﻿using MonoUtils.Logging;
using MonoUtils.Sound;

namespace NoNameButtonGame.Music;

public static class DnB
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
            Log.Warning("Music \"DnB\" not initialized!");
            return;
        }
        _station.ResetVolume();
        _station.SetVolume(Statics.Music.Melody.Main, 1F);
        _station.SetVolume(Statics.Music.Lead.DnB, 0.3F);
        _station.SetVolume(Statics.Music.Drums.DnB, 0.8F);
        _station.SetVolume(Statics.Music.Bass.SynthwaveDnB, 0.4F);
    }
}