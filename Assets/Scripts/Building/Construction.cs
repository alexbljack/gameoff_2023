using UnityEngine.UI;
using System.Collections;
using UnityEngine;

public class Construction : MonoBehaviour
{
    [SerializeField] GameObject buildingPrefab;
    [SerializeField] BuildingType buildingType;
    [SerializeField] MapTile tile;
    [SerializeField] Slider _construction;
    
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
        _construction.maxValue = buildTime;
        while (timePassed < buildTime)
        {
            var _time = Time.deltaTime;
            _construction.value += _time;
            timePassed += _time;
            yield return null;
        }
        GameObject building = Instantiate(buildingPrefab);
        building.transform.position = tile.transform.position;
        building.transform.SetParent(tile.gameObject.transform);
        building.GetComponent<Building>().Init(buildingType, tile);
        tile.FinishBuilding();
        Destroy(gameObject);
    }
}