using System;
using System.Collections;
using UnityEngine;

public class Construction : MonoBehaviour
{
    public GameObject buildingPrefab;
    public MapTile tile;
    
    public void Build(GameObject building, MapTile mapTile)
    {
        buildingPrefab = building;
        tile = mapTile;
        StartCoroutine(BuildRoutine());
    }

    IEnumerator BuildRoutine()
    {
        float timePassed = 0;
        float buildTime = buildingPrefab.GetComponent<Building>().timeToBuild;
        while (timePassed < buildTime)
        {
            timePassed += Time.deltaTime;
            yield return null;
        }
        GameObject building = Instantiate(buildingPrefab);
        building.transform.position = tile.transform.position;
        building.transform.SetParent(tile.gameObject.transform);
        tile.FinishBuilding();
        Destroy(gameObject);
    }
}