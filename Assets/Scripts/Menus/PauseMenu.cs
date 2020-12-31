using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject optionsPanel;
    public GameObject optionsFirstObject;
    
    public void ClosePauseMenu()
    {
        if (!optionsPanel.activeSelf)
        {
            //return to game
        }
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void OpenOptions()
    {
        optionsPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(optionsFirstObject);
    }
}
