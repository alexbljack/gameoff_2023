using System;
using System.Collections;
using UnityEngine;

public class RandomEventManager : MonoBehaviour
{
    public static event Action<EventData> RandomEventTriggered;
    
    [SerializeField] EventData randomEvent;
    [SerializeField] float eventDelay;

    void Start()
    {
        StartCoroutine(RandomEventRoutine());
    }

    IEnumerator RandomEventRoutine()
    {
        yield return new WaitForSeconds(eventDelay);
        TriggerEvent(randomEvent);
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
