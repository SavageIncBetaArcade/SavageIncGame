
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(UUID))]
public class Portal : MonoBehaviour, IDataPersistance
{
    #region members
    public int TargetPortalIndex;

    public Portal[] TargetPortal;

    public InteractionTrigger[] Triggers;

    public bool AffectedByBossAlert = true;

    public int TargetPortalOnTrigger;

    public Transform NormalVisible;

    public Transform NormalInvisible;

    public Renderer ViewthroughRenderer;

    public Texture ViewthroughDefaultTexture;

    public Portal[] VisiblePortals;

    public int MaxRecursionsOverride = -1;

    private Material viewthroughMaterial;

    private Camera mainCamera;

    private Vector4 vectorPlane;

    public float OffMeshLinkResolution = 0.2f;
    public Transform OffMeshLinkRef1;
    public Transform OffMeshLinkRef2;
    public int OffMeshLinkArea;
    private readonly List<PortalOffMeshLink> offMeshLinks = new List<PortalOffMeshLink>();
    private int previousTargetPortal = 0; //previous target portal before the trigger was set

    private struct PortalOffMeshLink
    {
        public Transform RefTransform;
    }

    private UUID uuid;
    #endregion

    #region members
    private void Awake()
    {
        // Generate OffMeshLinks

        var directionToRef2 = OffMeshLinkRef2.position - OffMeshLinkRef1.position;
        var distanceToGenerate = directionToRef2.magnitude;
        directionToRef2.Normalize();
        var roundedDistanceToGenerate = Math.Round(distanceToGenerate, 1);
        for (var currentDistance = 0f; currentDistance <= roundedDistanceToGenerate; currentDistance += OffMeshLinkResolution)
        {
            var newPosition = OffMeshLinkRef1.position + directionToRef2 * currentDistance;
            var newTransform = new GameObject("[AUTO] OffMeshLink Transform").transform;
            newTransform.parent = transform;
            newTransform.position = newPosition;

            offMeshLinks.Add(new PortalOffMeshLink()
            {
                RefTransform = newTransform
            });
        }

        foreach (var trigger in Triggers)
        {
            trigger.OnTrigger += checkTriggers;
        }

        uuid = GetComponent<UUID>();
    }

    private readonly HashSet<PortalableObject> objectsInPortal = new HashSet<PortalableObject>();
    private readonly HashSet<PortalableObject> objectsInPortalToRemove = new HashSet<PortalableObject>();

    public bool ShouldRender(Plane[] cameraPlanes) =>
    ViewthroughRenderer.isVisible &&
    GeometryUtility.TestPlanesAABB(cameraPlanes,
        ViewthroughRenderer.bounds);

    private struct VisiblePortalResources
    {
        public Portal VisiblePortal;
        public RenderTexturePool.PoolItem PoolItem;
        public Texture OriginalTexture;
    }

    private void OnDrawGizmos()
    {
        // Linked portals

        if (TargetPortal.Length > 0 && TargetPortal[TargetPortalIndex] != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, TargetPortal[TargetPortalIndex].transform.position);
        }

