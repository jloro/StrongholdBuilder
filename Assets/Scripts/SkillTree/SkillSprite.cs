using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSprite : SkillNode
{
    protected SpriteRenderer _render;
    private Collider2D col;
    private void OnEnable()
    {
        _render = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }
    public override bool Unlock()
    {
        bool dst = base.Unlock();
        if (dst)
        {
            _render.enabled = dst;
            _render.color = _tree.unlockedColor;
            col.enabled = dst;
        }
        return dst;
    }
    protected override void Activate()
    {
        base.Activate();
        if (activated && !unlimitedUsage)
        {
            _render.color = _tree.activatedColor;
        }
    }
    public override void StartTimer()
    {
        base.StartTimer();
        if (!unlimitedUsage)
            col.enabled = false;
    }
    public override void Cancel()
    {
        base.Cancel();
        col.enabled = true;
    }
    protected void OnMouseDown()
    {
        AskConfirmation();
    }
}
