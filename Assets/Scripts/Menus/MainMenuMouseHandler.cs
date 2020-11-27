using UnityEngine;

public class MainMenuMouseHandler : MonoBehaviour
{
    public GameObject instructionsPanel;
    public GameObject optionsPanel;
    public GameObject creditsPanel;
    
    public void OnPlayClicked()
    {
        
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