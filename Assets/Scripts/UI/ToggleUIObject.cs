using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleUIObject : MonoBehaviour
{
    public RectTransform UITransform;
    public string ButtonName;

    public bool Visible = true;
    public bool ShowMouse = true;
    public bool PauseGame = true;

    // Start is called before the first frame update
    void Start()
    {
        UITransform?.gameObject.SetActive(Visible);
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonUp(ButtonName))
        {
            Visible = !Visible;
            UITransform?.gameObject.SetActive(Visible);

            if(ShowMouse && Visible)
                Cursor.lockState = CursorLockMode.None;
            else if (ShowMouse && !Visible)
                Cursor.lockState = CursorLockMode.Locked;

            if (PauseGame && Visible)
                Time.timeScale = 0;
            else if (PauseGame && !Visible)
                Time.timeScale = 1;
        }
    }
}
