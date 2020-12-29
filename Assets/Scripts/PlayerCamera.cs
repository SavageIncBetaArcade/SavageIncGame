using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float LookSensitivity = 200.0f;
    public Transform PlayerBody;

    private float xRotation = 0f;
    private CharacterBase playerCharacterBase;

    Vector3 originalPos;
    private Coroutine continousShakeCoroutine;

    void Awake()
    {
        if (PlayerBody != null)
            playerCharacterBase = PlayerBody.GetComponent<CharacterBase>();

        originalPos = transform.localPosition;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;


    }

    void Update()
    {
        //If the player is stunned then stop player from looking around
        if (playerCharacterBase != null && playerCharacterBase.IsStunned)
            return;

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

    public void ShakePosition(float duration, Vector2 magnitude, float minDeviation, float maxDeviation, float roughness = 1.0f)
    {
        if (continousShakeCoroutine != null)
        {
            StopCoroutine(continousShakeCoroutine);
            transform.localPosition = originalPos;
        }

        continousShakeCoroutine = StartCoroutine(shakePosition(duration,magnitude,minDeviation,maxDeviation,roughness));
    }

    private IEnumerator shakePosition(float duration, Vector2 magnitude, float minDeviation, float maxDeviation, float roughness)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(minDeviation, maxDeviation) * (magnitude.x);
            float y = Random.Range(minDeviation, maxDeviation) * (magnitude.y);

            transform.localPosition = originalPos + Vector3.MoveTowards(Vector3.zero, new Vector3(x, y, originalPos.z), roughness * Time.deltaTime);

            elapsed += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        transform.localPosition = originalPos;
    }

    public IEnumerator ShakeRotation(float duration, Vector3 magnitude, float minDeviation, float maxDeviation)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float y = Random.Range(minDeviation, maxDeviation) * (magnitude.y / 100);
            float z = Random.Range(minDeviation, maxDeviation) * (magnitude.z / 100);

            transform.localRotation = new Quaternion(transform.localRotation.x, y, z, transform.localRotation.w);

            elapsed += Time.deltaTime;

            yield return null;
        }
    }
}
