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
    public static event Action<Resource, int> AmountChanged;
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
        AmountChanged?.Invoke(this, value);
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
    public int startPopulation = 3;
    public float secondsPerYear = 60f;
    
    [Header("Population settings")]
    public float populationIncreaseTimeStep = 2f;
    public int populationIncreaseAmountPerStep = 1;
    public ResourceType foodResource;
    public int foodConsumptionPerCitizen = 1;
    public float foodConsumptionStep = 10;
    public float defaultPositiveLoyaltyRate = 1f;
    public float foodDecreaseCoef = 2f;
    public float housingDecreaseCoef = 1f;

    [Header("UI")] 
    [SerializeField] PopulationInfo populationUI;
    [SerializeField] LoyaltyInfo loyaltyUI;
    [SerializeField] EventModal eventModal;
    [SerializeField] TMP_Text kingdomName;
    [SerializeField] TMP_Text yearsOnThrone;
    [SerializeField] GameObject notEnoughResourcesMsg;
    [SerializeField] GameObject acceptLightbox;

    int _yearsOnThrone = 0;

    int _population = 1;
    float _loyalty = 50f;
    Dictionary<ResourceType, Resource> _resources;

    bool _gameOver = false;
    bool _paused = false;
    bool _isFoodShortage = false;
    IEnumerator _alertRoutine;

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

    void PauseGame()
    {
        Time.timeScale = 0;
    }

    void ResumeGame()
    {
        Time.timeScale = 1;
    }

    float GetLoyalityDescreaseRate() 
    {
        var _default = defaultPositiveLoyaltyRate;
        if (Population > Housing && _isFoodShortage)
        {
            _default = -1f;
            _default *= (housingDecreaseCoef + foodDecreaseCoef);
        }
        else if (Population > Housing) 
        {
            _default = -1f;
            _default *= housingDecreaseCoef;
        }
        else if (_isFoodShortage)
        {
            _default = -1f;
            _default *= foodDecreaseCoef;
        }
        return _default * Time.deltaTime;
    }

    void Update()
    {
        ChangeLoyalty(GetLoyalityDescreaseRate());
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
            var value = populationIncreaseAmountPerStep * (_yearsOnThrone != 0 ? _yearsOnThrone : 1);
            ChangePopulation(value);
            yield return new WaitForSeconds(populationIncreaseTimeStep);
        }
    }

    IEnumerator ConsumeFoodRoutine()
    {
        while (!_gameOver)
        {
            yield return new WaitForSeconds(foodConsumptionStep);
            int consumeAmount = _population * foodConsumptionPerCitizen;
            _isFoodShortage = consumeAmount > _resources[foodResource].Amount;
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
        BuildCursor.NotEnoughResources += OnNotEnoughResources;
    }

    void OnDisable()
    {
        Building.ResourceGenerated -= OnChangeResource;
        MapTile.SpentResourcesOnBuilding -= OnChangeResource;
        Building.BuildingCreated -= OnBuildingCreated;
        RandomEventManager.RandomEventTriggered -= OnRandomEvent;
        EventChoiceButton.EventOutcomeSelected -= OnEventChoice;
        BuildCursor.NotEnoughResources -= OnNotEnoughResources;
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
        PauseGame();
    }

    void OnEventChoice(EventChoice choice)
    {
        foreach (EventResMod mod in choice.resources) { ChangeResource(mod.resource, mod.amount); }
        ChangePopulation(choice.populationChange);
        ChangeLoyalty(choice.loyaltyChange);
        eventModal.gameObject.SetActive(false);
        
        ResumeGame();
        
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

    void OnNotEnoughResources()
    {
        if (_alertRoutine != null)
        {
            StopCoroutine(_alertRoutine);
        }
        _alertRoutine = AlertBoxRoutine();
        StartCoroutine(_alertRoutine);
    }

    IEnumerator AlertBoxRoutine()
    {
        notEnoughResourcesMsg.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        notEnoughResourcesMsg.gameObject.SetActive(false);
    }

    public void ShowAcceptLightbox() 
    {
        PauseGame();
        acceptLightbox.SetActive(true);
    }

    public void CloseAcceptLightbox()
    {
        ResumeGame();
        acceptLightbox.SetActive(false);
    }

    public void CloseSession() 
    {
        account.ClearData();
        SceneManager.LoadScene(0);
    }
}
