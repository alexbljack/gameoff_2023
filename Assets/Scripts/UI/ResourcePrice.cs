using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourcePrice : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI priceText;

    public void Init(ResourceCost cost)
    {
        image.sprite = cost.resource.icon;
        priceText.text = cost.cost.ToString();
    }
}