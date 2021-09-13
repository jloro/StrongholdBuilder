using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct ResourceCost 
{
    public int wood;
    public int food;
    public int stone;

    public ResourceCost(int qtt)
    {
        if (qtt < 0) { qtt = 0; }
        wood = qtt;
        food = qtt;
        stone = qtt;
    }
    public ResourceCost(ResourceCost cost)
    {
        this.wood = cost.wood;
        this.food = cost.food;
        this.stone = cost.stone;
    }
    public ResourceCost(int wood = 0, int food = 0, int stone = 0)
    {
        if (wood < 0) { wood = 0; }
        if (food < 0) { food = 0; }
        if (stone < 0) { stone = 0; }
        this.wood = wood;
        this.food = food;
        this.stone = stone;
    }

    public static ResourceCost operator +(ResourceCost a, ResourceCost b)
    {
        return new ResourceCost(a.wood + b.wood, a.food + b.food, a.stone + b.stone);
    }
    /// <summary>
    /// RessourceCost never have negative value. Here 2 - 5 = 0
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static ResourceCost operator -(ResourceCost a, ResourceCost b)
    {
        return new ResourceCost(a.wood - b.wood, a.food - b.food, a.stone - b.stone);
    }
    public bool IsFree() { return (wood == 0 && food == 0 && stone == 0); }

    public bool CanAfford() { return (wood <= ResourceManager.instance.wood && stone <= ResourceManager.instance.stone && food <= ResourceManager.instance.food); }
}
