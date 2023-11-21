using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;


public class Building : MonoBehaviour
{
    public static event Action<ResourceType, int> ResourceGenerated;
    public static event Action BuildingCreated;
    public static event Action BuildingDestroyed;
    
    public BuildingType buildingType;

    SpriteRenderer _rndr;
    bool _generatingResources = true;

    void Awake()
    {
        _rndr = GetComponent<SpriteRenderer>();
    }

    public void Init(BuildingType building)
    {
        buildingType = building;
        _rndr.sprite = buildingType.Image;
        BuildingCreated?.Invoke();
        
        foreach (ResourceGenerator generator in building.Resources)
        {
            StartCoroutine(GenerateResourceRoutine(generator.resource, generator.amount, generator.timeStep));
        }
    }

    IEnumerator GenerateResourceRoutine(ResourceType resource, int amount, float delay)
    {
        while (_generatingResources)
        {
            yield return new WaitForSeconds(delay);
            Debug.Log($"Generating {amount} {resource}");
            ResourceGenerated?.Invoke(resource, amount);
        } 
    }
}
