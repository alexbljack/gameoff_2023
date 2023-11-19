using System;
using UnityEngine;
using UnityEngine.UI;

public class BuildButton : MonoBehaviour
{
    [SerializeField] BuildingType buildingType;
    public static event Action<BuildingType> BuildButtonClicked;

    Image _image;
    Button _btn;

    void Awake()
    {
        _btn = GetComponent<Button>();
        _image = GetComponent<Image>();
    }

    void Start()
    {
        _btn.onClick.AddListener(OnClick);
    }

    public void Init(BuildingType building)
    {
        buildingType = building;
        _image.sprite = buildingType.Image;
    }

    void OnClick()
    {
        BuildButtonClicked?.Invoke(buildingType);
    }
}
