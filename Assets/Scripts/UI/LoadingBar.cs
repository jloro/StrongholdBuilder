using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{
    private const string endStr = " %</b>";
    private const string beginStr = "<b>";
    private Coroutine _timerRoutine;
    [SerializeField] private Slider _slider;
    [SerializeField] private Text _txt;
    public void CreateTimer(float time)
    {
        CancelTimer();
        _timerRoutine = StartCoroutine(MyTimer(time));

    }
    public void CancelTimer()
    {
        if (_timerRoutine != null)
            StopCoroutine(_timerRoutine);
    }
    private IEnumerator MyTimer(float maxTime)
    {
        float percent;
        float time = 0.0f;
        while (time < maxTime)
        {
            percent = time / maxTime;
            _txt.text = beginStr + (percent * 100.2).ToString("#0") + endStr;
            _slider.SetValueWithoutNotify(percent);
            time += Time.deltaTime * GameTimer.timeScale;
            yield return new WaitForEndOfFrame();
        }
    }
    /// <summary>
    /// Update manually the loading bar, not compaptible with Timer function
    /// a call to this function will cancel the timer
    /// </summary>
    public void UpdateValue(float percent)
    {
        if (_timerRoutine != null) { StopCoroutine(_timerRoutine); }
        _slider.SetValueWithoutNotify(percent);
        _txt.text = beginStr + (percent * 100).ToString("#0") + endStr;
    }
}
