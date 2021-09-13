using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestResourceManager : MonoBehaviour
{
    public Text pop;
    public Text workers;
    public Text scientist;
    public Text soldier;
    public Text unAssigned;
    private void Update()
    {
        pop.text = ResourceManager.instance.population.ToString() +" / " + ResourceManager.instance.populationMax.ToString(); 
        workers.text = ResourceManager.instance.freeWorkers.ToString() +" / " + ResourceManager.instance.totalWorkers.ToString();
        scientist.text = ResourceManager.instance.freeScientists.ToString() +" / " + ResourceManager.instance.totalScientists.ToString();
        soldier.text = ResourceManager.instance.freeSoldiers.ToString() +" / " + ResourceManager.instance.totalSoldiers.ToString(); 
        //unAssigned.text = ResourceManager.instance.
    }
    public void AddPopulation(int amount)
    {
        ResourceManager.instance.IncreaseMaxPop((uint)amount);
    }
    public void SpecializeWorker()
    {
        if (!ResourceManager.instance.SpecializePeople(eJob.Worker))
        { Debug.Log("cannot specialize another worker"); }
    }
    public void SpecializeScientist()
    {
        if (!ResourceManager.instance.SpecializePeople(eJob.Scientist))
        { Debug.Log("cannot specialize another Scientist"); }
    }
    public void SpecializeSoldier()
    {
        if (!ResourceManager.instance.SpecializePeople(eJob.Soldier))
        { Debug.Log("cannot specialize another Soldier"); }
    }
    public void HireSoldier()
    {
        if (!ResourceManager.instance.HirePeople(eJob.Soldier))
        { Debug.Log("cannot Hire another Soldier"); }
    }
    public void HireScientist()
    {
        if (!ResourceManager.instance.HirePeople(eJob.Scientist))
        { Debug.Log("cannot Hire another Scientist"); }
    }
    public void HireWorker()
    {
        if (!ResourceManager.instance.HirePeople(eJob.Worker))
        { Debug.Log("cannot Hire another Worker"); }
    }
    public void KillWorker(bool isHired)
    {
        ResourceManager.instance.Burry(eJob.Worker, isHired);
    }
    public void KillScientist(bool isHired)
    {
        ResourceManager.instance.Burry(eJob.Scientist, isHired);
    }
    public void KillSoldier(bool isHired)
    {
        ResourceManager.instance.Burry(eJob.Soldier, isHired);
    }
}
