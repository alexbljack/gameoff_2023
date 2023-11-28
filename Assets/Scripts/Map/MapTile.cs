using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public struct TileProb
{
    public TileType type;
    public float probability;
}


public class MapTile : MonoBehaviour
{
    public static event Action<MapTile> CursorEnteredTile;
    public static event Action<MapTile> CursorLeftTile; 
    public static event Action<MapTile> BuiltOnTile;
    public static event Action<ResourceType, int> SpentResourcesOnBuilding;

    [SerializeField] SpriteRenderer resourceIcon;
    public List<TileProb> tileProbabilities;

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
        resourceIcon.gameObject.SetActive(false);
    }

    public void Open()
    {
        _opened = true;
        _type = ChooseTile();
        _renderer.sprite = _type.PickRandom();
        if (_type.resourceIcon != null)
        {
            resourceIcon.sprite = _type.resourceIcon;
            resourceIcon.gameObject.SetActive(true);
        }
    }

    TileType ChooseTile()
    {
        var tilesToChoose = new List<TileType>();
        foreach (TileProb tileProb in tileProbabilities)
        {
            for (var i = 0; i < tileProb.probability * 100; i++)
            {
                tilesToChoose.Add(tileProb.type);
            }
        }
        return Utils.PickRandomFromList(tilesToChoose);
    }

    public void Build(BuildingType building)
    {
        GameObject construction = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Buildings/Construction.prefab"); 
        GameObject obj = Instantiate(construction);
        obj.transform.position = transform.position;
        obj.transform.SetParent(gameObject.transform);
        obj.GetComponent<Construction>().Build(building, this);
        _occupied = true;
        resourceIcon.gameObject.SetActive(false);
        foreach (var cost in building.Cost)
        {
            SpentResourcesOnBuilding?.Invoke(cost.resource, -cost.cost);
        }
    }

    public void FinishBuilding()
    {
        BuiltOnTile?.Invoke(this);
    }

    public bool CanBuild(BuildingType building)
    {
        if (!_opened || _occupied) { return false; }
        return _type.buildingsAvailable.Contains(building);
    }

    public void Free()
    {
        _occupied = false;
    }

    public void OnMouseEnter()
    {
        CursorEnteredTile?.Invoke(this);
    }

    public void OnMouseExit()
    {
        CursorLeftTile?.Invoke(this);
    }
}
