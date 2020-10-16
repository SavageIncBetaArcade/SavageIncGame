﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalOcclusionVolume : MonoBehaviour
{

    public new Collider collider;
    public Portal[] portals;

    // Start is called before the first frame update
    void Start()
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

        portals = portalBuilder.ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
