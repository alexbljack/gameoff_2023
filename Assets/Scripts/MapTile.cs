using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapTile : MonoBehaviour
{
    public TileType plain;
    public TileType rocks;
    public TileType forest;

    SpriteRenderer _renderer;
    bool _can_build;

    void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        TileType tileType = plain;
        float dice = Random.value;
        
        if (dice < 0.1)
        {
            tileType = rocks;
        }

        if (dice >= 0.1 && dice < 0.3)
        {
            tileType = forest;
        }

        if (dice >= 0.3)
        {
            tileType = plain;
        }

        _renderer.sprite = tileType.PickRandom();
        _can_build = plain.isPassable;
    }

    void Update()
    {
        
    }

    void OnMouseEnter()
    {
        Debug.Log("Entered");
    }
}
