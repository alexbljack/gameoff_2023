using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TileType : ScriptableObject
{
    public List<Sprite> tiles;
    public List<BuildingType> buildingsAvailable;
    public Sprite resourceIcon;

    public Sprite PickRandom()
    {
        return tiles[Random.Range(0, tiles.Count)];
    }
}
