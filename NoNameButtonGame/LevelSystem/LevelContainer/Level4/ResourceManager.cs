using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoUtils.Logging;
using MonoUtils.Ui.Color;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level4;

public class ResourceManager
{
    private readonly Random _random;

    private readonly int[] _userItems;

    public readonly Resource.Type RareResource;
    private readonly int _commonResourceCount;

    private int _tradesPointer;

    private readonly List<ResourceTrade> _basicTrades;
    private readonly List<ResourceTrade> _advancedTrades;
    private readonly List<ResourceTrade> _superTrades;

    private readonly Dictionary<Guid, int> _assignedTrades;


    public ResourceManager(Random random, int villageCount)
    {
        _random = random;
        RareResource = (Resource.Type) random.Next(0, 8);

        ColorBuilder builder = new ColorBuilder();
        builder.AddColor(Color.White, 16);
        builder.AddColor(Color.Gold, RareResource.ToString().Length);
        Log.WriteColor($"RareResource => {RareResource}", builder.GetColor());

        /*Same length as field Count for Resource.Type*/
        int resourceCount = 8;
        _userItems = new int[resourceCount];
        _commonResourceCount = resourceCount - 1;

        for (int i = 0; i < _userItems.Length; i++)
        {
            if (i != (int) RareResource)
                _userItems[i] = 10;
        }

        _basicTrades = new List<ResourceTrade>();
        _advancedTrades = new List<ResourceTrade>();
        _superTrades = new List<ResourceTrade>();
        _assignedTrades = new Dictionary<Guid, int>();

        var rareTrade =
            new ResourceTrade(this, GetTradeValues(4, 9, new List<Resource.Type>()).ToList(),
                new() {new ResourceTrade.TradePosition(RareResource, 1)});

        rareTrade.TradeInitiated += HandleTrade;
        
        _basicTrades.Add(rareTrade);
        for (int i = 0; i < villageCount * 2 - 1; i++)
            _basicTrades.Add(GetResourceTrade(1, 1, 1, 1));

        for (int i = 0; i < villageCount * 2; i++)
            _advancedTrades.Add(GetResourceTrade(1, 1, 2, 1));

        for (int i = 0; i < villageCount * 2; i++)
            _superTrades.Add(GetResourceTrade(2, 1, 3, 1));
    }


    private ResourceTrade GetResourceTrade(int fromCount, int fromAmount, int toCount, int toAmount)
    {
        var blocked = new List<Resource.Type>();
        var resource =
            new ResourceTrade(this, GetTradeValues(fromCount, fromAmount, blocked).ToList(),
                GetTradeValues(toCount, toAmount, blocked).ToList());
        resource.TradeInitiated += HandleTrade;
        return resource;
    }

    private IEnumerable<ResourceTrade.TradePosition> GetTradeValues(int count, int amount,
        List<Resource.Type> blockedResources)
    {
        for (int i = 0; i < count; i++)
            yield return new ResourceTrade.TradePosition(GetRandomResource(blockedResources), amount);
    }

    private Resource.Type GetRandomResource(List<Resource.Type> blocked)
    {
        Resource.Type type;
        do
        {
            type = (Resource.Type) _random.Next(0, 8);
        } while (blocked.Any(r => r == type) || type == RareResource);

        blocked.Add(type);
        return type;
    }

    public int GetUserValue(Resource.Type type)
        => _userItems[(int) type];

    private void IncreaseUserValue(Resource.Type type, int value)
    {
        _userItems[(int) type] += value;

        if (_userItems[(int) type] > 99)
            _userItems[(int) type] = 99;
    }

    private void DecreaseUserValue(Resource.Type type, int value)
    {
        _userItems[(int) type] -= value;

        if (_userItems[(int) type] < 0)
            _userItems[(int) type] = 0;
    }

    public IEnumerable<ResourceTrade> GetTrades(Guid guid)
    {
        if (_assignedTrades.TryGetValue(guid, out int value))
        {
            yield return _basicTrades[value];
            yield return _basicTrades[value + 1];
            yield return _advancedTrades[value];
            yield return _advancedTrades[value + 1];
            yield return _superTrades[value];
            yield return _superTrades[value + 1];
        }

        _assignedTrades.Add(guid, _tradesPointer);

        yield return _basicTrades[_tradesPointer];
        yield return _basicTrades[_tradesPointer + 1];
        yield return _advancedTrades[_tradesPointer];
        yield return _advancedTrades[_tradesPointer + 1];
        yield return _superTrades[_tradesPointer];
        yield return _superTrades[_tradesPointer + 1];

        _tradesPointer += 2;
    }

    private void HandleTrade(ResourceTrade sender)
    {
        foreach (var fromItem in sender.FromItem)
            DecreaseUserValue(fromItem.Item, fromItem.Amount);


        foreach (var toItem in sender.ToItems)
            IncreaseUserValue(toItem.Item, toItem.Amount);
    }
}