using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public enum eUnit
{
    none = 0,
    infantry,
    archer
}

public class Unit : Character
{
    [Header("Unit Settings")]
    [SerializeField] protected Orc _target;
    [SerializeField] protected float _range = 10.0f;
    public float range { get { return _range; } protected set { _range = value; } }
    protected Coroutine chase_coroutine;
    [SerializeField] protected bool _isAttacking = false;
    public float offsetCharacter = 3.5f;
    protected Rigidbody _rigidbody;
    public Sprite spriteStop;
    public Sprite spriteIconAttack;
    public Sprite spriteIconBehaviorAtt;
    public Sprite spriteIconBehaviorDef;
    public eUnit unitType { get; protected set; }
    public Vector3 mainTarget = Vector3.zero;
    public bool ignoreEnemy;
    private bool _ignoreEnemy;
    public Outline outlineComponent;
    public SphereCollider fightTrigger;

    protected  static List<Unit> _allUnits = new List<Unit>();

    #region UnityMethods

    protected override void Start()
    {
        base.Start();
        _rigidbody = GetComponent<Rigidbody>();
        AddAction(StopRunning, spriteStop, true, "Stop", "Stop the current unit");
        AddAction(BehaviorAtt, spriteIconBehaviorAtt, true, "Behavior attack", "The unit will attack all ennemies");
        AddAction(BehaviorDef, spriteIconBehaviorDef, true, "Behavior defense", "The unit ignore ennemies until it arrives");
        _allUnits.Add(this);
    }
    public void ApplyStats(UnitData data)
    {
        damage = data.damage;
        _hpMax = data.maxHp;
    }

    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(GoodSpawn_coroutine());
    }

    private void LateUpdate()
    {

    }

    protected override void Update()
    {
        base.Update();

        if (!_isAttacking && _target != null && Vector3.Distance(transform.position, _target.transform.position) <= _range + offsetCharacter)
            Attack(true);

        if (_target != null && !_target.isDead && Vector3.Distance(transform.position, _target.transform.position) > _range + offsetCharacter)
        {
            Attack(false);
            Chase();
        }

        if (_target != null && _target.isDead)
        {
            StopRunning();
            Attack(false);
        }
    }

    public void OnTriggerStay_special(Collider other)
    {
        if (other.transform.gameObject.CompareTag(Tags.Enemy) && !other.transform.parent.GetComponent<Character>().isDead && ((_target != null && _target.isDead) || _target == null) && !_ignoreEnemy)
        {
            FocusTarget(other.transform.parent.gameObject.GetComponent<Character>());
        }
    }
    protected virtual void OnDestroy()
    {
        _allUnits.Remove(this);
    }

    #endregion

    #region PublicMethods

    public override void StopRunning()
    {
        base.StopRunning();
        _ignoreEnemy = false;
    }
    public void UpgradeHpMax(uint amount)
    {
        _hpMax += (int)amount;
        _hp += (int)amount;
    }
    public void FocusTarget(Character target)
    {
        _target = target.GetComponent<Orc>();
        Chase();
    }

    public void RemoveTarget()
    {
        _isAttacking = false;
        _animator.SetBool("Attack", false);
        _target = null;
        StopChase();
    }

    public override void Die()
    {
        base.Die();
        RemoveTarget();
        Destroy(fightTrigger);
        GameManager.instance.selection.ChangeUnitList(gameObject, false);
    }

    public void IgnoreEnemy(bool state)
    {
        ignoreEnemy = state;
        if (_isRunning)
            _ignoreEnemy = state;
    }

    public override void Run(Vector3 destination)
    {
        base.Run(destination);
        _ignoreEnemy = ignoreEnemy;
    }

    public void BehaviorAtt()
    {
        IgnoreEnemy(false);
        UiManager.instance.RefreshUi();
    }

    public void BehaviorDef()
    {
        IgnoreEnemy(true);
        UiManager.instance.RefreshUi();
    }

    #endregion

    #region PrivateMethods

    #endregion

    #region ProtectedMethods

    protected IEnumerator Chase_coroutine()
    {
        while (_target && !_target.isDead && _isRunning)
        {
            if (Vector3.Distance(transform.position, _target.transform.position) > _range + offsetCharacter)
                SetDestination(_target.transform.position);
            else
                StopRunning();
            yield return new WaitForFixedUpdate();
        }
    }

    protected virtual void Attack(bool state)
    {
        if (state)
        {
            _rigidbody.constraints = RigidbodyConstraints.FreezePosition;
            _isAttacking = true;
            _animator.SetBool("Attack", true);
        }
        else
        {
            _rigidbody.constraints = RigidbodyConstraints.None;
            _isAttacking = false;
            _animator.SetBool("Attack", false);
            if (_target != null && _target.isDead)
                _target = null;
            if (_target == null && mainTarget != Vector3.zero && Vector3.Distance(transform.position, mainTarget) > _agent.stoppingDistance)
                Run(mainTarget);
        }

    }

    protected virtual void StopChase()
    {
        if (chase_coroutine != null)
            StopCoroutine(chase_coroutine);
    }

    protected virtual void Chase()
    {
        Run(_target.transform.position);
        StopChase();
        chase_coroutine = StartCoroutine(Chase_coroutine());
    }

    protected IEnumerator GoodSpawn_coroutine()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<Rigidbody>().isKinematic = true;
    }

    #endregion
    #region staticMethods
    public static void UpgradeDamageUnit(eUnit type, int amount)
    {
        foreach (Unit unit in _allUnits)
        {
            if (unit.unitType == type)
            {
                unit.damage += amount;
            }
        }
    }
    public static void UpgradeLpUnit(eUnit type, uint amount)
    {
        foreach (Unit unit in _allUnits)
        {
            if (unit.unitType == type)
            {
                unit.UpgradeHpMax(amount);
            }
        }
    }
    #endregion
}