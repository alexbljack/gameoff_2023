using UnityEngine.SceneManagement;
using UnityEngine;

public class FinalSceneManager : MonoBehaviour
{
    Account account;

    [SerializeField] GameObject democraty;
    [SerializeField] GameObject revolution;

    private void Awake()
    {
        account = FindAnyObjectByType<Account>();    
    }

    private void Start()
    {
        if (account == null)
        {
            democraty.SetActive(false);
            revolution.SetActive(true);
            Debug.LogError($"Account не был создан. ¬озможно игра была запущена не с главного меню");
        }
        else if (account.GameResult == -1)
        {
            democraty.SetActive(false);
            revolution.SetActive(true);
        }
        else if (account.GameResult == 1)
        {
            democraty.SetActive(true);
            revolution.SetActive(false);
        }
        else 
        {
            Debug.LogError($"GameResult: {account.GameResult}. ј должен быть -1 или 1!");
        }
    }

    public void CloseFinalScene() 
    {
        if (account != null)
            account.SaveData();
        SceneManager.LoadScene(0);
    }
}
