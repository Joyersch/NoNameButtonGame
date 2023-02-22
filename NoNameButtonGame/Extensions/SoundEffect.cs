using Audio = Microsoft.Xna.Framework.Audio;

namespace NoNameButtonGame.Extensions;

public static class SoundEffect
{
    public static Audio.SoundEffectInstance GetInstanceEx(this Audio.SoundEffect soundEffect, bool music)
        => Globals.SoundSettingsListener.Register(soundEffect.CreateInstance(), music);
}