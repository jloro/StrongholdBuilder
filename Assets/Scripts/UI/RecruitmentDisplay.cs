using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecruitmentDisplay : MonoBehaviour
{
    public LoadingBar loadingBar;
    [SerializeField] private Image[] _recruits;
    [SerializeField] private Image _trainingRecruit;
    [SerializeField] private UnitData _bowmanData;
    [SerializeField] private UnitData _infantryData;
    [SerializeField] private CanvasGroup _canvasGroup;

    public void DisplayQueue(Queue<eUnit> queue, eUnit trainingUnit)
    {
        DisplayTrainingUnit(trainingUnit);
        DisplayQueue(queue);
    }
    public void DisplayQueue(Queue<eUnit> queue)
    {
        Queue<eUnit> copy; 
        if (queue != null)
        {
           copy = new Queue<eUnit>(queue);
        }
        else
        {
            copy = new Queue<eUnit>();
        }

        for (int i = 0; i < _recruits.Length; i++)
        {
            if (copy.Count > 0)
                SetSprite(_recruits[i], copy.Dequeue());
            else
                SetSprite(_recruits[i], eUnit.none);
        }
    }
    public void DisplayTrainingUnit(eUnit trainingUnit)
    {
        SetSprite(_trainingRecruit, trainingUnit);
        if (trainingUnit != eUnit.none)
        {
            loadingBar.CreateTimer(Barrack.trainingTime);
            Show();
        }
        else
        {
            Hide();
        }
    }
    public void DisplayBarrack(Barrack barrack)
    {
        DisplayQueue(barrack.spawnQueue, barrack.currentRecruit);
        loadingBar.UpdateValue(barrack.recruitMentProgression);
    }
    public void Hide()
    {
        Utils.HideCanvasGroup(_canvasGroup);
    }
    public void Show()
    {
        Utils.ShowCanvasGroup(_canvasGroup);
    }
    private UnitData GetUnitData(eUnit unit)
    {
        if (unit == eUnit.infantry) { return _infantryData; }
        else if (unit == eUnit.archer) { return _bowmanData; }
        else { return null; }
    }
    private void SetSprite(Image img, UnitData unit)
    {
        if (unit == null)
        {
            img.enabled = false;
        }
        else
        {
            img.enabled = true;
            img.sprite = unit.sprite;
        }
    }
    private void SetSprite(Image img, eUnit unit)
    {
        if (unit == eUnit.archer)
        {
            img.enabled = true;
            img.sprite = _bowmanData.sprite;
        }
        else if (unit == eUnit.infantry)
        {
            img.enabled = true;
            img.sprite = _infantryData.sprite;
        }
        else
        {
            img.enabled = false;
        }
    } 
}
