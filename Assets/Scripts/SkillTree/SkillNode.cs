using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class SkillNode : MonoBehaviour
{
    public UnityEvent action;
    public ResourceCost cost = new ResourceCost(0);
    public SkillNode[] parents;
    public SkillNode[] childs;
    
    [Tooltip("base time to activate : in secondes")]
    public float time = 10.0f;//in seconds
    public float timeLeft { get; protected set; }
    public bool unlocked;// { get; private set;  }
    public bool activated { get; private set; }
    public bool timerStarted { get; protected set; }
    public bool unlimitedUsage;
    protected Coroutine _routine = null;
    protected  SkillTree _tree;

    protected virtual void Start()
    {
        if (parents == null || parents.Length == 0)
        {
            unlocked = true;
        }
        _tree = GetComponentInParent<SkillTree>();
        timerStarted = false;
    }
    public virtual bool Unlock()
    {
        if (parents == null || parents.Length == 0 || unlocked == true)
        {
            unlocked = true;
        }
        else
        {
            unlocked = true;
            for (int i = 0; i < parents.Length; i++) 
            {
                //if (node == null) { continue; }
                if (!parents[i].activated)
                {
                   unlocked = false;
                   break;
                }
            }
        }
        return unlocked;
    }
    public virtual void StartTimer()
    {
        if (!unlocked || timerStarted || (activated && !unlimitedUsage)
          || (!cost.IsFree() && !ResourceManager.instance.CanPlace(cost)))
        { return; }
        if (!cost.IsFree())
        { ResourceManager.instance.ConsumeResource(cost); }
        timerStarted = true;
        _tree.Lock();
        StopTimerRoutine();
        _routine = StartCoroutine(TimerCoroutine());
    }
    protected virtual IEnumerator TimerCoroutine()
    {
        timeLeft = time;
        while (timeLeft > 0.0f)
        {
            timeLeft -= Time.deltaTime * _tree.currentSpeed;
            yield return null;
        }
        Activate();
    }
    protected virtual void Activate()
    {
        activated = true;
        _tree.UnLock();
        timerStarted = false;
        if (action != null) { action.Invoke(); }
        for (int i = 0; i < childs.Length; ++i)
        {
            childs[i].Unlock();
        }
    }
    public virtual void AskConfirmation()
    {
        ConfirmationPanel panel = _tree.confirmationPanel;
        
        panel.BindAction(GetConfirmation);
        if (timerStarted)
        { panel.Display(eConfirmationType.cancel); }
        else
        {
            if (!_tree.isLock && (!activated || unlimitedUsage)&& ResourceManager.instance.CanPlace(cost))
            { panel.Display(eConfirmationType.validate); }
            else
            { panel.Display(eConfirmationType.none); }
        }
    }
    public virtual void GetConfirmation(bool answer)
    {
        if (answer)
        {
            StartTimer();
        }
        else
        {
            Cancel();
        }
        if (!_tree.confirmationPanel.hideOnValidate)
        {
            AskConfirmation(); //update the display of the panel button
        }
    }
    public virtual void StopTimerRoutine()
    {
        if (_routine != null)
        { StopCoroutine(_routine); }
        _routine = null;
    }
    public virtual void Cancel()
    {
        StopTimerRoutine();
        _tree.UnLock();
        timerStarted = false;
        ResourceManager.instance.AddRessource(cost);
    }
}
