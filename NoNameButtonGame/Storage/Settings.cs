using System;
using NoNameButtonGame.Interfaces;

namespace NoNameButtonGame.Storage;

public class Settings : IChangeable
{
    private bool _isFixedStep;

    public bool IsFixedStep
    {
        get => _isFixedStep;
        set
        {
            _isFixedStep = value;
            HasChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private bool _isFullscreen;
    
    public bool IsFullscreen
    {
        get => _isFullscreen;
        set
        {
            _isFullscreen = value;
            HasChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private int _musicVolume;
    
    public int MusicVolume
    {
        get => _musicVolume;
        set
        {
            _musicVolume = value;
            HasChanged?.Invoke(this, EventArgs.Empty);
        }
    }
    
    private int _sfxVolume;
    
    public int SfxVolume
    {
        get => _sfxVolume;
        set
        {
            _sfxVolume = value;
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