using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapTile : MonoBehaviour
{
    public static event Action<MapTile> BuiltOnTile;
    
    public TileType plain;
    public TileType rocks;
    public TileType forest;

    SpriteRenderer _renderer;

    Vector2Int _position = Vector2Int.zero;
    bool _opened = false;
    bool _occupied = false;

    public Vector2Int Position => _position;
    public bool Opened => _opened;
    public bool Occupied => _occupied;
    
    public void Init(Vector2Int position)
    {
        _position = position;
        name = $"Tile {position.x}x{position.y}";
    }

    void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    public void Open()
    {
        _opened = true;
        _renderer.sprite = ChooseTile().PickRandom();
    }

    TileType ChooseTile()
    {
        float dice = Random.value;
        if (dice < 0.1) { return rocks; }
        if (dice >= 0.1 && dice < 0.5) { return forest; }
        return plain;
    }

    void OnMouseEnter()
    {
        // Debug.Log("Entered");
    }

    void OnMouseDown()
    {
        if (_opened)
        {
            BuiltOnTile?.Invoke(this);
        }
    }
}
