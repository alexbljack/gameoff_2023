using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopulationInfo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI populationText;
    [SerializeField] TextMeshProUGUI housingText;

    GameManager _game;
    
    void OnEnable()
    {
        GameManager.PopulationChanged += OnPopulationChange;
        GameManager.HousingChanged += OnHousingChange;
    }

    void OnDisable()
    {
        GameManager.PopulationChanged -= OnPopulationChange;
    }

    public void Init(GameManager game)
    {
        SetPopulationText(game.Population);
        SetHousingText(game.Housing);
    }

    void OnPopulationChange(int value)
    {
        SetPopulationText(value);
    }

    void OnHousingChange(int value)
    {
        SetHousingText(value);
    }

    void SetPopulationText(int population)
    {
        populationText.text = population.ToString();
    }

    void SetHousingText(int housing)
    {
        housingText.text = housing.ToString();
    }
}
