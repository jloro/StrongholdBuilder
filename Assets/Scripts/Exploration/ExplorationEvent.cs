using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationEvent : SkillSprite
{
    [SerializeField] private DataExplorationEvent _exploEvent;
    private delegate void ExploFun(ExploTeam team);

    private static Dictionary<eExploFunctions, ExploFun> _exploFunctions = null;
    public ExploTeam team;

    protected override void Start()
    {
        base.Start();
        if (_exploFunctions == null) { Initialize(); }
    }
    public DataExplorationEvent GetExploEvent()
    {
        return _exploEvent;
    }
    private static void Initialize()
    {
        _exploFunctions = new Dictionary<eExploFunctions, ExploFun>();
        _exploFunctions.Add(eExploFunctions.nothing, DoNothing);
        _exploFunctions.Add(eExploFunctions.gainResources, GainResources);
        _exploFunctions.Add(eExploFunctions.gainFood, GainFood);
        _exploFunctions.Add(eExploFunctions.gainWood, GainWood);
        _exploFunctions.Add(eExploFunctions.gainStone, GainStone);
        _exploFunctions.Add(eExploFunctions.findAlly, FindAlly);
        _exploFunctions.Add(eExploFunctions.healAll, HealAll);
        _exploFunctions.Add(eExploFunctions.healFew, HealOne);
        _exploFunctions.Add(eExploFunctions.hurt, Hurt);
        _exploFunctions.Add(eExploFunctions.kill, KillMember);
        _exploFunctions.Add(eExploFunctions.slow, Slow);
        _exploFunctions.Add(eExploFunctions.stealResources, StealResources);
        _exploFunctions.Add(eExploFunctions.hurtFew, HurtFew);
        _exploFunctions.Add(eExploFunctions.waveTip, WaveTip);
        _exploFunctions.Add(eExploFunctions.comingHome, ComingHome);
    }
    #region exploFunctions
    public static void DoNothing(ExploTeam team) { }
    public static void GainResources(ExploTeam team)
    {
        team.AddResources(
            new ResourceCost(Random.Range(0, 20) * 10,
            Random.Range(0, 20) * 10, Random.Range(0, 20) * 10)
            );    
    }
    public static void GainFood(ExploTeam team)
    {
        team.AddResources(new ResourceCost(food:Random.Range(0, 20) * 10));
    }
    public static void GainWood(ExploTeam team)
    {
        team.AddResources(new ResourceCost(wood: Random.Range(0, 20) * 10));
    }
    public static void GainStone(ExploTeam team)
    {
        team.AddResources(new ResourceCost(stone: Random.Range(0, 20) * 10));
    }
    public static void HealAll(ExploTeam team)
    {
        team.HealAll();
    }
    public static void HealOne(ExploTeam team)
    {
        team.HealOne();
    }
    public static void FindAlly(ExploTeam team)
    {
        Scout ally = new Scout((eScoutType)Random.Range(0, 2));
        team.AddMember(ally);
    }
    public static void WaveTip(ExploTeam team)
    {
        WaveUI.instance.UnlockTip();
    }
    public static void HurtFew(ExploTeam team)
    {
        team.TakeDmg(0, 1);
    }

    public static void ComingHome(ExploTeam team)
    {
        ResourceManager.instance.AddRessource(team.loot);
        var lst = team.team.FindAll(x => x.scoutType == eScoutType.scientist);
        ResourceManager.instance.FirePeople(eJob.Scientist, lst.Count);
        lst = team.team.FindAll(x => x.scoutType == eScoutType.soldier);
        ResourceManager.instance.FirePeople(eJob.Soldier, lst.Count);
        ExploManager.instance.RemoveTeam(team);
    }
    public static void Slow(ExploTeam team)
    {
        Debug.Log("Sllow Not implemented for now");
    }
    public static void Hurt(ExploTeam team)
    {
        team.TakeDmg(0, 2);
    }
    public static void StealResources(ExploTeam team)
    {
        team.StealResources(new ResourceCost(Random.Range(5, 13) * 10, //wood
            Random.Range(5, 13) * 10, //food
            Random.Range(5, 13) * 10) //stone
            );
    }
    public static void KillMember(ExploTeam team)
    {
        team.KillAMember();
    }

    #endregion
    public void Success(ExploTeam team)
    {
        _exploFunctions[_exploEvent.onSuccess](team);
        if (!unlimitedUsage)
            _render.color = ((ExplorationTree)_tree).succesColor;
    }
    public override void AskConfirmation()
    {
        ExploManager.instance.SelectDestination(this);
        team = ExploManager.instance.selectedTeam;
        if (team == null) { return; }
        base.AskConfirmation();
    }

    protected override IEnumerator TimerCoroutine()
    {
        if (team.currentEvent != null)
        { team.currentEvent.Cancel(); }
        ExploManager.instance.GoToDestination(team, this);
        team.currentEvent = this;
        Vector3 dir =  (transform.position - team.icon.transform.position).normalized;
        Transform teamTransform = team.icon.transform;
        while (Vector3.Distance(teamTransform.position, transform.position) > 0.2f)
        {
            teamTransform.Translate(dir * _tree.currentSpeed * Time.deltaTime * GameTimer.timeScale);
            yield return null;
        }
        team.currentEvent = null;
        yield return base.TimerCoroutine();
    }

    protected override void Activate()
    {
        ExploManager.instance.DisplayEvent(this);
        base.Activate();
    }

    public void TryEvent(out bool success)
    {
        success = false;
        if (!activated) { return; }
        
        if (_exploEvent.eventType == eExploEventType.combat)
        {
            success = DoEvent(team.Fight() > _exploEvent.score);
        }
        else
        {
            success = DoEvent(team.Search() > _exploEvent.score);
        }
        ExploManager.instance.UpdateDisplay();//Update the team description
    }
    private bool DoEvent(bool success)
    {
        if (success) { Success(team); }
        else { Fail(team); }
        return success;
    }
    
    public void Fail(ExploTeam team)
    {
        _exploFunctions[_exploEvent.onFailure](team);
        _render.color = ((ExplorationTree)_tree).failColor;
    }


}
