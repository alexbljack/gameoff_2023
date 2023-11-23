using System.Collections.Generic;
using UnityEngine;

public class BuildingsPanel : MonoBehaviour
{
    [SerializeField] List<BuildingType> buildings;
    [SerializeField] GameObject buildButtonPrefab;
    
    void Start()
    {
        foreach (BuildingType building in buildings)
        {
            GameObject button = Instantiate(buildButtonPrefab);
            button.transform.SetParent(transform);
            button.GetComponent<BuildButton>().Init(building);
            button.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }
    }
}
