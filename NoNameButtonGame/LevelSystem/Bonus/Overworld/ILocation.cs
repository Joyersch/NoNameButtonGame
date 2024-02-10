using System;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Bonus.Overworld;

public interface ILocation
{
    public string GetName();
    public Guid GetGuid();
}