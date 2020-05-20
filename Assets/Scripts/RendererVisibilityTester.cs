using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RendererVisibilityTester : MonoBehaviour
{
    public bool isVisible;
    private Renderer r;
    private Camera c;
    
    void Awake()
    {
        r = GetComponent<Renderer>();
        c = Camera.main;
    }

    void LateUpdate()
    {
        Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(c);
        isVisible = GeometryUtility.TestPlanesAABB(frustumPlanes, r.bounds);         
    }
}
