using UnityEngine;
using UnityEngine.EventSystems;

public class ClosePanel : MonoBehaviour, IPointerClickHandler
{
    public GameObject panel;


    public void OnPointerClick(PointerEventData eventData)
    {
        panel.SetActive(false);
    }
}
