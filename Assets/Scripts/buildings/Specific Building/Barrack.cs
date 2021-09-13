using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Barrack : SA.Building
{
    [Header("Barrack Settings")]
	public	UnitData	infantryData;
	public	UnitData	archerData;
	[SerializeField]
	private	GameObject	_spawn;
    public bool debug_UnlimitedUnit = true;
    public static float trainingTime = 5f;
    delegate void VoidFun();
    private static event VoidFun _onArcherUnlock;
    //private static event VoidFun _onUnitUpgrade;
    private static bool _archerUnlocked = false;
    private int _QueueCapacity = 5;
    private Coroutine _spawningRoutine = null;
    public float spawStatus = 0.0f;
    public bool isSpawning { get; private set; }
    public Queue<eUnit> spawnQueue { get; private set; }
    public eUnit currentRecruit { get; private set; }
    public float recruitMentProgression { get; private set; }
    

    protected override void Start()
    {
        base.Start();
        if (_archerUnlocked) { UnlockArcher(); }
        spawnQueue = new Queue<eUnit>(_QueueCapacity);
        isSpawning = false;
    }
    override protected void OnEnable()
    {
		base.OnEnable();
        _onArcherUnlock += UnlockArcher;
        AddAction(SpawnInfantry, infantryData.sprite, true, "Infantry", "Melee unit", infantryData.cost);
    }
    protected virtual void OnDisable()
    {
        _onArcherUnlock -= UnlockArcher;
        //CancelSpawn();
    }
    private void OnDestroy()
    {
        if (spawnQueue == null)
        {
            return;
        }
        spawnQueue.Clear();
        CancelSpawn();
    }
    #region staticFun
    public static void EnableArcher()
    {
        _archerUnlocked = true;
        if (_onArcherUnlock != null)
            _onArcherUnlock.Invoke();
    }
    public static void EnableFastRecruitment()
    {
        trainingTime *= 0.8f;
    }
    #endregion

    public void UnlockArcher()
    {
        AddAction(SpawnArcher, archerData.sprite, true, "Archer", "Range unit", archerData.cost);
    }
    private void AddToQueue(eUnit unit)
    {
        spawnQueue.Enqueue(unit);
        SpawnNext();
    }
    public void CancelSpawn()
    {
        if (_spawningRoutine != null)
        {
            StopCoroutine(_spawningRoutine);
            ResourceManager.instance.Burry(eJob.Soldier, true);
            _spawningRoutine = null;
            //UiManager.instance.recruitmenDisplays.DisplayTrainingUnit(eUnit.none);
            SpawnNext();
        }
    }
    private void SpawnNext()
    {
        if (_spawningRoutine == null && spawnQueue.Count > 0)
        {
            eUnit unit = spawnQueue.Dequeue();
            //UiManager.instance.recruitmenDisplays.DisplayTrainingUnit(unit);
            _spawningRoutine = StartCoroutine(Spawning(unit));
        }
        else if (_spawningRoutine == null)
        {
            isSpawning = false;
            UiManager.instance.RefreshUi();
        }
        //UiManager.instance.recruitmenDisplays.DisplayQueue(spawnQueue);
    }
    private IEnumerator Spawning(eUnit unit)
    {
        float timePassed = 0.0f;
        isSpawning = true;
        currentRecruit = unit;
        while ( timePassed < trainingTime)
        {
            yield return new WaitForEndOfFrame();
            timePassed += GameTimer.timeScale * Time.deltaTime;
            recruitMentProgression = timePassed / trainingTime;
        }
        InstanciateUnit(unit);
        _spawningRoutine = null;
        currentRecruit = eUnit.none;
        //UiManager.instance.recruitmenDisplays.DisplayTrainingUnit(eUnit.none);
        SpawnNext();
    }
    /// <summary>
    /// INstanciate the desired Unit to the world
    /// </summary>
    /// <param name="unit"></param>
    private void InstanciateUnit(eUnit unit)
    {
        UnitData data = (unit == eUnit.infantry) ? infantryData : archerData;
        GameObject go = Instantiate(data.prefab, _spawn.transform.position, Quaternion.identity);
        go.GetComponent<Unit>()?.ApplyStats(data);
    }
    public UnitData GetUnitData(eUnit unit)
    {
        if (unit == eUnit.archer) { return archerData; }
        if (unit == eUnit.infantry) { return infantryData; }
        return null;
    }
	public	void	SpawnInfantry()
	{
        TrySpawnUnit(eUnit.infantry);
	}
    public bool TrySpawnUnit(eUnit unit)
    {
        if (!debug_UnlimitedUnit && spawnQueue.Count >= _QueueCapacity)
            return false;
        UnitData unitData = GetUnitData(unit);
        if (ResourceManager.instance.freeSoldiers >= 1 && ResourceManager.instance.CanPlace(unitData.cost))
        {
            if (ResourceManager.instance.HirePeople(eJob.Soldier) && ResourceManager.instance.ConsumeResource(unitData.cost))
            {
                AddToQueue(unit);
                return true;
            }
        }
        return false;
    }
    public void SpawnArcher()
    {
        TrySpawnUnit(eUnit.archer);
    }
}
