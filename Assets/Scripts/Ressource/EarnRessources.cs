using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarnRessources : MonoBehaviour
{
	[Header("Ressources parameters for this building.")]
	[Tooltip("Ressource type (Food, Stone or Wood).")]
	public	eResources	ressourceType;
	[SerializeField]
	[Tooltip("Frequency to earn ressource.")]
	private	float				_frequency;
	public	float				frequency { get { return _frequency; }
											set {
												_frequency = value;
												CancelInvoke();
												InvokeRepeating("AddRessource_invoke", 0.0f, _frequency);
											}

	}
	[Tooltip("Number of ressources this building has earned.")]
	[SerializeField] private	int					_nbRessource = 0;
	public	int					nbRessource { get { return _nbRessource; } }
	[Tooltip("Number of workers this building has.")]
	[SerializeField]private	int					_nbWorkers;
	public	int									nbWorkers { get { return _nbWorkers; } }
	[Tooltip("Number of workers max this building can have.")]
	//[SerializeField]private	int					_maxWorkers;
    public Sprite iconRessource;

	private static Dictionary<eResources, int> _workerEfficiency = new Dictionary<eResources, int> {
		[eResources.Wood] = 1,
		[eResources.Food] = 1,
		[eResources.Stone] = 1
	};
	private static Dictionary<eResources, int> _maxWorkers = new Dictionary<eResources, int>
	{
		[eResources.Wood] = 10,
		[eResources.Food] = 10,
		[eResources.Stone] = 10
	};

	static public void IncreaseEfficiency(eResources resource)
	{
		_workerEfficiency[resource] += 1;
	}
	static public void IncreaseCapacity(eResources resource, int increaseBy)
	{
		_maxWorkers[resource] += increaseBy;
	}
	// Start is called before the first frame update
	private void Start()
    {
        InvokeRepeating("AddRessource_invoke", 0.0f, _frequency);
        GetComponent<SA.Building>().AddAction(AddWorker, ResourceManager.instance.spriteAddWorker, true, "Add worker", "Add a worker to this building");
        GetComponent<SA.Building>().AddAction(RemoveWorker, ResourceManager.instance.spriteRemoveWorker, true, "Remove worker", "Remove a worker from this building");
	}

	private	void	AddRessource_invoke()
	{
        if (GameManager.instance.freezeStatus)
            return;
		ResourceManager.instance.AddRessource(ressourceType, _nbWorkers * _workerEfficiency[ressourceType]);
		_nbRessource += _nbWorkers * _workerEfficiency[ressourceType];
	}

	//Add 1 worker and return if buidling has already maximum worker
	public	void	AddWorker()
	{
		if (_nbWorkers < _maxWorkers[ressourceType] && ResourceManager.instance.HirePeople(eJob.Worker))
		{
			_nbWorkers++;
			//return true;
		}
		UpdateBuildingDescription();
		//return false;
	}

	public void RemoveWorker()
    {
		if (_nbWorkers > 0)
		{
			ResourceManager.instance.FirePeople(eJob.Worker);
			_nbWorkers--;
		}
		UpdateBuildingDescription();
	}

	public void AddMaxWorker(int add)
    {
        //_maxWorkers += add;
    }

	private void UpdateBuildingDescription()
	{
		SA.Building bd = GetComponent<SA.Building>();
		switch (bd.type)
		{
			case SA.Building.BuildingType.Farm:
				bd.description = $"Earn food\nWorkers: {nbWorkers} / {_maxWorkers[ressourceType]}";
				break;
			case SA.Building.BuildingType.Mine:
				bd.description = $"Earn stone\nWorkers: {nbWorkers} / {_maxWorkers[ressourceType]}";
				break;
			case SA.Building.BuildingType.Sawmill:
				bd.description = $"Earn wood\nWorkers: {nbWorkers} / {_maxWorkers[ressourceType]}";
				break;
		}
		UiManager.instance.RefreshUi();
	}
}
