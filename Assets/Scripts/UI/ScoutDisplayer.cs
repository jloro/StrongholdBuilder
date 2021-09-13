using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ScoutDisplayer : MonoBehaviour
{
    public Scout scout { get; protected set; }
    private Image icon;
    private Slider lpSlider;
    private TextMeshProUGUI lpTxt;
    //public Image icon;
    //public Slider lpSlider;

    void Awake()
    {
        icon = GetComponentInChildren<Image>();
        lpSlider = GetComponentInChildren<Slider>();
        lpTxt = lpSlider.GetComponentInChildren<TextMeshProUGUI>();
    }
    public void Display(Scout scout)
    {
        this.scout = scout;
        if (scout == null) 
        { 
            Hide();
            return;
        }
        if (scout.scoutType == eScoutType.soldier)
        {
            icon.sprite = ExploManager.instance.soldierIcon;
        }
        else 
        {
            icon.sprite = ExploManager.instance.scientistIcon;
        }
        UpdateDisplay();
    }
    public void Hide()
    {
        icon.sprite = null;
        lpSlider.gameObject.SetActive(false);
    }
    public void UpdateDisplay()
    {
        lpSlider.gameObject.SetActive(true);
        lpSlider.maxValue = scout.maxLp;
        lpSlider.value = scout.lp;
        lpTxt.text = scout.lp + " / " + scout.maxLp;
    }
}
