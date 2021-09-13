using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sawmill : SA.Building
{
    [Header("Sawmill Settings")]

    [SerializeField] private int _maxWorkersAdd;
    private GameObject _slot;
    private SawmillSlot _s;

    #region UnityMethods

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.SawmillSlot))
        {
           _slot = other.gameObject;
            _s = _slot.GetComponent<SawmillSlot>();
           if (_s.CanPlace()) {
               onSlot = true;
           }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.SawmillSlot))
        {
            onSlot = false;
            _slot = null;
        }
    }

    #endregion

    #region PublicMethods

    public override void Placed()
    {
        if (_s.AddSawmill()) {
            base.Placed();
        }
    }

    public override void Destroy()
    {
        if(_s.RemoveSawmill()) {
            base.Destroy();
        }
    }

    public override void Destroy(float time)
    {
        if(_s.RemoveSawmill()) {
            base.Destroy(time);
        }
    }

    public override void ImproveBuilding()
    {
        base.ImproveBuilding();
        GetComponent<EarnRessources>().AddMaxWorker(_maxWorkersAdd);
    }
    #endregion
}
