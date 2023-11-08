using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TileType : ScriptableObject
{
    public List<Sprite> tiles;
    public bool isPassable;

    public Sprite PickRandom()
    {
        return tiles[Random.Range(0, tiles.Count)];
    }
}
