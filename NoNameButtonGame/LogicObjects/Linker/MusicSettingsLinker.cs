using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using NoNameButtonGame.Storage;

namespace NoNameButtonGame.LogicObjects.Linker;

public class MusicSettingsLinker
{
    private readonly List<SoundEffectInstance> instances;

    private readonly Settings settings;

    public MusicSettingsLinker(Settings settings)
    {
        instances = new List<SoundEffectInstance>();
        this.settings = settings;
        this.settings.HasChanged += SettingsOnHasChanged;
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

            effect.Volume = settings.MusicVolume / 10F;
        }

        foreach (var effect in toRemove)
            instances.Remove(effect);
    }

    public void AddSettingsLink(SoundEffectInstance soundEffect)
    {
        instances.Add(soundEffect);
        SettingsOnHasChanged(settings, EventArgs.Empty);
    }
}