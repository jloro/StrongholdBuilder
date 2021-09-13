using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Unit
{
    [Header("Archer Settings")]
    [Tooltip("Placeholder to instantiate arrow.")]
	[SerializeField]private	GameObject	arrowPlaceHolder;
	[SerializeField]private	GameObject	arrowPrefab;

    protected override void Start()
    {
        base.Start();
        unitType = eUnit.archer;
    }

    protected override void Update()
    {
        base.Update();
    }

	public	void	Shoot_animEv()
	{
        if (_target != null && !_target.isDead)
        {
            Arrow arrow = Instantiate(arrowPrefab, arrowPlaceHolder.transform).GetComponent<Arrow>();
            arrow.SetUp(_target, this, damage);
        }
	}
}
