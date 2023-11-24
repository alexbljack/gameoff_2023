using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

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
    bool _paused = false;
    
    public Dictionary<ResourceType, Resource> Resources => _resources;
    public int Population => _population;
    public float Loyalty => _loyalty;

    public List<Building> Buildings => GameObject.FindGameObjectsWithTag("Building")
        .Select(b => b.GetComponent<Building>()).ToList();
    
    public int Housing => Buildings.Select(b => b.buildingType.Housing).Sum();
    
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
            ChangeLoyalty(-housingLoyaltyDecreaseRate * Time.deltaTime);
        }
    }

    IEnumerator PopulationRoutine()
    {
        while (!_gameOver)
        {
            ChangePopulation(1);
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

    void ChangeResource(ResourceType resource, int amount)
    {
        string action = amount >= 0 ? "Received" : "Spent";  
        Debug.Log($"{action} {amount} of {resource}");
        _resources[resource].Change(amount);
    }

    void ChangePopulation(int amount)
    {
        _population += amount;
        PopulationChanged?.Invoke(_population);
    }

    void ChangeLoyalty(float amount)
    {
        _loyalty += amount;
        LoyaltyChanged?.Invoke(Loyalty);
    }
 
    void OnChangeResource(ResourceType resource, int amount)
    {
        ChangeResource(resource, amount);
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
        foreach (EventResMod mod in choice.resources) { ChangeResource(mod.resource, mod.amount); }
        ChangePopulation(choice.populationChange);
        ChangeLoyalty(choice.loyaltyChange);
        eventModal.gameObject.SetActive(false);

        if (choice.destroyBuilding)
        {
            var buildings = Buildings.FindAll(b => b.buildingType == choice.destroyBuilding);
            if (buildings.Count > 0)
            {
                var building = buildings[Random.Range(0, buildings.Count)];
                building.DestroyBuilding();
            }
        }
    }
}
