using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PortalRender : MonoBehaviour
{

    public Camera portalCamera;
    public int maxRecursions = 2;

    public int debugTotalRenderCount;

    private Camera mainCamera;
    private Portal[] allPortals;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        allPortals = FindObjectsOfType<Portal>();
        //RenderPipelineManager.beginCameraRendering += RenderPipelineManager_StartCameraRendering;
        RenderPipelineManager.endCameraRendering += RenderPipelineManager_endCameraRendering;
    }

    private void RenderPipelineManager_StartCameraRendering(ScriptableRenderContext context, Camera camera)
    {
        OnPreRender();
    }

    private void RenderPipelineManager_endCameraRendering(ScriptableRenderContext context, Camera camera)
    {
        OnPostRender();
    }

    private void OnPreRender()
    {
        debugTotalRenderCount = 0;

        foreach (var portal in allPortals)
        {
            //portal.RenderViewthroughRecursive(
            //    mainCamera.transform.position,
            //    mainCamera.transform.rotation,
            //    out _,
            //    out _,
            //    out var renderCount,
            //    portalCamera,
            //    0,
            //    maxRecursions);

            //debugTotalRenderCount += renderCount;
        }
    }

    private void OnPostRender()
    {
        RenderTexturePool.Instance.ReleaseAllTextures();
    }

    // Update is called once per frame
    void Update()
    {
        OnPreRender();
    }
}
