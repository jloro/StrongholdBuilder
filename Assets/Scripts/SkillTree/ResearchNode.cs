using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchNode : SkillButton
{
    [SerializeField] private DataResearch _researchData;
    protected override void Start()
    {
        base.Start();
        cost = _researchData.cost;
        _button.image.sprite = _researchData.img;
        time = _researchData.time;
    }
    public override void AskConfirmation()
    {
        _tree.dislayer.Display(_researchData);
        base.AskConfirmation();
    }
    public override void GetConfirmation(bool answer)
    {
        base.GetConfirmation(answer);

        if (answer)
            ((ResearchTree)_tree).DisplayOnGoingResearch(_researchData);
        
    }
    public override void Cancel()
    {
        base.Cancel();
        ((ResearchTree)_tree).StopOnGoingResearch();
    }
}
