using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using NoNameButtonGame.Storage;

namespace NoNameButtonGame.LogicObjects.Linker;

public class SoundSettingsLinker
{
    private List<SoundEffectInstance> instances;

    private Settings _settings;

    public SoundSettingsLinker(Settings settings)
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
            effect.Volume = _settings.SfxVolume / 10F;
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