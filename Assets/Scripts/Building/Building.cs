using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;


public class Building : MonoBehaviour
{
    public static event Action<ResourceType, int> ResourceGenerated;
    public static event Action<int> BuildingCreated;
    public static event Action<int> BuildingDestroyed;
    
    public BuildingType buildingType;

    [SerializeField] GameObject PoofEffect;

    SpriteRenderer _rndr;
    bool _generatingResources = true;
    MapTile _tile;

    void Awake()
    {
        _rndr = GetComponent<SpriteRenderer>();
    }

    public void Init(BuildingType building, MapTile tile)
    {
        buildingType = building;
        _tile = tile;
        _rndr.sprite = buildingType.Image;
        BuildingCreated?.Invoke(building.Housing);
        
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

    public void DestroyBuilding()
    {
        BuildingDestroyed?.Invoke(-buildingType.Housing);
        _tile.Free();
        Instantiate(PoofEffect, position: gameObject.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
