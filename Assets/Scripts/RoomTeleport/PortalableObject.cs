using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalableObject : MonoBehaviour
{

    public delegate void HasTeleportedHandler(Portal startPortal, Portal endPortal, Vector3 newPosition, Quaternion newRotation);
    public event HasTeleportedHandler HasTeleported;

    public void OnHasTeleported(Portal startPortal, Portal endPortal, Vector3 newPosition, Quaternion newRotation)
    {
        HasTeleported?.Invoke(startPortal, endPortal, newPosition, newRotation);
    }

}
