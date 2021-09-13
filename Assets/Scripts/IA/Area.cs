using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    [SerializeField]
    private Transform[] _childrens;
    public eDirection direction;
    private void Awake()
    {
        if (_childrens == null || _childrens.Length == 0)
            _childrens = GetComponentsInChildren<Transform>();
    }
    public Transform[] GetSpawAreas()
    {
        return _childrens;
    }
}
