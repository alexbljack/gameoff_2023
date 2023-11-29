using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HistoryItem : MonoBehaviour
{
    [SerializeField] TMP_Text years;
    [SerializeField] TMP_Text kingdomName;
    [SerializeField] Sprite democraty;
    [SerializeField] Sprite revolution;
    [SerializeField] Image icon;
    public void Init(AccountData data) 
    {
        kingdomName.text = data.KingdomName;
        years.text = $"{data.YearsOnThrone} years";
        var sprite = data.GameResult == -1 ? revolution : democraty;
        icon.sprite = sprite;
    }
}
