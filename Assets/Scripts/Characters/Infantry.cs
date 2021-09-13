using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infantry : Unit
{
    protected override void Start()
    {
        base.Start();
        unitType = eUnit.infantry;
    }

    protected override void Update()
    {
        base.Update();
    }

	public	void	Attack_animEv()
	{
		if (_target != null && !_target.isDead && _target.TakeDamage(damage))
		{
			_target = null;
			StopRunning();
            Attack(false);
		}
	}
}
