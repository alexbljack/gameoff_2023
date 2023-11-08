using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float tick_length = 1f;
    public static event Action GameTicked;
    public static event Action<int> WoodChanged; 
    public bool is_paused;
    
    public int wood = 0;
    public int money = 0;

    void Start()
    {
        StartCoroutine(GameTickLoop());
    }

    void OnEnable()
    {
    }

    void OnDisable()
    {
        
    }

    void Update()
    {
        
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
