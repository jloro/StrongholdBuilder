using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonAction
{
    public Button.ButtonClickedEvent action;
    public Sprite sprite;
    public bool tooltip;
    private string _name;
    public string name { set { _name = value; Changetext(); } get { return _name; } }
    private string _desc;
    public string desc { set { _desc = value; Changetext(); } get { return _desc; } }
    public ResourceCost cost;
    public string text { get; private set; }

    public ButtonAction(UnityAction call, Sprite sprite, bool tooltip = false, string name = "", string desc = "", ResourceCost cost = new ResourceCost())
    {
        this.cost = cost;
        action = new Button.ButtonClickedEvent();
        action.AddListener(call);
        this.sprite = sprite;
        this.tooltip = tooltip;
        this.name = name;
        this.desc = desc;
    }

    private void Changetext()
    {
        text = $"{_name}\n";
        if (cost.IsFree())
            text += $"Free\n{_desc}";
        else
            text += $"w: {cost.wood} s: {cost.stone} f: {cost.food}\n{_desc}";
    }
}
