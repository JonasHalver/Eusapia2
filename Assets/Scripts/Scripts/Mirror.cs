using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    public Mirror linkedMirror;
    public MeshRenderer screen;
    public Camera playerCam;
    public Camera mirrorCam;
    private RenderTexture viewTexture;
    private MirrorCameraController c;

    void Awake()
    {
        if (!playerCam)
            playerCam = Camera.main;

        c = mirrorCam.GetComponent<MirrorCameraController>();
    }

    void CreateViewTexture()
    {
        if (viewTexture == null || viewTexture.width != Screen.width || viewTexture.height != Screen.height)
        {
            if (viewTexture != null)
            {
                viewTexture.Release();
            }
            viewTexture = new RenderTexture(Screen.width, Screen.height, 0);

            mirrorCam.targetTexture = viewTexture;

            linkedMirror.screen.material.SetTexture("_MainTex", viewTexture);
        }
    }

    void OnPreCull()
    {
        Render();
    }

    public void Render()
    {
        screen.enabled = false;
        CreateViewTexture();

        mirrorCam.projectionMatrix = playerCam.projectionMatrix;

        //var m = transform.localToWorldMatrix * linkedMirror.transform.worldToLocalMatrix * playerCam.transform.localToWorldMatrix;
        c.Move();
        //mirrorCam.transform.SetPositionAndRotation(m.GetColumn(3), m.rotation);

        mirrorCam.Render();
        screen.enabled = true;
        linkedMirror.screen.material.SetInt("displayMask", 1);
    }
}
