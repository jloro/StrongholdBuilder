using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : SkillNode
{
    public Color unlockedColor = new Color(1, 1, 1, 1);
    public Color lockedColor = new Color(0.75f, 0.75f, 0.75f, 255);
    public Color activatedColor = new Color(0.0f, 0.75f, 0.0f, 255);
    public Button _button;
    protected override void Start()
    {
        if (_button == null) { _button = GetComponent<Button>(); }
        base.Start();
        _button.onClick.AddListener(AskConfirmation);
        _button.interactable = unlocked;
        //GetComponent<Image>().color = (unlocked) ? Color.cyan : Color.grey;
    }
    public override bool Unlock()
    {
        bool dst = base.Unlock();
        if (dst)
        {
            GetComponent<Image>().color = unlockedColor;
            if (!timerStarted)
            {
                _button.interactable = dst; 
            }
        }

        return dst;
    }
    protected override void Activate()
    {
        base.Activate();
        if (activated)
        {
            GetComponent<Image>().color = activatedColor;
        }
    }
}
