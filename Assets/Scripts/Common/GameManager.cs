using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static public GameManager instance;
    public Selection selection;
    public LayerMask ignoreRaycast;
    public delegate void FreezeHandler(bool freeze);
    public event FreezeHandler FreezeEvent;
    public bool freezeStatus { get; private set; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void Pause()
    {
        if (FreezeEvent != null)
            FreezeEvent(true);
        freezeStatus = true;
        GameTimer.timeScale = 0;
        UiManager.instance.UpdateSpeed();
    }

    public void Play()
    {
        if (freezeStatus)
        {
            if (FreezeEvent != null)
                FreezeEvent(false);
            freezeStatus = false;
        }
        GameTimer.ChangeTimeScale(1);
    }

    public void FastForward()
    {
        if (freezeStatus)
        {
            if (FreezeEvent != null)
                FreezeEvent(false);
            freezeStatus = false;
        }
        GameTimer.ChangeTimeScale(2);
    }
}