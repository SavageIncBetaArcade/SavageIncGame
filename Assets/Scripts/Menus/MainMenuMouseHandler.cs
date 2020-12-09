using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuMouseHandler : MonoBehaviour
{
    public GameObject instructionsPanel;
    public GameObject optionsPanel;
    public GameObject creditsPanel;
    public int GameSceneIndex = 1;
    
    public void OnPlayClicked()
    {
        SceneManager.LoadSceneAsync(GameSceneIndex);
    }
    
    public void OnInstructionsClicked()
    {
        instructionsPanel.SetActive(true);
    }
    
    public void OnOptionsClicked()
    {
        optionsPanel.SetActive(true);
    }
    
    public void OnCreditsClicked()
    {
        creditsPanel.SetActive(true);
    }
    
    public void OnExitClicked()
    {
        Application.Quit();
    }
}