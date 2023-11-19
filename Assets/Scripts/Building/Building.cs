using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Building : MonoBehaviour
{
    [SerializeField] BuildingType buildingType;

    SpriteRenderer _rndr;

    void Awake()
    {
        _rndr = GetComponent<SpriteRenderer>();
    }

    public void Init(BuildingType building)
    {
        buildingType = building;
        _rndr.sprite = buildingType.Image;
    }
    
    void OnGameTick()
    {
        foreach (ResourceGenerator resource in buildingType.Resources)
        {
            resource.OnTick();
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
