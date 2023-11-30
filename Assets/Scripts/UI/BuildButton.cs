using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] BuildingType buildingType;

    [SerializeField] GameObject infoPanel;
    [SerializeField] TextMeshProUGUI incomeText;
    [SerializeField] Image incomeResource;
    [SerializeField] GameObject resourcePricePrefab;
    [SerializeField] Transform priceContainer;
    
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
        infoPanel.gameObject.SetActive(false);
    }

    public void Init(BuildingType building)
    {
        buildingType = building;
        _image.sprite = buildingType.Image;

        ResourceGenerator income = building.Resources[0];
        incomeText.text = $"{income.RatePerMin().ToString()}";
        incomeResource.sprite = income.resource.icon;

        foreach (ResourceCost cost in building.Cost)
        {
            var costObj = Instantiate(resourcePricePrefab);
            costObj.transform.SetParent(priceContainer);
            costObj.GetComponent<ResourcePrice>().Init(cost);
        }
    }

    void OnClick()
    {
        BuildButtonClicked?.Invoke(buildingType);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        infoPanel.gameObject.SetActive(true);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        infoPanel.gameObject.SetActive(false);
    }
}
