using MonoUtils.Settings;

namespace NoNameButtonGame.LevelSystem.Settings;

public class AudioSettings : ISettings
{
    public float MusicVolume { get; set; } = 0.1F;

    public float SoundEffectVolume { get; set; } = 0.2F;
}