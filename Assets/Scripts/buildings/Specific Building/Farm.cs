using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : SA.Building
{
    [Header("Farm Settings")]

    [SerializeField] private int _maxWorkersAdd;
    private GameObject _slot;

    #region UnityMethods

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.FarmSlot))
        {
            onSlot = true;
            _slot = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.FarmSlot))
        {
            onSlot = false;
            _slot = null;
        }
    }

    #endregion

    #region PublicMethods

    public override void Placed()
    {
        base.Placed();
        _slot.SetActive(false);
    }

    public override void Destroy()
    {
        base.Destroy();
        _slot.SetActive(true);
    }

    public override void Destroy(float time)
    {
        base.Destroy(time);
        _slot.SetActive(true);
    }

    public override void ImproveBuilding()
    {
        base.ImproveBuilding();
        GetComponent<EarnRessources>().AddMaxWorker(_maxWorkersAdd);
    }
    #endregion
}
