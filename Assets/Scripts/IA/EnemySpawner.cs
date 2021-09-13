using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour 
{
	public static EnemySpawner instance { get; private set; }

	public GameObject[] srcs;
	public Area[] areas;
    //public float waveTimer;
    public Targetable objectif;
    public Wave[] waves;
    public int waveIndex { get; private set; }

    private Vector3 _offset;
    private Coroutine spawnCoroutine = null;
    private Coroutine spawnTimer = null;
    private Orc[] _orcs = null;
    private bool _waveStarted = false;
    [SerializeField]
    private float _beginTime;
	private Transform[] _spawnAreas = null;
    private Dictionary<eDirection, Area> areasDict;
    
    [SerializeField] private WaveData[] _wavesDifficulties;
    
    public delegate void waveDelegate();

    public event waveDelegate OnWaveArrived;


    private void Awake() 
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
	private void OnEnable() 
	{
	//	GameOverEvent.OnGameOver += OnGameOver;
	}
	private void OnDisable() 
	{
        //	GameOverEvent.OnGameOver -= OnGameOver;
        if (spawnCoroutine != null) { StopCoroutine(spawnCoroutine); }
        if (spawnTimer != null) { StopCoroutine(spawnTimer); }
    }
    private void Start()
    {        
        waveIndex = 0;
        _offset = Vector3.zero;
        _beginTime = GameTimer.time ;
        areasDict = new Dictionary<eDirection, Area>();
        foreach (var area in areas)
        {
            areasDict.Add(area.direction, area);
        }
        spawnTimer = StartCoroutine(WaveTimer_coroutine());
        foreach (var wavesDifficulty in _wavesDifficulties)
        {
	        if (wavesDifficulty.difficulty == DifficultyManager.inst.difficulty)
	        {
		        waves = wavesDifficulty.wave;
	        }
        }
    }

    public void OnGameOver()
	{
        if (spawnCoroutine != null) { StopCoroutine(spawnCoroutine); }
        if (spawnTimer != null) { StopCoroutine(spawnTimer); }
	}
    /// <summary>
    /// Spawn one orc and set his goal
    /// </summary>
    /// <param name="remainToSpawn"></param>
	public Orc Spawn(int remainToSpawn)
	{
		GameObject child;
		Orc enemy;
		int rd = Random.Range(0, srcs.Length);
		int rd2 = Random.Range(0, _spawnAreas.Length);
		child = Instantiate(srcs[rd], _spawnAreas[rd2].position + _offset, Quaternion.identity, null);
		enemy = child.GetComponent<Orc>();
        enemy.SetMainTarget(objectif);
        _orcs[remainToSpawn - 1] = enemy;
        if (!_waveStarted)
        { enemy.Wait(); }
        else 
        { enemy.StartWave(); }
        child.name = "orc " + remainToSpawn.ToString();
        return enemy;
		//child.transform.localPosition = Vector3.zero;
	}
    /// <summary>
    /// This coroutine Spawn the entire wave 
    /// </summary>
    /// <returns></returns>
	IEnumerator SpawnWave_coroutine(Wave wave)
	{
        _orcs = new Orc[wave.nbOfOrcs];
        int i = 0;
        Orc tmp = null;
        _spawnAreas = areasDict[wave.direction].GetSpawAreas();
		while (wave.nbOfOrcs > 0)
		{
            if (tmp == null)
			    tmp = Spawn(wave.nbOfOrcs);
            if (tmp && !tmp.GetComponent<NavMeshAgent>().pathPending)
            {
                tmp = null;
                wave.nbOfOrcs--;
                i++;
                _offset.x += 0.3f;
                if (i == 30)
                {
                    i = 0;
                    _offset.x = 0;
                    _offset.z += 0.3f;
                }
                if (i % 2 == 0)
                { yield return null; }
            }
            else
                yield return null;
        }
	}
    /// <summary>
    /// create a timer waiting for the next wave.
    /// call spawnCoroutine
    /// call StartTheWave
    /// </summary>
    /// <param name="wave"></param>
    /// <returns></returns>
    private IEnumerator WaveTimer_coroutine()
    {
	    yield return null;
        while (waveIndex < waves.Length)
        {
            Wave wave = waves[waveIndex];
            float actualTime = (GameTimer.time - _beginTime);

            float delay = (wave.time * 60) - (actualTime + wave.estimatedSpawnTime);

            yield return StartCoroutine(GameTimer.Wait_coroutine(delay));
            spawnCoroutine = StartCoroutine(SpawnWave_coroutine(wave));
            yield return StartCoroutine(GameTimer.Wait_coroutine(wave.estimatedSpawnTime));
            StartTheWave();
            ++waveIndex;
        }
    }
    /// <summary>
    /// All the spawned orcs will be running to their goal, 
    /// the remaining orcs to spawn, will be set to run instantanely after spawned
    /// </summary>
    public void StartTheWave()
    {
	    OnWaveArrived?.Invoke();
        _waveStarted = true;
        for (int i = _orcs.Length -1; i >= 0; --i)
        {
            if (_orcs[i] == null) { break; }//the spawn can not be finish yet
            _orcs[i].StartWave();
        }
        AudioClip waveMusic = CommonSounds.inst.wave;
			if (null != waveMusic) {
                
                AudioSource s = CommonSounds.inst.audioSource;
                s.clip = waveMusic;
                s.loop = true;
                s.Play();
                // AudioSource.PlayClipAtPoint(punch, Camera.main.transform.position);
            }
    }
}
