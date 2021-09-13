using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : Targetable
{
	[Header("Character Settings")] 
	public int		damage;
    [SerializeField] protected Animator _animator;
    [SerializeField] protected NavMeshAgent _agent;
	[SerializeField] protected	Vector3	_dst;

    public bool _isRunning { get; protected set; }

    #region UnityMethods

    private void OnEnable()
    {
        GameManager.instance.FreezeEvent += Freeze;
    }

    private void OnDisable()
    {
        GameManager.instance.FreezeEvent -= Freeze;
    }

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
    }

    protected override void Start()
    {
        base.Start();
        targetType = eType.character;
        if (GameManager.instance.freezeStatus)
            Freeze(true);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (_isRunning && Vector3.Distance(transform.position, _dst) < _agent.stoppingDistance)
        {
            StopRunning();
        }
    }


    #endregion

    #region PublicMethods

    public void Freeze(bool freeze)
    {
        if (freeze)
        {
            _agent.isStopped = true;
            _animator.speed = 0;
        }
        else
        {
            _agent.isStopped = false;
            _animator.speed = 1;
        }
    }

    virtual public void StopRunning()
    {
       // SetDestination(transform.position);
        _agent.isStopped = true;
        _isRunning = false;
        _animator.SetBool("Run", false);
    }

    virtual public void SetDestination(Vector3 pos)
    {
        _dst = pos;
        _agent.SetDestination(_dst);
    }

    virtual public void Run(Vector3 destination)
    {
        if (!GameManager.instance.freezeStatus)
            _agent.isStopped = false;
        else
            _agent.isStopped = true;
        SetDestination(destination);
        _isRunning = true;
        _animator.SetBool("Run", true);
    }

    public void Die_animEv()
    {
        Destroy(gameObject, 3.0f);
    }

    virtual public void Die()
    {
        isDead = true;
        _animator.SetTrigger("Die");
        StopRunning();
        Destroy(GetComponent<Collider>());
    }

    public override bool TakeDamage(int amount)
    {
        if (_hp == 0)
            return true;
        _hp -= amount;
        if (_hp <= 0)
        {
            _hp = 0;
            Die();
            return true;
        }
        return false;
    }

    #endregion

    #region PrivateMethods

    #endregion


}
