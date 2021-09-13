using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using TMPro;
using UnityEditor;

public class WaveUI : MonoBehaviour
{
    public static WaveUI instance;
    [SerializeField] private TextMeshProUGUI _textContains;
    [SerializeField] private TextMeshProUGUI _textTimer;
    [SerializeField] private TextMeshProUGUI _textDirection;
    [SerializeField] private EnemySpawner _enemySpawner;
    private bool _showContains = false;
    private bool _showTimer = false;
    private bool _showDirection = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnEnable()
    {
        _enemySpawner.OnWaveArrived += Reset;
    }

    private void OnDisable()
    {
        _enemySpawner.OnWaveArrived -= Reset;
    }

    public void Reset()
    {
        _showContains = _showDirection = _showTimer = false;
        _textContains.text = "???";
        _textTimer.text = "???";
        _textDirection.text = "???";
    }

    private void ChangeDirection()
    {
        _textDirection.text = _enemySpawner.waves[_enemySpawner.waveIndex].direction.ToString();
    }

    private void ChangeContent()
    {
        _textContains.text = _enemySpawner.waves[_enemySpawner.waveIndex].nbOfOrcs.ToString();
    }

    public void UnlockTip()
    {
        if (!_showDirection)
        {
            _showDirection = true;
            ChangeDirection();
        }
        else if (!_showTimer)
        {
            _showTimer = true;
        }
        else if (!_showContains)
        {
            _showContains = true;
            ChangeContent();
        }
    }

    private void Update()
    {
        if (_showTimer)
        {
            _textTimer.text = GameTimer.timeBeforeNextWave.ToString("00");
        }
    }
}
