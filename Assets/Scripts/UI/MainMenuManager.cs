using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject lightBox;
    [SerializeField] GameObject acceptBtn;
    [SerializeField] TMP_InputField kingdomName;

    public void StartNewGame() 
    {
        var _kingdomName = kingdomName.text;
        if (_kingdomName == null)
            _kingdomName = "The suffering Kingdom";
        PlayerPrefs.SetString("KingdomName", _kingdomName);
        PlayerPrefs.Save();
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
