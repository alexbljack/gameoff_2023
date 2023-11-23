using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResModifier : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Image icon;
    
    public void Init(EventResMod mod)
    {
        icon.sprite = mod.resource.icon;
        string sign = Math.Sign(mod.amount) > 0 ? "+" : "-";
        text.text = $"{sign}{Math.Abs(mod.amount)}";
    }
}