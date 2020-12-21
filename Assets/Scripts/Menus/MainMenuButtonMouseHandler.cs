using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuButtonMouseHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
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
        ChangeButtonScale(0.9f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ChangeButtonScale(1.0f);
    }
}
