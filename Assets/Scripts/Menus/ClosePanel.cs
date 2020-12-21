using UnityEngine;
using UnityEngine.EventSystems;

public class ClosePanel : MonoBehaviour, IPointerClickHandler
{
    public GameObject panel;
    public GameObject buttonToHighlight;

    private void Update()
    {
        if(Input.GetKeyDown("joystick button 1")) 
            Close();
    }

    private void Close()
    {
        panel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(buttonToHighlight);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Close();
    }
}
