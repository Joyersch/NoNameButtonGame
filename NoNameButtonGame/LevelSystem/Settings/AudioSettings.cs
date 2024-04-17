using MonoUtils.Settings;

namespace NoNameButtonGame.LevelSystem.Settings;

public class AudioSettings : ISettings
{
    public float MusicVolume { get; set; } = 0.3F;

    public float SoundEffectVolume { get; set; } = 0.5F;
}