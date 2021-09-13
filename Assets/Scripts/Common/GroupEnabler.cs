using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupEnabler : MonoBehaviour
{
    [SerializeField] private GameObject[] _group;

    public void ActivateGroupe()
    {
        ToggleGroup(true);
    }
    public void DisactivateGroup()
    {
        ToggleGroup(false);
    }
    public void ToggleGroup(bool state)
    {
        if (_group == null) { return; }
        for (int i = 0; i < _group.Length; ++i)
        {
            _group[i].SetActive(state);
        }
    }
}
