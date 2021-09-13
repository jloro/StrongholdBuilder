using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eScoutType{scientist =0, soldier = 1 }
public enum eScoutUpgrade : uint
{ 
    none = 0,
    soldierEfficient = 1 << 1,
    soldier
}
public class Scout //: MonoBehaviour
{
    public eScoutType scoutType { get; protected set; }
    public int lp { get; protected set; }
    public int maxLp { get; protected set; }

    protected Dice _combatDice;
    protected Dice _searchDice;
    public Scout(eScoutType type)
    {
        scoutType = type;
        if (type == eScoutType.soldier)
        {
            maxLp = lp = 6;
            _combatDice = new Dice(6);
            _searchDice = new Dice(2);
        }
        else
        {
            maxLp = lp = 4;
            _combatDice = new Dice(2);
            _searchDice = new Dice(6);
        }
    }

    public int Search()
    {
        return _searchDice.Roll();
    }
    public int Fight()
    {
        return _combatDice.Roll();
    }
    public void Heal()
    {
        lp = maxLp;
    }
    public void Heal(int amount)
    {
        lp += amount;
        if (lp > maxLp) { lp = maxLp; }
    }
    /// <summary>
    /// reduce the acual life point of the Scout by the amount.
    /// </summary>
    /// <param name="amount"></param>
    /// <returns>true if dead, false otherwise</returns>
    public bool TakeDmg(int amount)
    {
        lp -= amount;
        if (lp <= 0)
        {
            lp = 0;
            return true;
        }
        return false;
    }
}
