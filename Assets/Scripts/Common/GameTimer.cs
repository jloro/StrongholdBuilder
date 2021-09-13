using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    static public GameTimer instance;
    static public uint timeScale;
    static public float time { get; private set; }
    static public float timeBeforeNextWave {
        get
        {
            return _endTimer - time;
        }
    }

    static private float _endTimer;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        timeScale = 1;
        time = 0;
    }

    static public void ChangeTimeScale(uint scale)
    {
        Time.timeScale = scale;
        timeScale = scale;
        UiManager.instance.UpdateSpeed();
    }


    static public IEnumerator Wait_coroutine(float seconds)
    {
        _endTimer = time + seconds;
        while (time < _endTimer)
            yield return null;
    }

    private void FixedUpdate()
    {
        if (timeScale != 0)
            time += Time.deltaTime;
    }

    private void OnDestroy()
    {
        Time.timeScale = 1;
    }
}
