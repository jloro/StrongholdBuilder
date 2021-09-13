using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eDirection
{
    north,
    west,
    est,
    south
}

[System.Serializable]
public struct Wave
{
    public int nbOfOrcs;
    [Tooltip("in minutes, frome the begenning of the mission")]
    public float time;
    [Tooltip("in seconds")]
    public float estimatedSpawnTime;
    public eDirection direction;
}
