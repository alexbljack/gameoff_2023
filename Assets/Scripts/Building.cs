using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public int wood_speed;
    public int wood_amount;

    int _woodTicksPassed;
    
    void Start()
    {
        _woodTicksPassed = 0;
    }

    void OnGameTick()
    {
        _woodTicksPassed += 1;
        if (_woodTicksPassed >= wood_speed)
        {
            Debug.Log($"Generating {wood_amount}");
            _woodTicksPassed = 0;
        }
    }

    void OnEnable()
    {
        GameManager.GameTicked += OnGameTick;
    }

    void OnDisable()
    {
        GameManager.GameTicked -= OnGameTick;
    }
}
