using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHall : SA.Building
{
    [Header("Main Hall Settings")]
    [SerializeField] private UnitData _scientist;
    [SerializeField] private UnitData _worker;
    [SerializeField] private UnitData _soldier;
    [SerializeField] private Sprite _scoutSprite;

    public GameObject endGameText;

    protected override void OnEnable()
    {
        base.OnEnable();
        AddAction(SpecializeScientist, _scientist.sprite, true, "Specialize Scientist", "Transform a villager into a scientist");
        AddAction(SpecializeSoldier, _soldier.sprite, true, "Specialize Soldier", "Transform a villager into a soldier");
        AddAction(SpecializeWorker, _worker.sprite, true, "Specialize Worker", "Transform a villager into a worker");
        AddAction(CreateExplorationTeam, _scoutSprite, true, "Create an exploration team", "Create an exploration team");
    }

    public void SpecializeWorker()
    {
        ResourceManager.instance.SpecializePeople(eJob.Worker);
    }

    public void SpecializeScientist()
    {
        ResourceManager.instance.SpecializePeople(eJob.Scientist);
    }

    public void SpecializeSoldier()
    {
        ResourceManager.instance.SpecializePeople(eJob.Soldier);
    }

    public void CreateExplorationTeam()
    {
        if (ResourceManager.instance.freeSoldiers > 2 && ResourceManager.instance.freeScientists > 2)
        {
            ResourceManager.instance.HirePeople(eJob.Scientist, 3);
            ResourceManager.instance.HirePeople(eJob.Soldier, 3);
            ExploManager.instance.CreateTeam(3, 3);
        }
    }

    public void DisolveExplorationTeam(ExploTeam team)
    {

    }

    public override void Destroy()
    {
        base.Destroy();
        GameManager.instance.Pause();
        endGameText.gameObject.SetActive(true);
        var component = Camera.main.GetComponent<CommonSounds>();
        if (component == null)
        {
            return;
        }
        AudioClip gameOver = component.gameOver;
        if (null != gameOver)
        {
            CommonSounds.inst.audioSource.PlayOneShot(gameOver, 2F);
        }
    }
}
