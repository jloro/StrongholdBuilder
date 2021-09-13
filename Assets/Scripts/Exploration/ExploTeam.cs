using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploTeam 
{
    public List<Scout> team { get; protected set; }
    public static int maxSize { get; protected set; }
    public int actualSize { get { return team.Count; } }
    private static float _speed = 5.0f;
    public float teamSpeed { get; protected set; }
    public ResourceCost loot{get; protected set;}
    public Vector3 position;
    public static float speed = 1.0f;
    public GameObject icon;
    public ExplorationEvent currentEvent = null;


    public ExploTeam(int nbScientist, int nbSoldier, GameObject icon )
    {
        if (maxSize == 0) { maxSize = 6; }

        if (nbScientist < 0 || nbSoldier < 0) 
        { throw new System.ArgumentOutOfRangeException("nbSoldier and nbScientis must be at least zero"); }
        if (nbSoldier + nbScientist > maxSize)
        { throw new System.ArgumentOutOfRangeException("nbSoldier and nbScientis cannot exeed maxSize"); }
        
        team = new List<Scout>(maxSize);
        for (int i = 0; i < nbScientist; ++i)
        {
            team.Add(new Scout(eScoutType.scientist));
        }
        for (int i = 0; i < nbSoldier; ++i)
        {
            team.Add(new Scout(eScoutType.soldier));
        }
        loot = new ResourceCost(0);
        teamSpeed = _speed;
        this.icon = icon;
    }
    public int Fight()
    {
        int dst = 0;
        foreach (Scout scout in team)
        {
            dst += scout.Fight();
        }
        return dst;
    }
    public int Search()
    {
        int dst = 0;
        foreach (Scout scout in team)
        {
            dst += scout.Search();
        }
        return dst;
    }
    public void AddResources(ResourceCost resources)
    {
        loot += resources;
    }
    public void StealResources(ResourceCost amount)
    {
        loot -= amount;
    }
    public bool AddMember(Scout member)
    {
        if (team.Count < maxSize)
        {
            team.Add(member);
            return true;
        }
        return false;
    }

    public void HealOne()
    {
        if (team == null|| team.Count <= 0)
        {
            return;
        }
        var lst = team.FindAll(x => x.lp < x.maxLp);
        if (lst != null && lst.Count > 0)
        {
            Scout scout = lst[Random.Range(0, lst.Count)];
            scout.Heal();
        }
    }
    public void HealOne(Scout scout)
    {
        scout?.Heal();
    }
    public void HealOne(Scout scout, int amount)
    {
        scout?.Heal(amount);
    }
    public void HealAll()
    {
        foreach (Scout scout in team)
        {
            scout.Heal();
        }
    }
    public void HealAll(int amount)
    {
        foreach (Scout scout in team)
        {
            scout.Heal(amount);
        }
    }
    public void KillAMember()
    {
        team.RemoveAt(Random.Range(0, team.Count));
    }
    public void TakeDmg(int min, int max)
    {
        foreach (Scout scout in team)
        {
            scout.TakeDmg(Random.Range(min, max +1));
        }
    }
}