        // Visible portals
        Gizmos.color = Color.blue;
        foreach (var visiblePortal in VisiblePortals)
        {
            Gizmos.DrawLine(transform.position, visiblePortal.transform.position);
        }
    }

    public static Vector3 TransformPositionBetweenPortals(Portal sender, Portal target, Vector3 position)
    {
        return target == null ? new Vector3() :
            target.NormalInvisible.TransformPoint(
                sender.NormalVisible.InverseTransformPoint(position));
    }

    public static Quaternion TransformRotationBetweenPortals(Portal sender, Portal target, Quaternion rotation)
    {
        return target == null ? new Quaternion() :
            target.NormalInvisible.rotation *
            Quaternion.Inverse(sender.NormalVisible.rotation) *
            rotation;
    }

    public static Vector3 TransformDirectionBetweenPortals(Portal sender, Portal target, Vector3 position)
    {
        return target.NormalInvisible.TransformDirection(sender.NormalVisible.InverseTransformDirection(position));
    }

    private void Start()
    {
        // Finish OffMeshLink generation

        TargetPortalIndex = 0;

        if(TargetPortal.Length > 0 && TargetPortal[TargetPortalIndex] != null)
        {
            for (var i = 0; i < offMeshLinks.Count; i++)
            {
                var offMeshLink = offMeshLinks[i];

                var newLink = offMeshLink.RefTransform.gameObject.AddComponent<OffMeshLink>();
                newLink.startTransform = offMeshLink.RefTransform;
                newLink.endTransform = TargetPortal[TargetPortalIndex].offMeshLinks[TargetPortal[TargetPortalIndex].offMeshLinks.Count - 1 - i].RefTransform;
                newLink.biDirectional = false;
                newLink.costOverride = -1;
                newLink.autoUpdatePositions = false;
                newLink.activated = true;
                newLink.area = OffMeshLinkArea;
            }
        }

        viewthroughMaterial = ViewthroughRenderer.material;

        mainCamera = Camera.main;

        // Generate bounding plane

        var plane = new Plane(NormalVisible.forward, transform.position + NormalInvisible.forward * 0.01f);
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

    private static bool RaycastRecursiveInternal(
    Vector3 position,
    Vector3 direction,
    int maxRecursions,
    out RaycastHit hitInfo,
    float range,
    int currentRecursion,
    GameObject ignoreObject,
    int layerMask)
    {
        //Onjects to ignore when RayCasting 
        var ignoreObjectOriginalLayer = 0;
        if (ignoreObject)
        {
            ignoreObjectOriginalLayer = ignoreObject.layer;
            ignoreObject.layer = 2; // Ignore raycast
        }

        var raycastHitSomething = Physics.Raycast(
        position,
        direction,
        out var hit,
        range,
        layerMask); // Clamp to max array length

        // If no objects are hit, the recursion ends here, with no effect

        if (!raycastHitSomething)
        {
            hitInfo = new RaycastHit(); 
            return false;
        }

        // If the object hit is a portal, recurse, unless we are already at max recursions

        var portal = hit.collider.GetComponent<Portal>();
        if (portal)
        {
            Debug.Log("Hit Portal");

            if (currentRecursion >= maxRecursions)
            {
                hitInfo = new RaycastHit(); 
                return false;
            }

            // Just keep recusing

            return RaycastRecursiveInternal(
                TransformPositionBetweenPortals(portal, portal.TargetPortal[portal.TargetPortalIndex], hit.point),
                TransformDirectionBetweenPortals(portal, portal.TargetPortal[portal.TargetPortalIndex], direction),
                maxRecursions,
                out hitInfo,
                range,
                currentRecursion + 1,
                portal.TargetPortal[portal.TargetPortalIndex].gameObject,
                layerMask);
        }

        // If the object hit is not a portal, then congrats! We stop here and report back that we hit something.
        hitInfo = hit;
        return true;
    }


    public static bool RaycastRecursive(
    Vector3 position,
    Vector3 direction,
    int maxRecursions,
    out RaycastHit hitInfo,
    float range,
    int layerMask = ~0)
    {
        return RaycastRecursiveInternal(position,
            direction,
            maxRecursions,
            out hitInfo,
            range,
            0,
            null,
            layerMask);
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

        var virtualPosition = TransformPositionBetweenPortals(this, TargetPortal[TargetPortalIndex], refPosition);
        var virtualRotation = TransformRotationBetweenPortals(this, TargetPortal[TargetPortalIndex], refRotation);


        portalCamera.transform.SetPositionAndRotation(virtualPosition, virtualRotation);

        // Convert target portal's plane to camera space (relative to target camera)

        var targetViewThroughPlaneCameraSpace =
            Matrix4x4.Transpose(Matrix4x4.Inverse(portalCamera.worldToCameraMatrix))
            * (TargetPortal != null ? TargetPortal[TargetPortalIndex].vectorPlane : new Vector4());

        // Set portal camera projection matrix to clip walls between target portal and target camera
        // Inherits main camera near/far clip plane and FOV settings

        var obliqueProjectionMatrix = mainCamera.CalculateObliqueMatrix(targetViewThroughPlaneCameraSpace);
        portalCamera.projectionMatrix = obliqueProjectionMatrix;

        // Store visible portal resources to release and reset 

        var visiblePortalResourcesList = new List<VisiblePortalResources>();

        var cameraPlanes = GeometryUtility.CalculateFrustumPlanes(portalCamera);

        var actualMaxRecursions = TargetPortal[TargetPortalIndex].MaxRecursionsOverride >= 0
        ? TargetPortal[TargetPortalIndex].MaxRecursionsOverride
        : maxRecursions;

        if (currentRecursion < actualMaxRecursions)
        {
            foreach (var visiblePortal in TargetPortal[TargetPortalIndex].VisiblePortals)
            {
                //only render for portals which are visible
                if (!visiblePortal.ShouldRender(cameraPlanes)) continue;

                if(TargetPortal != null)
                {
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
        }
        else
        {
            foreach (var visiblePortal in TargetPortal[TargetPortalIndex].VisiblePortals)
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
        viewthroughMaterial.mainTexture = ViewthroughDefaultTexture;
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
            var pivotToNormalDotProduct = Vector3.Dot(directionToPivotFromTransform, NormalVisible.forward);
            if (pivotToNormalDotProduct > 0) continue;

            // Warp object

            var newPosition = TransformPositionBetweenPortals(this, TargetPortal[TargetPortalIndex], portalableObject.transform.position);
            var newRotation = TransformRotationBetweenPortals(this, TargetPortal[TargetPortalIndex], portalableObject.transform.rotation);
            portalableObject.transform.SetPositionAndRotation(newPosition, newRotation);
            portalableObject.OnHasTeleported(this, TargetPortal[TargetPortalIndex], newPosition, newRotation);

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
        foreach (var trigger in Triggers)
        {
            trigger.OnTrigger -= checkTriggers;
        }

        // Destroy cloned material 
        Destroy(viewthroughMaterial);
        
    }

    private void checkTriggers(bool triggered, InteractionTrigger trigger)
    {
        if(InteractionTrigger.AllTrue(Triggers))
        {
            previousTargetPortal = TargetPortalIndex;
            TargetPortalIndex = TargetPortalOnTrigger;
        }
        else
        {
            TargetPortalIndex = previousTargetPortal;
        }
    }
    #endregion

    #region
    public void Save(DataContext context)
    {
        if (!uuid)
            return;

        context.SaveData(uuid.ID, "TargetPortalIndex", TargetPortalIndex);
        context.SaveData(uuid.ID, "previousTargetPortal", previousTargetPortal);
    }

    public void Load(DataContext context, bool destroyUnloaded = false)
    {
        if (!uuid)
            return;

        TargetPortalIndex = context.GetValue<int>(uuid.ID, "TargetPortalIndex");
        previousTargetPortal = context.GetValue<int>(uuid.ID, "previousTargetPortal");
    }
    #endregion
}