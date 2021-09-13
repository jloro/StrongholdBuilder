using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Sync/SyncVector")]
public class SyncVector : ScriptableObject
{
	[SerializeField]private Vector3 baseValue = Vector3.zero;

	public Vector3 runValue ;

	private void OnEnable()
	{
		Reset();
	}
	public void Reset()
	{
		runValue = baseValue;
	}
}
