using System;
using UnityEngine;


[Serializable]
public class ResourceGenerator
{
    public ResourceType resource;
    public int amount;
    public float timeStep;

    public int RatePerMin()
    {
        return (int) (60 / timeStep) * amount;
    }
}
