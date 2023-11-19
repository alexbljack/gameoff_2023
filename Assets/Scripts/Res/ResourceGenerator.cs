

using System;
using UnityEngine;


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
