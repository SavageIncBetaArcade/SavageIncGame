using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class RaycastInteractionTrigger : InteractionTrigger
{
    public float Range = 2.0f;

    private Camera playerCamera;
    private BoxCollider boxCollider;

   

    protected override void Awake()
    {
        base.Awake();

        playerCamera = FindObjectOfType<PlayerCamera>()?.GetComponent<Camera>();
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;
    }

    void OnDisable()
    {
        ShowPopupText(false);
    }

    void Update()
    {
        if (!playerCamera)
            return;

        ShowPopupText(false);

        //raycast from center of camera and see if it hits this object
        RaycastHit hitInfo;

        if(Vector3.Distance(playerCamera.transform.position,transform.position) > Range)
            return;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, Range))
        {
            if(hitInfo.collider.gameObject != gameObject)
                return;

            ShowPopupText(true);
            if (Input.GetButtonUp("Interact"))
            {
                Interact();
            }
                
        }

    }
}
