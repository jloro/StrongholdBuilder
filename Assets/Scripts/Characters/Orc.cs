using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;
using UnityEngine.AI;

public class Orc : Character
{
	[Header("Orc Settings")]
    [Tooltip("Actual taget.")]
    [SerializeField] private Targetable _target;
	[SerializeField] private	Targetable	_mainTarget;
    [SerializeField] private	bool		_follow;
    [SerializeField] private bool _start;
    public float _range;
    public GameObject arrowTarget { get { return _arrowTarget; } }
    [SerializeField] private GameObject _arrowTarget;
    private bool _focusMt = true;
    [SerializeField] private int _wallDestroyed;
    [Tooltip("Number of wall to destroy in range before ignoring walls.")]
    public int maxWall;
    [SerializeField] private LayerMask _layerRaycast;

    public AudioClip orcSound;

    private float targetTime = 2.0f ;

    protected override void Start()
    {
        base.Start();
        _agent.speed = 0;
        _agent.isStopped = false;
    }

    public void StartWave()
    {
        _start = true;
        _agent.speed = 40;
    }
    public void Wait()
    {
        _start = false;
        _agent.isStopped = true;
    }

    private void Laugh() {
        if (50 == Random.Range(0, 10000)) {
                if (null != orcSound) {
                    AudioSource.PlayClipAtPoint(orcSound, Camera.main.transform.position);
            }
        }
    }

    protected override void Update()
    {
        if (!_start || isDead) { return; }

        base.Update();
        
        Laugh();

        if (((_target != null && _target.isDead) || _target == null) && !_focusMt)
		{
			_follow = true;
            if (_animator.speed != 0)
			    FocusMainTarget();
			_animator.SetBool("Attack", false);
			_animator.SetBool("Run", true);
		}
			
		if (_target != null && _agent.remainingDistance < _agent.stoppingDistance + _range)
		{
			_follow = false;
			_animator.SetBool("Run", false);
			_animator.SetBool("Attack", true);
            AudioClip punch = CommonSounds.inst.punch;
			if (null != punch) {
                targetTime += Time.deltaTime;
 
                if (targetTime >= 2.0f) {
                    AudioSource.PlayClipAtPoint(punch, Camera.main.transform.position);
                    targetTime = 0.0f;
                }
            }
		}
    }

    private void LateUpdate()
    {
        if (!_start || isDead) { return; }

		if (_target != null && _follow && _target.targetType == eType.character)
		{
            _agent.SetDestination(_target.transform.position);
		}
    }
    public void SetMainTarget(Targetable tg)
    {
        _mainTarget = tg;
        if (_target == null)
        {
            Run(tg.transform.position);
			FocusMainTarget();
        }
    }
	public	void	Attack_animEv()
	{

		if (_target != null && _target.TakeDamage(damage) )
		{
			_target = null;
			_animator.SetBool("Attack", false);
			_animator.SetBool("Run", true);
            FocusMainTarget();
        }
	}
    private void OnTriggerStay(Collider other)
    {
        if ((_target == null || (_target != null && _target.isDead)) && (other.CompareTag(Tags.Player) || other.CompareTag(Tags.Wall)))
        {
            if (other.GetComponent<Targetable>().targetType == eType.building && (other.GetComponent<Building>().type == Building.BuildingType.Wall || other.GetComponent<Building>().type == Building.BuildingType.WallPole) && _wallDestroyed >= maxWall)
                return;
            ChangeTarget(other.GetComponent<Targetable>());
        }
    }
    private	void	OnTriggerEnter(Collider other)
	{
        
        GameObject go = other.gameObject;
		if (_target == null && (go.CompareTag(Tags.Player) || go.CompareTag(Tags.Wall) ))
		{
            if (other.GetComponent<Targetable>().targetType == eType.building && (other.GetComponent<Building>().type == Building.BuildingType.Wall || other.GetComponent<Building>().type == Building.BuildingType.WallPole) && _wallDestroyed >= maxWall)
                return;
            ChangeTarget(other);
		}

        if (other.gameObject.CompareTag(Tags.WallDestroyEvent) && _wallDestroyed < maxWall)
            _wallDestroyed++;
    }

    protected void FocusMainTarget()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position + new Vector3(0.0f, 2.0f, 0.0f), _mainTarget.transform.position - transform.position, out hit, Mathf.Infinity, _layerRaycast);
        Debug.DrawRay(transform.position + new Vector3(0.0f, 2.0f, 0.0f), (_mainTarget.transform.position - transform.position) * 1000, Color.red, 10000);
        if (hit.transform.gameObject == _mainTarget.gameObject)
        {
            _focusMt = true;
            _follow = true;
            Run(_mainTarget.GetComponent<Collider>().ClosestPoint(transform.position));
            _wallDestroyed = maxWall;
        }
        else
        {
            _follow = true;
            Run(hit.transform.GetComponent<Collider>().ClosestPoint(transform.position));
            _wallDestroyed = 0;
        }
    }
    protected void ChangeTarget(Targetable tg)
    {
        _focusMt = false;
        if (tg == null)
            return;
        _follow = true;
        _target = tg;
        if (tg.targetType == eType.building)
        {
            Vector3 dst = tg.GetComponent<Collider>().ClosestPoint(transform.position);
            Run(dst);
        }
        else
        { Run(_target.transform.position); }
    }
    protected void ChangeTarget(Collider tg)
    {
        _focusMt = false;
        _follow = true;
        _target = tg.GetComponent<Targetable>();
        if (_target.targetType == eType.building)
        {
            Vector3 dst = tg.ClosestPoint(transform.position);
            Run(dst);
        }
        else
        { Run(_target.transform.position); }
    }
}
