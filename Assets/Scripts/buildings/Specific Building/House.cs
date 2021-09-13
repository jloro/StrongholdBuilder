using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : SA.Building
{
    [Header("House Settings")]
    [Tooltip("Population increase Per Level.")]
    [SerializeField] private List<uint> _ppl;

    public override void Placed()
    {
        base.Placed();
        ResourceManager.instance.IncreaseMaxPop(_ppl[0]);
    }

    public override void ImproveBuilding()
    {
        base.ImproveBuilding();
        ResourceManager.instance.IncreaseMaxPop(_ppl[1]);
    }

    public override void Destroy()
    {
        base.Destroy();
        for (int i = 0; i <= _lvl; i++)
            ResourceManager.instance.DecreaseMaxPop(_ppl[i]);
    }
}
