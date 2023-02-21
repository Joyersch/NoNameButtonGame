using System.Collections.Generic;
using Microsoft.Xna.Framework;
using NoNameButtonGame.GameObjects;

namespace NoNameButtonGame.LogicObjects.Linker;

public class GameObjectLinker
{
    private List<(GameObject main, GameObject sub)> mappings;

    public GameObjectLinker()
    {
        mappings = new List<(GameObject main, GameObject sub)>();
    }

    public void Update(GameTime gameTime)
        => mappings.ForEach(m => m.sub.Position = m.main.Position);

    public void Add(GameObject main, GameObject sub)
        => mappings.Add((main, sub));
}