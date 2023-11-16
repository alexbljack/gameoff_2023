using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Resource
{
    public static event Action<Resource> AmountChanged;
    public int Amount => _amount;
    public ResourceType Type => _type;
    
    int _amount = 0;
    ResourceType _type;

    public Resource(int startValue, ResourceType type)
    {
        Change(startValue);
        _type = type;
    }

    public void Change(int value)
    {
        _amount += value;
        AmountChanged?.Invoke(this);
    }
}


[Serializable]
public struct ResourceHolder
{
    public ResourceType type;
    public int startValue;
}


public class GameManager : MonoBehaviour
{
    public float tick_length = 1f;
    public static event Action GameTicked;
    
    public bool is_paused;

    public List<ResourceHolder> resourceSetup;
    public Dictionary<ResourceType, Resource> Resources => _resources;

    Dictionary<ResourceType, Resource> _resources;

    void Awake()
    {
        _resources = new Dictionary<ResourceType, Resource>();
        foreach (ResourceHolder res in resourceSetup)
        {
            _resources[res.type] = new Resource(res.startValue, res.type);
        }
    }

    void Start()
    {
        StartCoroutine(GameTickLoop());
    }

    void OnEnable()
    {
        ResourceGenerator.ResourceGenerated += OnChangeResource;
        MapTile.SpentResourcesOnBuilding += OnChangeResource;
    }

    void OnDisable()
    {
        ResourceGenerator.ResourceGenerated -= OnChangeResource;
        MapTile.SpentResourcesOnBuilding -= OnChangeResource;
    }

    void OnChangeResource(ResourceType resource, int amount)
    {
        string action = amount >= 0 ? "Received" : "Spent";  
        Debug.Log($"{action} {amount} of {resource}");
        _resources[resource].Change(amount);
    }

    IEnumerator GameTickLoop()
    {
        while (!is_paused)
        {
            yield return new WaitForSeconds(tick_length);
            GameTicked?.Invoke();
        }
    }
}
