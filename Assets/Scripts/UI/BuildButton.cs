using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildButton : MonoBehaviour
{
    public GameObject buildingPrefab;
    public static event Action<GameObject> BuildButtonClicked;

    Button _btn;
    
    void Start()
    {
        _btn = GetComponent<Button>();
        _btn.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        Debug.Log(buildingPrefab);
        BuildButtonClicked?.Invoke(buildingPrefab);
    }
}
