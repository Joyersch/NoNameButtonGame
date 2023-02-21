using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Audio;

namespace NoNameButtonGame.Cache;

public class SoundEffectsCache
{
    private readonly Dictionary<string, SoundEffect> cache = new();

    public SoundEffectInstance GetInstance(string key)
        => GetEffect(key).CreateInstance();

    public SoundEffect GetEffect(string key)
        => cache.FirstOrDefault(x => x.Key == key).Value;

    public bool AddMappingToCache(string key, SoundEffect effect)
        => cache.TryAdd(key, effect);
}