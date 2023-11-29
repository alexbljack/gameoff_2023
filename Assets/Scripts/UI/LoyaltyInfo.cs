using UnityEngine;
using UnityEngine.UI;
using System;

public class LoyaltyInfo : MonoBehaviour
{
    [SerializeField] Slider loyaltyMeter;

    public event Action<int> EndOfLoyalty;
    public void Init(GameManager game)
    {
        loyaltyMeter.value = game.Loyalty;
    }

    void OnEnable()
    {
        GameManager.LoyaltyChanged += OnLoyaltyChanged;
    }

    void OnDisable()
    {
        GameManager.LoyaltyChanged -= OnLoyaltyChanged;
    }

    void OnLoyaltyChanged(float value)
    {
        loyaltyMeter.value = value;
        if (loyaltyMeter.value <= loyaltyMeter.minValue)
            EndOfLoyalty?.Invoke(-1);
        if (loyaltyMeter.value >= loyaltyMeter.maxValue)
            EndOfLoyalty?.Invoke(1);

    }
}

