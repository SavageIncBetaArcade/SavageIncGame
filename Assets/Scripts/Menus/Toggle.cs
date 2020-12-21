using UnityEngine;

public class Toggle : MonoBehaviour
{
    public GameObject yesImage;
    public GameObject noImage;
    private new bool enabled = true;

    private void Start()
    {
        SetActiveImage();
    }

    public void ToggleIsOn()
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
