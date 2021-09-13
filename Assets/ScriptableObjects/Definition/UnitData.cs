using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/UnitData")]

public class UnitData : ScriptableObject
{
    /* "bv" mean base value, the value at the begenning, if change at rentime, must be reset at the begenning of a new start*/
    public int woodCost { get { return cost.wood; } }
    public int stoneCost { get { return cost.stone; } }
    public int foodCost { get { return cost.food; } }
    public GameObject prefab;
    public Sprite sprite;
    public Sprite spriteStop;
    public int maxHp;
    public int damage;
    public ResourceCost cost;

    [SerializeField] private int maxHp_bv;
    [SerializeField] private int damage_bv;

    private void OnEnable()
    {
        Reset();
    }
    public void Reset()
    {
        maxHp = maxHp_bv;
        damage = damage_bv;
    }
    public void UpgradeHealth()
    {
        maxHp += 5;
    }
    public void UpgradeDamage()
    {
        damage += 1;
    }
}
