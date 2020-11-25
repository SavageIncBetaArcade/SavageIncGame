using UnityEngine;

public class MainMenuMouseHandler : MonoBehaviour
{
    public GameObject instructionsPanel;
    public GameObject optionsPanel;
    
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
        
    }
    
    public void OnExitClicked()
    {
        Application.Quit();
    }
}