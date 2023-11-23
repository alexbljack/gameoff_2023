using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEventManager : MonoBehaviour
{
    public static event Action<EventData> RandomEventTriggered;
    
    [SerializeField] EventData randomEvent;
    
    void Start()
    {
        RandomEventTriggered?.Invoke(randomEvent);
    }

    void Update()
    {
        
    }
}
