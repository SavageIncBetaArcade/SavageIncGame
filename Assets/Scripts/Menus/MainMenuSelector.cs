using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuSelector : MonoBehaviour
{
    public GameObject instructionsPanel;
    public GameObject optionsPanel;
    public GameObject creditsPanel;
    public GameObject instructionsFirstObject;
    public GameObject optionsFirstObject;
    public GameObject creditsFirstObject;
    public int GameSceneIndex = 1;
    
    public void OnPlayClicked()
    {
        SceneManager.LoadSceneAsync(GameSceneIndex);
    }
    
    public void OnOptionsClicked()
    {
        optionsPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(optionsFirstObject);
    }
    
    public void OnInstructionsClicked()
    {
        instructionsPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(instructionsFirstObject);
    }
    
    public void OnCreditsClicked()
    {
        creditsPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(creditsFirstObject);
    }
    
    public void OnExitClicked()
    {
        Application.Quit();
    }
}