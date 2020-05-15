using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    public static List<Mirror> mirrors = new List<Mirror>();
    public int mirrorIndex;

    public enum World { Living, Dead }
    public World currentWorld = World.Living;

    public bool isPortal = false;
    public bool canTeleport = true;

    public GameObject player;

    public Mirror linkedMirror;
    public MeshRenderer screen;
    public Camera playerCam;
    public Camera mirrorCam;
    private RenderTexture viewTexture;
    private MirrorCameraController c;
    Vector3 playerToMirror;
    public Transform handler;
    public bool isBlockingCamera = false;

    Transform deadWorld, livingWorld;

    void Awake()
    { 
        if (!mirrors.Contains(this))
        {
            mirrors.Add(this);
        }

        deadWorld = GameObject.FindGameObjectWithTag("DeadWorld").transform;
        livingWorld = GameObject.FindGameObjectWithTag("LivingWorld").transform;

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

        if (isPortal)
        {
            switch (currentWorld)
            {
                case World.Living:
                    player = PlayerController.player;
                    break;
                case World.Dead:
                    player = MirrorManController.mirrorMan;
                    break;
            }
            GetComponent<BoxCollider>().isTrigger = true;
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

    public bool VisibleFromCamera(Renderer renderer, Camera camera)
    {
        Plane[] frustrumPlanes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(frustrumPlanes, renderer.bounds);
    }

    public void Render()
    {
        if (!VisibleFromCamera(linkedMirror.screen, playerCam))
            return;

        screen.enabled = false;        

        CreateViewTexture();

        mirrorCam.projectionMatrix = playerCam.projectionMatrix;

        //var m = transform.localToWorldMatrix * linkedMirror.transform.worldToLocalMatrix * playerCam.transform.localToWorldMatrix;
        //c.Move();
        Move();
        //mirrorCam.transform.SetPositionAndRotation(m.GetColumn(3), m.rotation);

        mirrorCam.Render();
        if (!isBlockingCamera)
            screen.enabled = true;

        linkedMirror.screen.material.SetInt("displayMask", 1);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TeleportTrigger"))
        {
            linkedMirror.canTeleport = false;
            Vector3 positionHolder1 = player.transform.parent.position;
            Vector3 positionHolder2 = player.transform.position;
            
            player.transform.parent.parent = player.transform.parent.parent == livingWorld ? deadWorld : livingWorld;
            player.transform.parent.position = linkedMirror.player.transform.parent.position;
            player.transform.position = linkedMirror.player.transform.position;

            linkedMirror.player.transform.parent.parent = linkedMirror.player.transform.parent.parent == deadWorld ? livingWorld : deadWorld;
            linkedMirror.player.transform.parent.position = positionHolder1;
            linkedMirror.player.transform.position = positionHolder2;
        }
    }

    public void ReactivatePortal()
    {
        if (!canTeleport)
            canTeleport = true;
    }
}
