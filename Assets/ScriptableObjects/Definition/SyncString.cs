using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Sync/SyncString")]
public class SyncString : ScriptableObject {

	[SerializeField] private string baseValue = null;

	public string runValue = null;

	private void OnEnable()
	{
		Reset();
	}
	public void Reset()
	{
		runValue = baseValue;
	}
}
