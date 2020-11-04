using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    private void Update()
    {
        var mousePos = Input.mousePosition;
        transform.position = new Vector3(
            mousePos.x + (((RectTransform) transform).rect.width / 2),
            mousePos.y + ((RectTransform)transform).rect.height / 2);
    }
}