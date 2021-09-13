using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Wave", menuName = "Data/WaveData")]
public class WaveData : ScriptableObject
{
    [SerializeField] private Wave[] _wave;
    [SerializeField] private eDifficulty _difficulty;

    public eDifficulty difficulty
    {
        get { return this._difficulty; }
    }

    public Wave[] wave
    {
        get { return this._wave; }
    }
}
