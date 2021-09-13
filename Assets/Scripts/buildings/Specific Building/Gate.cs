using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : SA.Building
{
    [Header("Gate Settings")]
    [SerializeField] private GameObject _gateOpen;
    [SerializeField] private GameObject _gateClose;
    [SerializeField] private List<GameObject> _poles;
    private Renderer _gateOpenRenderer;
    private Renderer _gateCloseRenderer;
    [SerializeField]private cakeslice.Outline _openOutline;
    [SerializeField]private cakeslice.Outline _closeOutline;
    private bool _close;
    [SerializeField]private Sprite spriteOpenDoor;

    #region UnityMethods
    private void Awake()
    {
        _close = false;
        _gateCloseRenderer = _gateClose.GetComponent<Renderer>();
        _gateOpenRenderer = _gateOpen.GetComponent<Renderer>();
        renderer = _gateOpenRenderer;
    }

    override protected void OnEnable()
    {
        base.OnEnable();
        AddAction(ChangeStateDoor, spriteOpenDoor);
    }

    #endregion

    #region PublicMethods

    public void ChangeStateDoor()
    {
        _close = !_close;
        _gateOpen.SetActive(!_close);
        _gateClose.SetActive(_close);
        if (_gateClose.activeSelf)
            renderer = _gateCloseRenderer;
        else
            renderer = _gateOpenRenderer;
        
        if (null != buildingSound) {
                    CommonSounds.inst.audioSource.PlayOneShot(buildingSound, 0.7F);
        }
    }

    override public void ActiveOutline(bool state)
    {
            _closeOutline.enabled = state;
            _openOutline.enabled = state;
    }
    public override void Placed()
    {
        base.Placed();
        foreach (GameObject go in _poles)
            go.SetActive(true);
    }
    #endregion
}
