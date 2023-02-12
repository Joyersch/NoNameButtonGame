using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace NoNameButtonGame.LogicObjects;

public class MusicSettingsLinker
{
    private List<SoundEffectInstance> instances;

    private Settings _settings;

    public MusicSettingsLinker(Settings settings)
    {
        instances = new List<SoundEffectInstance>();
        _settings = settings;
        _settings.HasChanged += SettingsOnHasChanged;
    }

    private void SettingsOnHasChanged(object sender, EventArgs e)
    {
        List<SoundEffectInstance> toRemove = new List<SoundEffectInstance>();
        foreach (var effect in instances)
        {
            if (effect.IsDisposed)
            {
                toRemove.Add(effect);
                continue;
            }

            effect.Volume = _settings.MusicVolume / 10F;
        }

        foreach (var effect in toRemove)
            instances.Remove(effect);
    }

    public void AddSettingsLink(SoundEffectInstance soundEffect)
    {
        instances.Add(soundEffect);
        SettingsOnHasChanged(_settings, EventArgs.Empty);
    }
}