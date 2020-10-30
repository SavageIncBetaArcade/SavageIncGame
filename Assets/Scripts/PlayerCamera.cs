using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float LookSensitivity = 200.0f;
    public Transform PlayerBody;

    private float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Dead zone for controller
        if (mouseX > -0.1 && mouseX < 0.1)
        {
            mouseX = 0;
        }

        if (mouseY > -0.1 && mouseY < 0.1)
        {
            mouseY = 0;
        }

        mouseX = mouseX * LookSensitivity * Time.deltaTime;
        mouseY = mouseY * LookSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        PlayerBody.Rotate(Vector3.up * mouseX);
    }

    public IEnumerator ShakePosition(float duration, Vector2 magnitude, float minDeviation, float maxDeviation)
    {
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(minDeviation, maxDeviation) * (magnitude.x / 100);
            float y = Random.Range(minDeviation, maxDeviation) * (magnitude.y / 100);

            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }

    public IEnumerator ShakeRotation(float duration, Vector2 magnitude, float minDeviation, float maxDeviation)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float y = Random.Range(minDeviation, maxDeviation) * (magnitude.x / 100);
            float z = Random.Range(minDeviation, maxDeviation) * (magnitude.y / 100);

            transform.localRotation = new Quaternion(transform.localRotation.x, y, z, transform.localRotation.w);

            elapsed += Time.deltaTime;

            yield return null;
        }
    }
}
