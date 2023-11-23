using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Resource
{
    public static event Action<Resource> AmountChanged;
    public int Amount => _amount;
    public ResourceType Type => _type;
    
    int _amount = 0;
    ResourceType _type;

    public Resource(int startValue, ResourceType type)
    {
        Change(startValue);
        _type = type;
    }

    public void Change(int value)
    {
        _amount += value;
        AmountChanged?.Invoke(this);
    }
}


[Serializable]
public struct ResourceHolder
{
    public ResourceType type;
    public int startValue;
}


public class GameManager : MonoBehaviour
{
    public static event Action<int> PopulationChanged;
    public static event Action<int> HousingChanged;
    public static event Action<float> LoyaltyChanged; 

    public List<ResourceHolder> resourceSetup;
    public float populationIncreaseStep = 2;
    public ResourceType foodResource;
    public int foodConsumptionPerCitizen = 1;
    public float foodConsumptionStep = 10;
    public float housingLoyaltyDecreaseRate = 5f;

    [Header("UI")] 
    [SerializeField] PopulationInfo populationUI;
    [SerializeField] LoyaltyInfo loyaltyUI;
    [SerializeField] EventModal eventModal;
    
    int _population = 1;
    float _loyalty = 50f;
    Dictionary<ResourceType, Resource> _resources;
    
    bool _gameOver = false;
    
    public Dictionary<ResourceType, Resource> Resources => _resources;
    public int Population => _population;
    public float Loyalty => _loyalty;

    public int Housing => GameObject
        .FindGameObjectsWithTag("Building")
        .Select(b => b.GetComponent<Building>().buildingType.Housing)
        .Sum();
    
    void Awake()
    {
        _resources = new Dictionary<ResourceType, Resource>();
        foreach (ResourceHolder res in resourceSetup)
        {
            _resources[res.type] = new Resource(res.startValue, res.type);
        }
    }

    void Start()
    {
        StartCoroutine(PopulationRoutine());
        StartCoroutine(ConsumeFoodRoutine());
        InitUI();
    }

    void Update()
    {
        if (Population > Housing)
        {
            _loyalty -= housingLoyaltyDecreaseRate * Time.deltaTime;
            LoyaltyChanged?.Invoke(Loyalty);
        }
    }

    IEnumerator PopulationRoutine()
    {
        while (!_gameOver)
        {
            _population += 1;
            PopulationChanged?.Invoke(_population);
            yield return new WaitForSeconds(populationIncreaseStep);
        }
    }

    IEnumerator ConsumeFoodRoutine()
    {
        while (!_gameOver)
        {
            yield return new WaitForSeconds(foodConsumptionStep);
            int consumeAmount = _population * foodConsumptionPerCitizen;
            _resources[foodResource].Change(-consumeAmount);
        }
    }

    void InitUI()
    {
        populationUI.Init(this);
        loyaltyUI.Init(this);
    }

    void OnEnable()
    {
        Building.ResourceGenerated += OnChangeResource;
        MapTile.SpentResourcesOnBuilding += OnChangeResource;
        Building.BuildingCreated += OnBuildingCreated;
        RandomEventManager.RandomEventTriggered += OnRandomEvent;
        EventChoiceButton.EventOutcomeSelected += OnEventChoice;
    }

    void OnDisable()
    {
        Building.ResourceGenerated -= OnChangeResource;
        MapTile.SpentResourcesOnBuilding -= OnChangeResource;
        Building.BuildingCreated -= OnBuildingCreated;
        RandomEventManager.RandomEventTriggered -= OnRandomEvent;
        EventChoiceButton.EventOutcomeSelected -= OnEventChoice;
    }

    void OnChangeResource(ResourceType resource, int amount)
    {
        string action = amount >= 0 ? "Received" : "Spent";  
        Debug.Log($"{action} {amount} of {resource}");
        _resources[resource].Change(amount);
    }

    void OnBuildingCreated()
    {
        HousingChanged?.Invoke(Housing);
    }

    void OnRandomEvent(EventData eventData)
    {
        eventModal.gameObject.SetActive(true);
        eventModal.BuildEventWindow(eventData);
    }

    void OnEventChoice(EventChoice choice)
    {
        eventModal.gameObject.SetActive(false);
    }
}
