using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchDisplayer : SkillDisplayer
{
    [SerializeField] private Text _title;
    [SerializeField] private Text _description;
    [SerializeField] private Image _icon;
    [SerializeField] private Text _wood;
    [SerializeField] private Text _stone;
    [SerializeField] private Text _food;
    [SerializeField] private CanvasGroup _canvasGroup;

    /// <summary>
    /// Display All information on non-null UI
    /// </summary>
    /// <param name="data"></param>
    public override void Display(ISkillData data)
    {
        DataResearch research = (DataResearch)data;
        if (research == null) { return; }
        if (_title != null)
            _title.text = research.title;
        if (_description != null)
            _description.text = research.description;
        if (_icon != null)
            _icon.sprite = research.img;
        ResourceCost cost = research.cost;
        if (_wood != null)
            _wood.text = "<b>" + cost.wood.ToString() + "</b>";
        if (_stone != null)
            _stone.text = "<b>" + cost.stone.ToString() + "</b>";
        if (_food != null)
            _food.text = "<b>" + cost.food.ToString() + "</b>";
    }
    public void Show()
    {
        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;
    }
    public void Hide()
    {
        _canvasGroup.alpha = 0f;
        _canvasGroup.blocksRaycasts = false;
    }
}
