using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    public static List<Mirror> mirrors = new List<Mirror>();
    public int mirrorIndex;
    public Mirror linkedMirror;
    public MeshRenderer screen;
    public Camera playerCam;
    public Camera mirrorCam;
    private RenderTexture viewTexture;
    private MirrorCameraController c;
    Vector3 playerToMirror;
    public Transform handler;

    void Awake()
    { 
        if (!mirrors.Contains(this))
        {
            mirrors.Add(this);
        }

        screen = GetComponent<MeshRenderer>();

        if (!playerCam)
            playerCam = Camera.main;

        c = mirrorCam.GetComponent<MirrorCameraController>();
    }

    void Start()
    {
        for (int i = 0; i < mirrors.Count; i++)
        {
            if (mirrors[i] != this && mirrors[i].mirrorIndex == mirrorIndex)
            {
                linkedMirror = mirrors[i];
                break;
            }
        }
    }

    void Update()
    {
        Render();
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

    public void Move()
    {
        mirrorCam.transform.localRotation = Quaternion.Euler(
            new Vector3(playerCam.transform.rotation.eulerAngles.x,
            playerCam.transform.rotation.eulerAngles.y,
            playerCam.transform.rotation.eulerAngles.z));

        playerToMirror = (playerCam.transform.position - linkedMirror.transform.position);
        mirrorCam.transform.localPosition = playerToMirror;

        handler.rotation = Quaternion.Euler(0, -linkedMirror.transform.localRotation.eulerAngles.y, 0);
    }

    public void Render()
    {
        screen.enabled = false;
        CreateViewTexture();

        mirrorCam.projectionMatrix = playerCam.projectionMatrix;

        //var m = transform.localToWorldMatrix * linkedMirror.transform.worldToLocalMatrix * playerCam.transform.localToWorldMatrix;
        //c.Move();
        Move();
        //mirrorCam.transform.SetPositionAndRotation(m.GetColumn(3), m.rotation);

        mirrorCam.Render();
        screen.enabled = true;
        linkedMirror.screen.material.SetInt("displayMask", 1);
    }
}
