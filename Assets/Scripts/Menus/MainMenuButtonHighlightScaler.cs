using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuButtonHighlightScaler : MonoBehaviour, IPointerEnterHandler
{
    public List<GameObject> panels;
    
    private void Update()
    {
        ChangeButtonScale(EventSystem.current.currentSelectedGameObject == gameObject ? 0.9f : 1.0f);
    }

    private void ChangeButtonScale(float scale)
    {
        transform.localScale = new Vector3(scale, scale, scale);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (panels.Any(panel => panel.activeSelf)) return;
        EventSystem.current.SetSelectedGameObject(gameObject);
    }
}
