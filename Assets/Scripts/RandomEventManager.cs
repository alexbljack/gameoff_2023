using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomEventManager : MonoBehaviour
{
    public static event Action<EventData> RandomEventTriggered;
    
    [SerializeField] List<EventData> positiveEvents;
    [SerializeField] List<EventData> disasterEvents;
    [SerializeField] List<EventData> choiceEvents;
    
    [SerializeField] float eventDelayMin;
    [SerializeField] float eventDelayMax;

    List<EventData> _usedEvents;
    bool lastEventWasBad = false;

    void Start()
    {
        StartCoroutine(RandomEventRoutine());
    }

    IEnumerator RandomEventRoutine()
    {
        EventData currentEvent = PickEvent();
        float toNextEvent = Random.Range(eventDelayMin, eventDelayMax);
        yield return new WaitForSeconds(toNextEvent);
        TriggerEvent(currentEvent);
    }

    EventData PickEvent()
    {
        if (Random.value < 0.66 && !lastEventWasBad)
        {
            lastEventWasBad = true;
            return Utils.PickRandomFromList(disasterEvents);
        }
        else
        {
            var eventType = (Random.value < 0.5) ? positiveEvents : choiceEvents;
            lastEventWasBad = false;
            return Utils.PickRandomFromList(eventType);
        }
    }

    void TriggerEvent(EventData eventData)
    {
        RandomEventTriggered?.Invoke(eventData);
    }

    void OnChoiceSelected(EventChoice choice)
    {
        StartCoroutine(RandomEventRoutine());
    }

    void OnEnable()
    {
        EventChoiceButton.EventOutcomeSelected += OnChoiceSelected;
    }

    void OnDisable()
    {
        EventChoiceButton.EventOutcomeSelected -= OnChoiceSelected;
    }
}
