using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float tick_length = 1f;
    public static event Action GameTicked; 
    public bool is_paused;

    public int wood = 0;
    public int stone = 0;
    public int reputation = 0;
    public int money = 0;

    void Start()
    {
        StartCoroutine(GameTickLoop());
    }

    void OnEnable()
    {
        ResourceGenerator.ResourceGenerated += OnGetResource;
    }

    void OnDisable()
    {
        ResourceGenerator.ResourceGenerated -= OnGetResource;
    }

    void Update()
    {
        
    }

    void OnGetResource(ResourceType resource, int amount)
    {
        Debug.Log($"Received {amount} of {resource}");
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
