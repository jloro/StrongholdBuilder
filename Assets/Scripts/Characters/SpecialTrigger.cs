using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] 
public class myTrigger : UnityEvent<Collider> {}
public class SpecialTrigger : MonoBehaviour
{
    [SerializeField]private bool _hasTriggerEnter;
    public myTrigger triggerEnter;
    [SerializeField] private bool _hasTriggerExit;
    public myTrigger triggerExit;
    [SerializeField] private bool _hasTriggerStay;
    public myTrigger triggerStay;

    private void Start()
    {
        _hasTriggerEnter = triggerEnter != null;
        _hasTriggerExit = triggerExit != null;
        _hasTriggerStay = triggerStay != null;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (_hasTriggerEnter)
            triggerEnter.Invoke(other);
    }
    private void OnTriggerExit(Collider other)
    {
        if (_hasTriggerExit)
            triggerExit.Invoke(other);
    }

    private void OnTriggerStay(Collider other)
    {
        if (_hasTriggerStay)
            triggerStay.Invoke(other);
    }
}
