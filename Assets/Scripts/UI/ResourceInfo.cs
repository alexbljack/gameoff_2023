using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceInfo : MonoBehaviour
{
    [SerializeField] ResourceType resource;
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI amountText;
    [SerializeField] GameManager game;
    
    void Start()
    {
        icon.sprite = resource.icon;
        if (game != null)
        {
            SetAmount(game.Resources[resource].Amount);
        }
    }

    void SetAmount(int amount)
    {
        amountText.text = amount.ToString();
    }

    void OnResourceUpdated(Resource res)
    {
        if (res.Type == resource)
        {
            SetAmount(res.Amount);
        }
    }

    void OnEnable()
    {
        Resource.AmountChanged += OnResourceUpdated;
    }

    void OnDisable()
    {
        Resource.AmountChanged -= OnResourceUpdated;
    }
}
