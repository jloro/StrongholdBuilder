using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum eJob { Worker, Soldier, Scientist, None }
public enum eResources { Wood, Food, Stone }

public class ResourceManager : MonoBehaviour
{
    public ResourceCost beginRessources;
	public int wood { get; protected set; }
	public	int	food { get; protected set; }
    public	int	stone { get; protected set; }
    public	int	populationMax { get; protected set; }
    public	int	population { get; protected set; }
    public	int	totalWorkers { get { return _specializedPpl[eJob.Worker]; } }
    public	int totalSoldiers { get { return _specializedPpl[eJob.Soldier]; } }
    public	int totalScientists { get { return _specializedPpl[eJob.Scientist]; } }
    public int freeWorkers { get { return _freePpl[eJob.Worker]; } }
    public int freeSoldiers { get { return _freePpl[eJob.Soldier]; } }
    public	int freeScientists { get { return _freePpl[eJob.Scientist]; } }
    private Dictionary<eJob, int> _specializedPpl; //Ppl stand for people
    private Dictionary<eJob, int> _freePpl;
    static public	ResourceManager	instance;
    public Sprite spriteAddWorker;
    public Sprite spriteRemoveWorker;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        _specializedPpl = new Dictionary<eJob, int>();
        _freePpl = new Dictionary<eJob, int>();

        int beginWorker = 30;
        int beginSoldier = 0;
        int beginScientist = 0;
        population = beginWorker + beginScientist + beginSoldier;
        populationMax = population + 5;
        _specializedPpl.Add(eJob.Worker, beginWorker);
        _specializedPpl.Add(eJob.Soldier, beginSoldier);
        _specializedPpl.Add(eJob.Scientist, beginScientist);
        _freePpl.Add(eJob.Worker, beginWorker);
        _freePpl.Add(eJob.Soldier, beginSoldier);
        _freePpl.Add(eJob.Scientist, beginScientist);
        wood = beginRessources.wood;
        food = beginRessources.food;
        stone = beginRessources.stone;
    }
    public bool ConsumeResource(int woodQtt, int foodQtt = 0, int stoneQtt =0 )
    {
        if (woodQtt <= wood && foodQtt <= food && stoneQtt <= stone)
        {
            wood -= woodQtt;
            food -= foodQtt;
            stone -= stoneQtt;
            return true;
        }
        return false;
    }
    /// <summary>
    /// if all ressources are availlable consume them all and return true. else consume nothing
    /// </summary>
    /// <param name="cost"></param>
    /// <returns></returns>
    public bool ConsumeResource(ResourceCost cost)
    {
        if (!CanPlace(cost))
            return false;
        wood -= cost.wood;
        stone -= cost.stone;
        food -= cost.food;
        return true;
    } 
    public bool ConsumeResource(eResources ressource, uint qtt)
    {

        if (ressource == eResources.Wood && (int)qtt <= wood)
        {
            wood -= (int)qtt;
            return true;
        }
        if (ressource == eResources.Food && (int)qtt <= food)
        {
            food -= (int)qtt;
            return true;
        }
        if (ressource == eResources.Stone && (int)qtt <= stone)
        {
            stone -= (int)qtt;
            return true;
        }
        return false;
    }
    public void AddRessource(ResourceCost cost)
    {
        wood += cost.wood;
        stone += cost.stone;
        food += cost.food;
    }
    public void	AddRessource(eResources type, int nb)
	{
		if (type == eResources.Wood)
			wood += nb;
		else if (type == eResources.Food)
			food += nb;
		else if (type == eResources.Stone)
			stone += nb;
	}
    public void IncreaseMaxPop(uint amount)
    {
        populationMax += (int)amount;
    }
    public void DecreaseMaxPop(uint amount)
    {
        populationMax -= (int)amount;
    }
    public bool HirePeople(eJob assignment, int qtt)
    {
        if (qtt < 1 || assignment == eJob.None || _freePpl[assignment] < qtt)
            return false;
        _freePpl[assignment] -= qtt;
        return true;
    }
    public bool HirePeople(eJob assignment) 
    {
        return HirePeople(assignment, 1);
    }
    public bool FirePeople(eJob assignment, int qtt)
    {
        if (qtt < 1 || assignment == eJob.None)
            return false;
        _freePpl[assignment] += qtt;
        return true;
    }

    public bool FirePeople(eJob assignment)
    {
        return FirePeople(assignment, 1);
    }

    public bool SpecializePeople(eJob assignment)
    {
        if (assignment == eJob.None) { return false; }
        if (population < populationMax)
        {
            ++population;
            _specializedPpl[assignment] += 1;
            _freePpl[assignment] += 1;
            return true;
        }
        return false;
    }
    public void Burry(eJob assignment, bool isEmployed = false, uint qtt = 1) 
    {

        _specializedPpl[assignment] -= (int)qtt;
        if (_specializedPpl[assignment] < 0) 
        { 
            _specializedPpl[assignment] = 0;
            return;
        }
        if (!isEmployed)
        {
            _freePpl[assignment] -= (int)qtt;
            if (_freePpl[assignment] < 0) { _freePpl[assignment] = 0; }
        }
        --population;
    }

    public	bool	CanPlace(int food, int wood, int stone)
	{
		if (food <= this.food && stone <= this.stone && wood <= this.wood)
			return true;
		return false;
	}
    public bool CanPlace(ResourceCost cost)
    {
        if (cost.food <= this.food && cost.stone <= this.stone && cost.wood <= this.wood)
            return true;
        return false;
    }
}