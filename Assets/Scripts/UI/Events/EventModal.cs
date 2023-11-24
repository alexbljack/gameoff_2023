using TMPro;
using UnityEngine;

public class EventModal : MonoBehaviour
{
    [SerializeField] Transform buttonGroup;
    [SerializeField] TextMeshProUGUI eventText;
    [SerializeField] GameObject choicePrefab;

    public void BuildEventWindow(EventData eventData)
    {
        eventText.text = eventData.Description[Random.Range(0, eventData.Description.Count)];
        
        foreach (Transform child in buttonGroup) {
            Destroy(child.gameObject);
        }
        
        foreach (EventChoice choice in eventData.Choices)
        {
            GameObject choiceButton = Instantiate(choicePrefab);
            choiceButton.transform.SetParent(buttonGroup);
            choiceButton.GetComponent<EventChoiceButton>().Init(choice);
            choiceButton.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }
    }
}