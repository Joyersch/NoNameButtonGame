using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using NoNameButtonGame.Storage;

namespace NoNameButtonGame.LogicObjects.Listener;

public class SoundSettingsListener
{
    private readonly List<(SoundEffectInstance effect, bool music)> _instances;

    private Settings _settings;

    public SoundSettingsListener(Settings settings)
    {
        _instances = new List<(SoundEffectInstance, bool)>();
        _settings = settings;
        _settings.HasChanged += SettingsOnHasChanged;
    }

    private void SettingsOnHasChanged(object sender, EventArgs e)
    {
        // list of disposed effects. References to them have to be removed
        var toRemove = new List<(SoundEffectInstance, bool)>();
        foreach (var instance in _instances)
        {
            if (instance.effect.IsDisposed)
            {
                toRemove.Add(instance);
                continue;
            }

            if (instance.music)
                instance.effect.Volume = _settings.MusicVolume / 10F;
            else
                instance.effect.Volume = _settings.SfxVolume / 10F;
        }

        // Remove disposed effects.
        foreach (var effect in toRemove)
            _instances.Remove(effect);
    }

    public SoundEffectInstance Register(SoundEffectInstance soundEffect, bool music)
    {
        _instances.Add((soundEffect, music));
        SettingsOnHasChanged(_settings, EventArgs.Empty);
        return soundEffect;
    }
}