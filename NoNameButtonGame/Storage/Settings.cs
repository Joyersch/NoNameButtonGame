using System;
using NoNameButtonGame.Interfaces;

namespace NoNameButtonGame.Storage;

public class Settings : IChangeable
{
    private bool isFixedStep;

    public bool IsFixedStep
    {
        get => isFixedStep;
        set
        {
            isFixedStep = value;
            HasChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private bool isFullscreen;
    
    public bool IsFullscreen
    {
        get => isFullscreen;
        set
        {
            isFullscreen = value;
            HasChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private int musicVolume;
    
    public int MusicVolume
    {
        get => musicVolume;
        set
        {
            musicVolume = value;
            HasChanged?.Invoke(this, EventArgs.Empty);
        }
    }
    
    private int sfxVolume;
    
    public int SfxVolume
    {
        get => sfxVolume;
        set
        {
            sfxVolume = value;
            HasChanged?.Invoke(this, EventArgs.Empty);
        }
    }
    
    public Settings()
    {
        Resolution = new Resolution();
        Resolution.HasChanged += (_, _) => HasChanged?.Invoke(this, EventArgs.Empty);
    }

    public Resolution Resolution { get; set; }
    public event EventHandler HasChanged;
}