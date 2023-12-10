using System.Runtime.Serialization;
using MonoUtils.Settings;

namespace NoNameButtonGame.LevelSystem.Settings;

public class AudioSettings : ISettings
{
    public float MusicVolume { get; set; } = 1F;

    public float SoundEffectVolume { get; set; } = 1F;
}