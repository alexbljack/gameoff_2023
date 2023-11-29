using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject lightBox;
    [SerializeField] GameObject acceptBtn;
    [SerializeField] TMP_InputField kingdomName;
    [SerializeField] GameObject History;
    [SerializeField] GameObject HistoryItem;
    [SerializeField] GameObject Content;
    [SerializeField] Account account;

    Dictionary<string, GameObject> cache;

    private void Awake()
    {
        var _account = FindAnyObjectByType<Account>();
        if (_account != null)
            account = _account;
        else
            account = Instantiate(account);
    }

    private void Start()
    {
        cache = new Dictionary<string, GameObject>();
    }

    public void StartNewGame() 
    {
        var _kingdomName = kingdomName.text;
        if (_kingdomName == null)
            _kingdomName = "The suffering Kingdom";
        account.KingdomName = _kingdomName;
        SceneManager.LoadScene(1);
    }

    public void ExitGame() 
    {
        Application.Quit();
    }

    public void ShowLightbox() 
    {
        if (lightBox.activeSelf)
            return;
        lightBox.SetActive(true);
    }

    public void ShowHistory() 
    {
        var accountData = account.LoadData();
        if (accountData.Count > 0) 
        {
            foreach (var item in accountData) 
            {
                if (!cache.ContainsKey(item.path))
                {
                    var historyItem = Instantiate(HistoryItem, parent: Content.transform);
                    historyItem.GetComponent<HistoryItem>().Init(item);
                    cache.Add(item.path, historyItem);
                }
            }
        }

        History.SetActive(true);
    }

    public void CloseHistory() 
    {
        History.SetActive(false);
    }

    public void CloseLightbox()
    {
        if (lightBox.activeSelf)
        {
            kingdomName.text = null;
            acceptBtn.SetActive(false);
            lightBox.SetActive(false);
        }
    }

    public void ShowAcceptBtn() 
    {
        if(kingdomName.text != null)
            acceptBtn.SetActive(true);
    }
}
