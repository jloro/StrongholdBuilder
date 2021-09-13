using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : SA.Building
{
    [Header("Wall Settings")]

    [SerializeField] private int _maxHpAdd;
    public override void ImproveBuilding()
    {
        base.ImproveBuilding();
        _hpMax += _maxHpAdd;
        _hp = _hpMax;
    }
}
