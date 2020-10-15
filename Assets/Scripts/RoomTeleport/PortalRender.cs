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
    private PortalOcclusionVolume[] occlusionVolumes;
    private bool onPreRender;
    private bool onPostRender;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        occlusionVolumes = FindObjectsOfType<PortalOcclusionVolume>();
        RenderPipelineManager.beginFrameRendering += RenderPipelineManager_StartCameraRendering;
        RenderPipelineManager.endFrameRendering += RenderPipelineManager_endCameraRendering;
    }

    private void RenderPipelineManager_StartCameraRendering(ScriptableRenderContext context, Camera[] camera)
    {
        onPreRender = true;
    }

    private void RenderPipelineManager_endCameraRendering(ScriptableRenderContext context, Camera[] camera)
    {
        onPostRender = true;
    }

    private void OnPreRender()
    {
        debugTotalRenderCount = 0;

        PortalOcclusionVolume currentOcclusionVolume = null;
        foreach (var occlusionVolume in occlusionVolumes)
        {
            if (occlusionVolume.collider.bounds.Contains(mainCamera.transform.position))
            {
                currentOcclusionVolume = occlusionVolume;
                break;
            }
        }

        if (currentOcclusionVolume != null)
        {
            var cameraPlanes = GeometryUtility.CalculateFrustumPlanes(mainCamera);

            foreach (var portal in currentOcclusionVolume.portals)
            {
                if (!portal.ShouldRender(cameraPlanes)) continue;
                
                portal.RenderViewthroughRecursive(
                    mainCamera.transform.position,
                    mainCamera.transform.rotation,
                    out _,
                    out _,
                    out var renderCount,
                    portalCamera,
                    0,
                    maxRecursions);

                debugTotalRenderCount += renderCount;
            }
        }
    }

    private void OnPostRender()
    {
        RenderTexturePool.Instance.ReleaseAllTextures();
    }

    private void LateUpdate()
    {
        if(onPreRender)
        {
            OnPreRender();
            onPreRender = false;
        }

        if(onPostRender)
        {
            OnPostRender();
            onPostRender = false;
        }
        
    }


}
