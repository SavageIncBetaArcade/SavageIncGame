using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PortalOcclusionVolume : MonoBehaviour
{
    public Portal[] Portals;
    private Collider collider;

    public Collider Collider => collider;

    void Awake()
    {
        if (collider == null)
        {
            collider = GetComponent<Collider>();
        }
    }


    [ContextMenu("Auto Add Portals")]
    private void EditorAutoAdd()
    {
        var col = GetComponent<Collider>();
        var allPortals = FindObjectsOfType<Portal>();
        var portalBuilder = new List<Portal>();

        foreach (var portal in allPortals)
        {
            if (col.bounds.Contains(portal.transform.position))
            {
                portalBuilder.Add(portal);
            }
        }

        Portals = portalBuilder.ToArray();
    }


    public bool IsPlayerInRoom()
    {
        var col = GetComponent<Collider>();
        var player = FindObjectOfType<PlayerBase>();

        if(col.bounds.Contains(player.transform.position))
        {
            return true;
        }

        return false;
    }
}
