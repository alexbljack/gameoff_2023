using UnityEngine;

public class BuildCursor : MonoBehaviour
{
    SpriteRenderer _rndr;
    BuildingType _building;
    MapTile _overTile;

    bool _buildMode;

    void Awake()
    {
        _rndr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        ExitBuildMode();
    }

    void OnEnable()
    {
        BuildButton.BuildButtonClicked += EnterBuildMode;
        MapTile.CursorEnteredTile += OnEnterTile;
    }

    void OnDisable()
    {
        BuildButton.BuildButtonClicked -= EnterBuildMode;
        MapTile.CursorEnteredTile -= OnEnterTile;
    }

    void Update()
    {
        if (_buildMode)
        {
            FollowCursor();
            if (Input.GetMouseButtonDown(0)) { TryBuild(); }
        }
    }

    void FollowCursor()
    {
        var cameraPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(cameraPos.x, cameraPos.y, transform.position.z);
    }

    void TryBuild()
    {
        if (_overTile != null && _overTile.CanBuild(_building))
        {
            _overTile.Build(_building);
        }
        else
        {
            ExitBuildMode();
        }
    }
    
    void EnterBuildMode(BuildingType building)
    {
        _rndr.sprite = building.Image;
        _building = building;
        _buildMode = true;
    }

    void ExitBuildMode()
    {
        _rndr.sprite = null;
        _building = null;
        _buildMode = false;
    }

    void Highlight(bool canBuild)
    {
        Color color = canBuild ? Color.green : Color.red;
        _rndr.color = color;
    }

    void OnEnterTile(MapTile tile)
    {
        _overTile = tile;
        Highlight(tile.CanBuild(_building));
    }
}
