using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct EventResMod
{
    public ResourceType resource; 
    public int amount;
}

[Serializable]
public class EventChoice
{
    public string choiceText;
    public float loyaltyChange;
    public float populationChange;
    public BuildingType destroyBuilding;
    public List<EventResMod> resources;
}

[CreateAssetMenu]
public class EventData : ScriptableObject
{
    [TextAreaAttribute]
    [SerializeField] string eventText;
    
    [SerializeField] List<EventChoice> choices;
}