using UnityEngine;

public class MainMenuMouseHandler : MonoBehaviour
{
    public GameObject InstructionsPanel;
    public GameObject OptionsPanel;
    
    public void OnPlayClicked()
    {
        
    }
    
    public void OnInstructionsClicked()
    {
        InstructionsPanel.SetActive(true);
    }
    
    public void OnOptionsClicked()
    {
        OptionsPanel.SetActive(true);
    }
    
    public void OnCreditsClicked()
    {
        
    }
    
    public void OnExitClicked()
    {
        Application.Quit();
    }
}