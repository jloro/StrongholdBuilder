using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Sync/SyncFloat")]
public class SyncFloat : ScriptableObject {

	[SerializeField] private float baseValue = 0;

	public float runValue = 0;

	private void OnEnable()
	{
		Reset();
	}
	public void Reset()
	{
		runValue = baseValue;
	}
}
