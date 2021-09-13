using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExploEventDisplayer : SkillDisplayer
{
    [SerializeField] private Text _title;
    [SerializeField] private Text _diff;
    [SerializeField] private Text _description;
    [SerializeField] private Text _eventType;
    [SerializeField] private Image _icon;
    [SerializeField] private Button _validate;
    [SerializeField] private Button _flee;
    [SerializeField] private Image _panel;
    private DataExplorationEvent _dataEvent;
    private ExplorationEvent _exploEvent;
    [SerializeField] private DataExplorationEvent debug;//
    private CanvasGroup _group;

    private void Awake()
    {
        _group = GetComponent<CanvasGroup>();
    }
    private void Start()
    {
        Utils.HideCanvasGroup(_group);
    }
    public void Validate()
    {
        if (_dataEvent == null) { return; }
        _exploEvent.TryEvent(out bool success);
        if (success)
        {
            _description.text = "SUCCESS" + System.Environment.NewLine + _dataEvent.successDesciption;
        }
        else
        {
            _description.text = "FAILURE" + System.Environment.NewLine + _dataEvent.failDesciption;
        }
        _validate.enabled = false;
        _flee.enabled = false;
    }
    public void Hide()
    {
        _panel.gameObject.SetActive(false);
    }
    public void DisplayEvent(ExplorationEvent exploEvent)
    {
        if (exploEvent == null ) { return; }
        _exploEvent = exploEvent;
        DisplayEvent(exploEvent.GetExploEvent());
    }
    public void LinkEvent(ExplorationEvent exploEvent)
    {
        _exploEvent = exploEvent;
    }
    public void DisplayEvent(DataExplorationEvent data)
    {
        if (data == null) { return; }
        Utils.ShowCanvasGroup(_group);
        _dataEvent = data;
        _panel.gameObject.SetActive(true);
        _title.text = _dataEvent.title;
        _diff.text = _dataEvent.score.ToString();
        _description.text = _dataEvent.description;
        _eventType.text = _dataEvent.eventType.ToString();
        _icon.sprite = _dataEvent.img;
        _validate.enabled = true;
        _flee.enabled = true;
    }
    public override void Display(ISkillData data)
    {
        DisplayEvent((DataExplorationEvent)data);
    }
}
