using System;
using System.Collections.Generic;
using System.Linq;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level12;

public class ResourceTrade
{
    private readonly ResourceManager _manager;
    public readonly List<TradePosition> FromItem;
    public readonly List<TradePosition> ToItems;

    public event Action<ResourceTrade> TradeInitiated;

    public record TradePosition(Resource.Type Item, int Amount);

    public ResourceTrade(ResourceManager manager, List<TradePosition> fromItem,
    List<TradePosition> toItems)
    {
        _manager = manager;
        FromItem = fromItem;
        ToItems = toItems;
    }

    public bool CanTrade()
        => FromItem.All(i => _manager.GetUserValue(i.Item) >= i.Amount);

    public void Trade()
    {
        if (!CanTrade())
            return;
        
        TradeInitiated?.Invoke(this);
    }
}