using UnityEngine;
using UnityEngine.EventSystems;

public class Toggle : MonoBehaviour, IPointerClickHandler
{
    public GameObject yesImage;
    public GameObject noImage;
    private bool enabled = true;

    private void Start()
    {
        SetActiveImage();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        enabled = !enabled;
        SetActiveImage();
    }

    private void SetActiveImage()
    {
        yesImage.SetActive(enabled);
        noImage.SetActive(!enabled);
    }
}
