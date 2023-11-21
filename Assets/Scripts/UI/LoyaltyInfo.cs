using UnityEngine;
using UnityEngine.UI;

public class LoyaltyInfo : MonoBehaviour
{
    [SerializeField] Slider loyaltyMeter;

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
    }
}

