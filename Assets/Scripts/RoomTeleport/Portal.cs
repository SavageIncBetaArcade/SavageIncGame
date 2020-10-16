﻿
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal targetPortal;

    public Transform normalVisible;
    public Transform normalInvisible;

    public Camera portalCamera;
    public Renderer viewthroughRenderer;
    private RenderTexture viewthroughRenderTexture;
    private Material viewthroughMaterial;

    private Camera mainCamera;

    private Vector4 vectorPlane;

    private HashSet<PortalableObject> objectsInPortal = new HashSet<PortalableObject>();
    private HashSet<PortalableObject> objectsInPortalToRemove = new HashSet<PortalableObject>();

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
        // Create render texture

        viewthroughRenderTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.DefaultHDR);
        viewthroughRenderTexture.Create();

        // Assign render texture to portal material (cloned)

        viewthroughMaterial = viewthroughRenderer.material;
        viewthroughMaterial.mainTexture = viewthroughRenderTexture;

        // Assign render texture to portal camera

        portalCamera.targetTexture = viewthroughRenderTexture;

        // Cache the main camera

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
                // Catch exceptions so our loop doesn't die whenever there is an error
                Debug.LogException(e);
            }
        }
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

    private void LateUpdate()
    {
        // Calculate portal camera position and rotation

        var virtualPosition = TransformPositionBetweenPortals(this, targetPortal, mainCamera.transform.position);
        var virtualRotation = TransformRotationBetweenPortals(this, targetPortal, mainCamera.transform.rotation);

        // Position camera

        portalCamera.transform.SetPositionAndRotation(virtualPosition, virtualRotation);

        // Calculate projection matrix

        var clipThroughSpace =
            Matrix4x4.Transpose(Matrix4x4.Inverse(portalCamera.worldToCameraMatrix))
            * targetPortal.vectorPlane;

        // Set portal camera projection matrix to clip walls between target portal and portal camera
        // Inherits main camera near/far clip plane and FOV settings

        var obliqueProjectionMatrix = mainCamera.CalculateObliqueMatrix(clipThroughSpace);
        portalCamera.projectionMatrix = obliqueProjectionMatrix;
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
        // Release render texture from GPU

        viewthroughRenderTexture.Release();

        // Destroy cloned material and render texture

        Destroy(viewthroughMaterial);
        Destroy(viewthroughRenderTexture);
    }
}