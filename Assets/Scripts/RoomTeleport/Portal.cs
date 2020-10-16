
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal targetPortal;

    public Transform normalVisible;

    public Transform normalInvisible;

    public Renderer viewthroughRenderer;

    public Texture viewthroughDefaultTexture;

    public Portal[] visiblePortals;

    public int maxRecursionsOverride = -1;

    private Material viewthroughMaterial;

    private Camera mainCamera;

    private Vector4 vectorPlane;

    private HashSet<PortalableObject> objectsInPortal = new HashSet<PortalableObject>();
    private HashSet<PortalableObject> objectsInPortalToRemove = new HashSet<PortalableObject>();

    public bool ShouldRender(Plane[] cameraPlanes) =>
    viewthroughRenderer.isVisible &&
    GeometryUtility.TestPlanesAABB(cameraPlanes,
        viewthroughRenderer.bounds);

    private struct VisiblePortalResources
    {
        public Portal VisiblePortal;
        public RenderTexturePool.PoolItem PoolItem;
        public Texture OriginalTexture;
    }

    private void OnDrawGizmos()
    {
        // Linked portals

        if (targetPortal != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, targetPortal.transform.position);
        }

        // Visible portals
        Gizmos.color = Color.blue;
        foreach (var visiblePortal in visiblePortals)
        {
            Gizmos.DrawLine(transform.position, visiblePortal.transform.position);
        }
    }

    public static Vector3 TransformPositionBetweenPortals(Portal sender, Portal target, Vector3 position)
    {
        return
            target.normalInvisible.TransformPoint(
                sender.normalVisible.InverseTransformPoint(position));
    }

    public static Quaternion TransformRotationBetweenPortals(Portal sender, Portal target, Quaternion rotation)
    {
        return
            target.normalInvisible.rotation *
            Quaternion.Inverse(sender.normalVisible.rotation) *
            rotation;
    }

    private void Start()
    {
        viewthroughMaterial = viewthroughRenderer.material;

        mainCamera = Camera.main;

        // Generate bounding plane

        var plane = new Plane(normalVisible.forward, transform.position + normalInvisible.forward * 0.01f);
        vectorPlane = new Vector4(plane.normal.x, plane.normal.y, plane.normal.z, plane.distance);

        StartCoroutine(WaitForFixedUpdateLoop());
    }

    private IEnumerator WaitForFixedUpdateLoop()
    {
        var waitForFixedUpdate = new WaitForFixedUpdate();
        while (true)
        {
            yield return waitForFixedUpdate;
            try
            {
                CheckForPortalCrossing();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }


    public void RenderViewthroughRecursive(
    Vector3 refPosition,
    Quaternion refRotation,
    out RenderTexturePool.PoolItem temporaryPoolItem,
    out Texture originalTexture,
    out int debugRenderCount,
    Camera portalCamera,
    int currentRecursion,
    int maxRecursions)
    {
        debugRenderCount = 1;

        // Calculate virtual camera position and rotation

        var virtualPosition = TransformPositionBetweenPortals(this, targetPortal, refPosition);
        var virtualRotation = TransformRotationBetweenPortals(this, targetPortal, refRotation);


        portalCamera.transform.SetPositionAndRotation(virtualPosition, virtualRotation);

        // Convert target portal's plane to camera space (relative to target camera)

        var targetViewThroughPlaneCameraSpace =
            Matrix4x4.Transpose(Matrix4x4.Inverse(portalCamera.worldToCameraMatrix))
            * targetPortal.vectorPlane;

        // Set portal camera projection matrix to clip walls between target portal and target camera
        // Inherits main camera near/far clip plane and FOV settings

        var obliqueProjectionMatrix = mainCamera.CalculateObliqueMatrix(targetViewThroughPlaneCameraSpace);
        portalCamera.projectionMatrix = obliqueProjectionMatrix;

        // Store visible portal resources to release and reset 

        var visiblePortalResourcesList = new List<VisiblePortalResources>();

        var cameraPlanes = GeometryUtility.CalculateFrustumPlanes(portalCamera);

        var actualMaxRecursions = targetPortal.maxRecursionsOverride >= 0
        ? targetPortal.maxRecursionsOverride
        : maxRecursions;

        if (currentRecursion < actualMaxRecursions)
        {
            foreach (var visiblePortal in targetPortal.visiblePortals)
            {
                //only render for portals which are visible
                if (!visiblePortal.ShouldRender(cameraPlanes)) continue;

                visiblePortal.RenderViewthroughRecursive(
                    virtualPosition,
                    virtualRotation,
                    out var visiblePortalTemporaryPoolItem,
                    out var visiblePortalOriginalTexture,
                    out var visiblePortalRenderCount,
                    portalCamera,
                    currentRecursion + 1,
                    maxRecursions);

                visiblePortalResourcesList.Add(new VisiblePortalResources()
                {
                    OriginalTexture = visiblePortalOriginalTexture,
                    PoolItem = visiblePortalTemporaryPoolItem,
                    VisiblePortal = visiblePortal
                });

                debugRenderCount += visiblePortalRenderCount;
            }
        }
        else
        {
            foreach (var visiblePortal in targetPortal.visiblePortals)
            {
                visiblePortal.ShowViewthroughDefaultTexture(out var visiblePortalOriginalTexture);

                visiblePortalResourcesList.Add(new VisiblePortalResources()
                {
                    OriginalTexture = visiblePortalOriginalTexture,
                    VisiblePortal = visiblePortal
                });
            }
        }

        temporaryPoolItem = RenderTexturePool.Instance.GetTexture();

        // Use portal camera

        portalCamera.targetTexture = temporaryPoolItem.Texture;
        portalCamera.transform.SetPositionAndRotation(virtualPosition, virtualRotation);
        portalCamera.projectionMatrix = obliqueProjectionMatrix;

        // Render portal camera to target texture

        portalCamera.Render();

        // Reset and release

        foreach (var resources in visiblePortalResourcesList)
        {
            // Reset to original texture
            // So that it will remain correct if the visible portal is still expecting to be rendered
            // on another camera but has already rendered its texture. Originally the texture may be overriden by other renders.

            resources.VisiblePortal.viewthroughMaterial.mainTexture = resources.OriginalTexture;

            // Release temp render texture

            if (resources.PoolItem != null)
            {
                RenderTexturePool.Instance.ReleaseTexture(resources.PoolItem);
            }
        }

        // Must be after camera render, in case it renders itself (in which the texture must not be replaced before rendering itself)
        // Must be after restore, in case it restores its own old texture (in which the new texture must take precedence)

        originalTexture = viewthroughMaterial.mainTexture;
        viewthroughMaterial.mainTexture = temporaryPoolItem.Texture;

    }

    private void ShowViewthroughDefaultTexture(out Texture originalTexture)
    {
        originalTexture = viewthroughMaterial.mainTexture;
        viewthroughMaterial.mainTexture = viewthroughDefaultTexture;
    }

    private void CheckForPortalCrossing()
    {
        // Clear removal queue

        objectsInPortalToRemove.Clear();

        // Check every touching object

        foreach (var portalableObject in objectsInPortal)
        {
            // If portalable object has been destroyed, remove it immediately

            if (portalableObject == null)
            {
                objectsInPortalToRemove.Add(portalableObject);
                continue;
            }

            // Check if portalable object is behind the portal using Vector3.Dot (dot product)
            // If so, they have crossed through the portal.

            var pivot = portalableObject.transform;
            var directionToPivotFromTransform = pivot.position - transform.position;
            directionToPivotFromTransform.Normalize();
            var pivotToNormalDotProduct = Vector3.Dot(directionToPivotFromTransform, normalVisible.forward);
            if (pivotToNormalDotProduct > 0) continue;

            // Warp object

            var newPosition = TransformPositionBetweenPortals(this, targetPortal, portalableObject.transform.position);
            var newRotation = TransformRotationBetweenPortals(this, targetPortal, portalableObject.transform.rotation);
            portalableObject.transform.SetPositionAndRotation(newPosition, newRotation);
            portalableObject.OnHasTeleported(this, targetPortal, newPosition, newRotation);

            // Object is no longer touching this side of the portal

            objectsInPortalToRemove.Add(portalableObject);
        }

        // Remove all objects queued up for removal

        foreach (var portalableObject in objectsInPortalToRemove)
        {
            objectsInPortal.Remove(portalableObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var portalableObject = other.GetComponent<PortalableObject>();
        if (portalableObject)
        {
            objectsInPortal.Add(portalableObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var portalableObject = other.GetComponent<PortalableObject>();
        if (portalableObject)
        {
            objectsInPortal.Remove(portalableObject);
        }
    }

    private void OnDestroy()
    {
        // Destroy cloned material 
        Destroy(viewthroughMaterial);
        
    }
}