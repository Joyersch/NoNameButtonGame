using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Audio;

namespace NoNameButtonGame.Cache;

public class SoundEffectsCache
{
    private Dictionary<string, SoundEffect> cache = new();

    public SoundEffectInstance GetInstance(string key)
        => cache.FirstOrDefault(x => x.Key == key).Value.CreateInstance();

    public bool AddMappingToCache(string key, SoundEffect effect)
        => cache.TryAdd(key, effect);
}