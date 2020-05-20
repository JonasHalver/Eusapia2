using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RendererVisibilityTester : MonoBehaviour
{
    public bool isVisible;
    private Renderer r;
    private Camera c;
    Animator anim;
    void Awake()
    {
        r = GetComponent<Renderer>();
        c = Camera.main;

        anim = GetComponentInParent<Animator>();
    }

    private void Update()
    {
        if (isVisible)
        {
            anim.speed = 0;
        }
        else
        {
            anim.speed = 1;
        }
    }

    void LateUpdate()
    {
        Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(c);
        isVisible = GeometryUtility.TestPlanesAABB(frustumPlanes, r.bounds);         
    }
}
