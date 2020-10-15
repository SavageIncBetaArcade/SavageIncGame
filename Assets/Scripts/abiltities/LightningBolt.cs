
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(LineRenderer))]
public class LightningBolt : MonoBehaviour
{
    public Material LightningMaterial;
    public float TimeToTarget = 0.5f;

    private LineRenderer lineRenderer;
    private Vector3 startPosition;
    private Vector3 target;
    private float time = 0.0f;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.shadowCastingMode = ShadowCastingMode.Off;
        lineRenderer.material = LightningMaterial;
        lineRenderer.enabled = false;
    }

    void Update()
    {
        time += Time.deltaTime / TimeToTarget;
        Vector3 pos = Vector3.Lerp(startPosition, target, time);
        lineRenderer.SetPosition(1, pos);
    }

    public void SetPoints(Vector3 start, Vector3 end)
    {
        time = 0;
        startPosition = start;
        target = end;
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, startPosition);
        lineRenderer.enabled = true;
    }
}
