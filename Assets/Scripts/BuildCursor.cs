using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildCursor : MonoBehaviour
{
    SpriteRenderer _rndr;
    Collider2D _collider;
    GameObject _building;
    MapTile _overTile;

    bool _buildMode;

    void Awake()
    {
        _rndr = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
    }

    void Start()
    {
        ExitBuildMode();
    }

    void OnEnable()
    {
        BuildButton.BuildButtonClicked += EnterBuildMode;
    }

    void OnDisable()
    {
        BuildButton.BuildButtonClicked -= EnterBuildMode;
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
    
    void EnterBuildMode(GameObject building)
    {
        _rndr.sprite = building.GetComponent<SpriteRenderer>().sprite;
        _building = building;
        _collider.enabled = true;
        _buildMode = true;
    }

    void ExitBuildMode()
    {
        _rndr.sprite = null;
        _building = null;
        _collider.enabled = false;
        _buildMode = false;
    }

    void Highlight(bool canBuild)
    {
        Color color = canBuild ? Color.green : Color.red;
        _rndr.color = color;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out MapTile tile))
        {
            _overTile = tile;
            Highlight(tile.CanBuild(_building));
        }
    }
}
