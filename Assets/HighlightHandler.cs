using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightHandler : MonoBehaviour
{
    public Transform lookatObj;

    void OnValidate()
    {
        if (!lookatObj)
            lookatObj = transform.GetChild(0);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(lookatObj.position, 1);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SmoothMouseLook.cameraControllerInstance.lookatOverride = true;
            SmoothMouseLook.cameraControllerInstance.lookatTransform = lookatObj;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SmoothMouseLook.cameraControllerInstance.lookatTransform = null;
            SmoothMouseLook.cameraControllerInstance.lookatOverride = false;
        }
    }
}
