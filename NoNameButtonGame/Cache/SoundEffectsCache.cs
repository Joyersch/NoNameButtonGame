using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Audio;

namespace NoNameButtonGame.Cache;

public class SoundEffectsCache
{
    private readonly Dictionary<string, (SoundEffect effect, bool isMusic)> _cache = new();

    public SoundEffectInstance GetInstance(string key, bool isMusic)
        => Extensions.SoundEffect.GetInstanceEx(GetEffect(key), isMusic);
    
    public SoundEffectInstance GetSfxInstance(string key)
        => Extensions.SoundEffect.GetInstanceEx(GetEffect(key), false);

    public SoundEffectInstance GetMusicInstance(string key)
        => Extensions.SoundEffect.GetInstanceEx(GetEffect(key), true);

    public SoundEffect GetEffect(string key)
        => _cache.FirstOrDefault(x => x.Key == key).Value.effect;

    public bool AddMappingToCache(string key, SoundEffect effect, bool isMusic)
        => _cache.TryAdd(key, (effect, isMusic));
    
    public bool AddSfxToCache(string key, SoundEffect effect)
        => _cache.TryAdd(key, (effect, false));
    
    public bool AddMusicToCache(string key, SoundEffect effect)
        => _cache.TryAdd(key, (effect, true));
}