using System;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level12.Overworld;

public interface ILocation
{
    public string GetName();
    public Guid GetGuid();
}