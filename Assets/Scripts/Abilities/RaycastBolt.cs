
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(LineRenderer))]
public class RaycastBolt : MonoBehaviour
{
    public Material Material;
    public float TimeToTarget = 0.1f; //How long it takes the bolt to reach the target in seconds
    public float LifeTime = 0.05f; //The lifetime of the bolt after hit, gets destroyed after set time in seconds (< 0 never gets destroyed)

    private LineRenderer lineRenderer;
    private Vector3 startPosition;
    private Vector3 target;

    private Transform startTransform;
    private Transform endTransform;
    private float time = 0.0f;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.shadowCastingMode = ShadowCastingMode.Off;
        lineRenderer.material = Material;
        lineRenderer.enabled = false;
    }

    void Update()
    {
        time += Time.deltaTime / TimeToTarget;

        //if(!startTransform | !endTransform)
        //    Destroy(gameObject);

        if (startTransform != null && endTransform != null)
        {
            Vector3 end = Vector3.Lerp(startTransform.position, endTransform.position, time);

            lineRenderer.SetPosition(0, startTransform.position);
            lineRenderer.SetPosition(1, end);
        }
        else
        {
            Vector3 end = Vector3.Lerp(startPosition, target, time);

            lineRenderer.SetPosition(0, startPosition);
            lineRenderer.SetPosition(1, end);
        }
    }

    public void SetPoints(Vector3 start, Vector3 end)
    {
        time = 0;
        startPosition = start;
        target = end;
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, startPosition);
        lineRenderer.enabled = true;

        SetDestroy();
    }

    public void SetPoints(Transform start, Transform end)
    {
        time = 0;
        startTransform = start;
        endTransform = end;
        lineRenderer.SetPosition(0, startTransform.position);
        lineRenderer.SetPosition(1, startTransform.position);
        lineRenderer.enabled = true;

        SetDestroy();
    }

    public void Fire(Transform start, Vector3 forward, float distance)
    {
        time = 0;
        startPosition = start.position;
        target = start.position + (forward * distance);
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, startPosition);
        lineRenderer.enabled = true;

        SetDestroy();
    }

    private void SetDestroy()
    {
        if (LifeTime >= 0.0f)
        {
            Destroy(gameObject, TimeToTarget + LifeTime);
        }
    }
}
