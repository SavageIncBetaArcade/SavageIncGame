using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;

public class PortalRender : MonoBehaviour
{
    public Camera PortalCamera;
    public int MaxRecursions = 2;

    public int DebugTotalRenderCount;

    private Camera mainCamera;
    private PortalOcclusionVolume[] occlusionVolumes;
    private bool onPreRender;
    private bool onPostRender;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        occlusionVolumes = FindObjectsOfType<PortalOcclusionVolume>();

        //sets up callback for hdrp camera rendering
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
        DebugTotalRenderCount = 0;

        PortalOcclusionVolume currentOcclusionVolume = null;
        foreach (var occlusionVolume in occlusionVolumes)
        {
            if (occlusionVolume.Collider.bounds.Contains(mainCamera.transform.position))
            {
                currentOcclusionVolume = occlusionVolume;
                break;
            }
        }

        if (currentOcclusionVolume != null)
        {
            var cameraPlanes = GeometryUtility.CalculateFrustumPlanes(mainCamera);

            foreach (var portal in currentOcclusionVolume.Portals)
            {
                if (!portal.ShouldRender(cameraPlanes)) continue;

                if(portal.TargetPortal[portal.TargetPortalIndex] != null)
                {
                    portal.RenderViewthroughRecursive(
                    mainCamera.transform.position,
                    mainCamera.transform.rotation,
                    out _,
                    out _,
                    out var renderCount,
                    PortalCamera,
                    0,
                    MaxRecursions);

                    DebugTotalRenderCount += renderCount;
                }
                
            }
        }
    }

    private void OnPostRender()
    {
        RenderTexturePool.Instance.ReleaseAllTextures();
    }

    private void LateUpdate()
    {
        Profiler.BeginSample("Portal Profile");
        //have to use the booleans as trying to run pre and post render inside the callback function causes an error
        //this is because hdrp does not support recursive rendering
        if (onPreRender)
        {
            OnPreRender();
            onPreRender = false;
        }

        if(onPostRender)
        {
            OnPostRender();
            onPostRender = false;
        }
        Profiler.EndSample();
    }


}
