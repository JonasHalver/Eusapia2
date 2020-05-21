using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightHandler : MonoBehaviour
{
    public Transform lookatObj;
    public Transform fakeCamera;
    public float lookatZoom = 1;
    public Transform cameraChild;

    void OnValidate()
    {
        if (!lookatObj)
            lookatObj = transform.GetChild(0);
        if (!fakeCamera)
            fakeCamera = transform.GetChild(1);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(lookatObj.position, 1);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(fakeCamera.position, 1f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cameraChild = SmoothMouseLook.cameraControllerInstance.transform.GetChild(0);
            cameraChild.parent = null;
            SmoothMouseLook.cameraControllerInstance.lookatOverride = true;
            SmoothMouseLook.cameraControllerInstance.lookatTransform = lookatObj;
            SmoothMouseLook.cameraControllerInstance.lookatZoom = lookatZoom;
            SmoothMouseLook.cameraControllerInstance.fakeCamTransform = fakeCamera;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cameraChild.parent = SmoothMouseLook.cameraControllerInstance.transform;
            SmoothMouseLook.cameraControllerInstance.lookatTransform = null;
            SmoothMouseLook.cameraControllerInstance.lookatOverride = false;
            SmoothMouseLook.cameraControllerInstance.fakeCamTransform = null;
        }
    }
}
