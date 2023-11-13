using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapGrid : MonoBehaviour
{
    public GameObject tilePrefab;
    public Vector2 mapSize;
    
    Dictionary<Vector2, MapTile> _tiles;

    void Awake()
    {
        _tiles = new Dictionary<Vector2, MapTile>();
    }

    void Start()
    {
        InitMap();
    }

    void OnEnable()
    {
        MapTile.BuiltOnTile += OnTileBuilt;
    }
    
    void OnDisable()
    {
        MapTile.BuiltOnTile -= OnTileBuilt;
    }

    void InitMap()
    {
        Vector2 center = mapSize / 2;
        for (int i = 0; i < mapSize.x; i++)
        {
            for (int j = 0; j < mapSize.y; j++)
            {
                GameObject tile = Instantiate(tilePrefab);
                Vector2Int position = new Vector2Int(i, j);
                var tileComponent = tile.GetComponent<MapTile>();
                tileComponent.Init(position);
                tile.transform.position = position - center;
                _tiles[new Vector2Int(i, j)] = tileComponent;
                if ((position.x > center.x - 2 && position.x < center.x + 2) 
                    && (position.y > center.y - 2 && position.y < center.y + 2))
                {
                    tileComponent.Open();
                }
            }
        }
    }

    void OnTileBuilt(MapTile tile)
    {
        for (int i = tile.Position.x - 1; i < tile.Position.x + 1; i++)
        {
            for (int j = tile.Position.y - 1; j < tile.Position.y + 1; j++)
            {
                MapTile neighbour = _tiles[new Vector2Int(i, j)];
                if (!neighbour.Opened)
                {
                    neighbour.Open();
                }
            }
        }
    }
}
