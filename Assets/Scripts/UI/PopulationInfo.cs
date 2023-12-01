using TMPro;
using UnityEngine;

public class PopulationInfo : MonoBehaviour
{
    [SerializeField] GameManager game;
    [SerializeField] TextMeshProUGUI populationText;
    [SerializeField] TextMeshProUGUI housingText;
    [SerializeField] GameObject changeTextPrefab;
    [SerializeField] Vector3 popChangeTextPosition;
    [SerializeField] Vector3 housingChangeTextPosition;

    GameManager _game;
    
    void OnEnable()
    {
        GameManager.PopulationChanged += OnPopulationChange;
        Building.BuildingCreated += OnHousingChange;
        Building.BuildingDestroyed += OnHousingChange;
    }

    void OnDisable()
    {
        GameManager.PopulationChanged -= OnPopulationChange;
        Building.BuildingCreated -= OnHousingChange;
        Building.BuildingDestroyed -= OnHousingChange;
    }

    public void Init(GameManager gameManager)
    {
        SetPopulationText(gameManager.Population);
        SetHousingText(gameManager.Housing);
    }

    void OnPopulationChange(int value, int change)
    {
        SetPopulationText(value);
        ShowChange(change, popChangeTextPosition);
    }

    void OnHousingChange(int change)
    {
        SetHousingText(game.Housing);
        ShowChange(change, housingChangeTextPosition);
    }

    void SetPopulationText(int population)
    {
        populationText.text = population.ToString();
    }

    void SetHousingText(int housing)
    {
        housingText.text = housing.ToString();
    }

    void ShowChange(int change, Vector3 position)
    {
        if (change != 0)
        {
            GameObject changeTxt = Instantiate(changeTextPrefab);
            changeTxt.transform.SetParent(transform);
            RectTransform rect = changeTxt.GetComponent<RectTransform>();
            rect.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            rect.localPosition = position;
            changeTxt.GetComponent<ResourceChangeText>().Init(change);
        }
    }
}
