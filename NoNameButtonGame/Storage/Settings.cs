using System;
using NoNameButtonGame.Interfaces;

namespace NoNameButtonGame;

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
    
    private int _SFXVolume;
    
    public int SFXVolume
    {
        get => _SFXVolume;
        set
        {
            _SFXVolume = value;
            HasChanged?.Invoke(this, EventArgs.Empty);
        }
    }
    
    public Settings()
    {
        Resolution = new Resolution();
        Resolution.HasChanged += (sender, args) => HasChanged?.Invoke(this, EventArgs.Empty);
    }

    public Resolution Resolution { get; set; }
    public event EventHandler HasChanged;
}