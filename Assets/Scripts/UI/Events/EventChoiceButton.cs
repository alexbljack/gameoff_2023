using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventChoiceButton : MonoBehaviour
{
    public static event Action<EventChoice> EventOutcomeSelected;

    [SerializeField] TextMeshProUGUI choiceText;
    [SerializeField] Image loyaltyIcon;
    [SerializeField] Sprite loyaltyGood;
    [SerializeField] Sprite loyaltyBad;
    [SerializeField] Transform resourcesGroup;
    [SerializeField] GameObject resourceModPrefab;

    EventChoice _choice;

    public void Init(EventChoice choice)
    {
        _choice = choice;
        
        choiceText.text = choice.choiceText;
        loyaltyIcon.sprite = choice.loyaltyChange < 0 ? loyaltyBad : loyaltyGood;
        
        foreach (Transform child in resourcesGroup) {
            Destroy(child.gameObject);
        }
        
        foreach (EventResMod resMod in choice.resources)
        {
            var resTip = Instantiate(resourceModPrefab);
            resTip.transform.SetParent(resourcesGroup);
            resTip.GetComponent<ResModifier>().Init(resMod);
        }
        
        GetComponent<Button>().onClick.AddListener(OnChoice);
    }

    void OnChoice()
    {
        EventOutcomeSelected?.Invoke(_choice);
    }
}