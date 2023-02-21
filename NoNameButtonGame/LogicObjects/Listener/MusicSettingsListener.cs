using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using NoNameButtonGame.Storage;

namespace NoNameButtonGame.LogicObjects.Listener;

public class MusicSettingsListener
{
    private readonly List<SoundEffectInstance> _instances;

    private readonly Settings _settings;

    public MusicSettingsListener(Settings settings)
    {
        _instances = new List<SoundEffectInstance>();
        _settings = settings;
        _settings.HasChanged += SettingsOnHasChanged;
    }

    private void SettingsOnHasChanged(object sender, EventArgs e)
    {
        List<SoundEffectInstance> toRemove = new List<SoundEffectInstance>();
        foreach (var effect in _instances)
        {
            if (effect.IsDisposed)
            {
                toRemove.Add(effect);
                continue;
            }

            effect.Volume = _settings.MusicVolume / 10F;
        }

        foreach (var effect in toRemove)
            _instances.Remove(effect);
    }

    public void AddSettingsLink(SoundEffectInstance soundEffect)
    {
        _instances.Add(soundEffect);
        SettingsOnHasChanged(_settings, EventArgs.Empty);
    }
}