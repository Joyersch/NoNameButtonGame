using System;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level4.Overworld;

public interface ILocation
{
    public string GetName();
    public Guid GetGuid();
}