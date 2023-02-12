using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using NoNameButtonGame.GameObjects;

namespace NoNameButtonGame.Cache;

public struct TextureHitboxMapping
{
    public Texture2D Texture;
    public Vector2 ImageSize;
    public Rectangle[] Hitboxes;
    public int AnimationsFrames;
    public bool? AnimationFromTop;
}

public class TextureCache
{
    private Dictionary<Type, TextureHitboxMapping> cache = new();

    public TextureHitboxMapping GetMappingFromCache<T>()
        => cache.FirstOrDefault(x => x.Key == typeof(T)).Value;

    public bool AddMappingToCache(Type type, TextureHitboxMapping textureHitboxMapping)
        => cache.TryAdd(type, textureHitboxMapping);
    
}