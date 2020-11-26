using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject optionsPanel;
    
    public void ClosePauseMenu()
    {
        //return to game
    }

    public void ReturnToMainMenu()
    {
        
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void OpenOptions()
    {
        optionsPanel.SetActive(true);
    }
}
