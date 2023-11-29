using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
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
        _amount = Mathf.Clamp(_amount, 0, 10000000);
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
    
    [Header("Setup")]
    public List<ResourceHolder> resourceSetup;
    public float startLoyalty = 75f;
    public int startPopulation = 1;
    public float secondsPerYear = 60f;
    
    [Header("Population settings")]
    public float populationIncreaseTimeStep = 2f;
    public int populationIncreaseAmountPerStep = 1;
    public ResourceType foodResource;
    public int foodConsumptionPerCitizen = 1;
    public float foodConsumptionStep = 10;
    public float loyaltyDecreaseOnFoodDeplete = 15f;
    public float housingLoyaltyDecreaseRate = 5f;

    [Header("UI")] 
    [SerializeField] PopulationInfo populationUI;
    [SerializeField] LoyaltyInfo loyaltyUI;
    [SerializeField] EventModal eventModal;
    [SerializeField] TMP_Text kingdomName;
    [SerializeField] TMP_Text yearsOnThrone;

    int _yearsOnThrone = 0;

    int _population = 1;
    float _loyalty = 50f;
    Dictionary<ResourceType, Resource> _resources;
    
    bool _gameOver = false;
    bool _paused = false;

    Account account;
    
    public Dictionary<ResourceType, Resource> Resources => _resources;
    public int Population => _population;
    public float Loyalty => _loyalty;

    public List<Building> Buildings => GameObject.FindGameObjectsWithTag("Building")
        .Select(b => b.GetComponent<Building>()).ToList();
    
    public int Housing => Buildings.Select(b => b.buildingType.Housing).Sum();
    
    void Awake()
    {
        account = FindAnyObjectByType<Account>();
        if (account != null)
            kingdomName.text = account.KingdomName;
        _resources = new Dictionary<ResourceType, Resource>();
        foreach (ResourceHolder res in resourceSetup)
        {
            _resources[res.type] = new Resource(res.startValue, res.type);
        }
    }

    void Start()
    {
        _loyalty = startLoyalty;
        _population = startPopulation;
        loyaltyUI.EndOfLoyalty += GameOver;
        StartCoroutine(PopulationRoutine());
        StartCoroutine(ConsumeFoodRoutine());
        StartCoroutine(YearsChangeRoutine());
        InitUI();
    }

    void Update()
    {
        if (Population > Housing)
        {
            ChangeLoyalty(-housingLoyaltyDecreaseRate * Time.deltaTime);
        }
    }

    void GameOver(int result) 
    {
        _gameOver = true;
        if (account != null)
        {
            account.GameResult = result;
            account.YearsOnThrone = _yearsOnThrone;
        }
        SceneManager.LoadScene(2);
    }

    IEnumerator PopulationRoutine()
    {
        while (!_gameOver)
        {
            ChangePopulation(populationIncreaseAmountPerStep);
            yield return new WaitForSeconds(populationIncreaseTimeStep);
        }
    }

    IEnumerator ConsumeFoodRoutine()
    {
        while (!_gameOver)
        {
            yield return new WaitForSeconds(foodConsumptionStep);
            int consumeAmount = _population * foodConsumptionPerCitizen;
            if (consumeAmount > _resources[foodResource].Amount)
            {
                ChangeLoyalty(-loyaltyDecreaseOnFoodDeplete);
            }
            _resources[foodResource].Change(-consumeAmount);
        }
    }

    IEnumerator YearsChangeRoutine() 
    {
        while (!_gameOver)
        {
            yearsOnThrone.text = $"{_yearsOnThrone} years";
            yield return new WaitForSeconds(secondsPerYear);
            _yearsOnThrone++;
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
