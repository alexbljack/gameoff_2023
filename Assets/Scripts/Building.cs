using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ResourceType
{
    Wood,
    Money,
    Stone
}


[Serializable]
public class ResourceGenerator
{
    public ResourceType resource;
    public int amount;
    public int ticks;
    
    public static event Action<ResourceType, int> ResourceGenerated;

    int _ticksPassed = 0;

    public void OnTick()
    {
        _ticksPassed += 1;
        if (_ticksPassed >= ticks)
        {
            Generate();
            _ticksPassed = 0;
        }
    }

    void Generate()
    {
        Debug.Log($"Generating {amount} {resource}");
        ResourceGenerated?.Invoke(resource, amount);
    }
}

public class Building : MonoBehaviour
{
    public float timeToBuild = 1f;
    public List<ResourceGenerator> resources;
    
    void OnGameTick()
    {
        foreach (ResourceGenerator resource in resources)
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
