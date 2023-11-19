using System;
using System.Collections;
using UnityEngine;

public class Construction : MonoBehaviour
{
    [SerializeField] GameObject buildingPrefab;
    [SerializeField] BuildingType buildingType;
    [SerializeField] MapTile tile;
    
    public void Build(BuildingType building, MapTile mapTile)
    {
        buildingType = building;
        tile = mapTile;
        StartCoroutine(BuildRoutine());
    }

    IEnumerator BuildRoutine()
    {
        float timePassed = 0;
        float buildTime = buildingType.TimeToBuild;
        while (timePassed < buildTime)
        {
            timePassed += Time.deltaTime;
            yield return null;
        }
        GameObject building = Instantiate(buildingPrefab);
        building.transform.position = tile.transform.position;
        building.transform.SetParent(tile.gameObject.transform);
        building.GetComponent<Building>().Init(buildingType);
        tile.FinishBuilding();
        Destroy(gameObject);
    }
}