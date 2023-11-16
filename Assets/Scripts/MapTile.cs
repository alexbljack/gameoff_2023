using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class MapTile : MonoBehaviour
{
    public static event Action<MapTile> BuiltOnTile;
    public static event Action<ResourceType, int> SpentResourcesOnBuilding; 

    public TileType plain;
    public TileType rocks;
    public TileType forest;

    SpriteRenderer _renderer;

    Vector2Int _position = Vector2Int.zero;
    bool _opened = false;
    bool _occupied = false;
    TileType _type;

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
        _type = ChooseTile();
        _renderer.sprite = _type.PickRandom();
    }

    TileType ChooseTile()
    {
        float dice = Random.value;
        if (dice < 0.1) { return rocks; }
        if (dice >= 0.1 && dice < 0.5) { return forest; }
        return plain;
    }

    public void Build(GameObject building)
    {
        GameObject construction = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Buildings/Construction.prefab"); 
        GameObject obj = Instantiate(construction);
        obj.transform.position = transform.position;
        obj.transform.SetParent(gameObject.transform);
        obj.GetComponent<Construction>().Build(building, this);
        _occupied = true;
        foreach (var cost in building.GetComponent<Building>().buildCost)
        {
            SpentResourcesOnBuilding?.Invoke(cost.resource, -cost.cost);
        }
    }

    public void FinishBuilding()
    {
        BuiltOnTile?.Invoke(this);
    }

    public bool CanBuild(GameObject building)
    {
        if (!_opened || _occupied) { return false; }
        return _type.buildingsAvailable.Contains(building);
    }

    void OnMouseDown()
    {
        
    }
}
