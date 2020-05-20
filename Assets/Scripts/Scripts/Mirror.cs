using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Mirror : MonoBehaviour
{
    public static List<Mirror> mirrors = new List<Mirror>();
    public int mirrorIndex;

    public static Mirror portal1, portal2;

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

    public event Action OnTeleport;
    public LayerMask mirrorMask;

    List<Renderer> hiddenRenderers = new List<Renderer>();

    void Awake()
    { 
        if (isPortal)
        {
            if (currentWorld == World.Living)
            {
                portal1 = this;
            }
            else
            {
                portal2 = this;
            }
        }
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

        if (currentWorld == World.Living)
        {
            player = PlayerController.player;
        }
        else
        {
            player = MirrorManController.mirrorMan;
        }
    }

    void Start()
    {
        for (int i = 0; i < mirrors.Count; i++)
        {
            if (mirrors[i] != this && mirrors[i].mirrorIndex == mirrorIndex)
            {
                linkedMirror = mirrors[i];
                if (currentWorld == World.Living)
                {
                    MirrorHandler.mirrorPairs.Add(this, linkedMirror);
                }                
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
            handler.localScale = new Vector3(1, 1, 1);
            handler.localRotation = Quaternion.Euler(90, 180, 0);
        }

        PortalMovement.instance.onTeleportSuccess += TeleportSuccess;
    }

    void LateUpdate()
    {
        if (isBlockingCamera || PlayerIsInSameWorld())
        {
            gameObject.layer = 10;
        }
        else
        {
            gameObject.layer = 9;
        }
        Render();
        if (!screen.enabled && !isBlockingCamera)
            screen.enabled = true;

        if (isBlockingCamera)
        {
            ConfirmCameraBlock();
        }
    }

    public void ConfirmCameraBlock()
    {
        RaycastHit hit1;
        Vector3 dir = (PlayerController.player.transform.position - playerCam.transform.position).normalized;
        float dst = Vector3.Distance(PlayerController.player.transform.position, playerCam.transform.position);
        if (Physics.Raycast(playerCam.transform.position, dir, out hit1, dst, mirrorMask))
        {
            Mirror m = hit1.collider.GetComponent<Mirror>();
            if (m && m == this)
            {
                isBlockingCamera = true;
            }
        }
        else
        {
            isBlockingCamera = false;
        }
    }

    public void HideObjectsBlockingCamera()
    {
        List<Renderer> nearbyRenderers = new List<Renderer>();
        Collider[] nearbyColliders = Physics.OverlapSphere(mirrorCam.transform.position, Vector3.Distance(mirrorCam.transform.position, transform.position));
        foreach(Collider c in nearbyColliders)
        {
            Renderer r = c.transform.GetComponent<Renderer>();
            if (r)
            {
                nearbyRenderers.Add(r);
            }
        }
        for(int i = 0; i < nearbyRenderers.Count; i++)
        {
            if (VisibleFromCamera(nearbyRenderers[i], mirrorCam) && nearbyRenderers[i].transform.CompareTag("Building"))
            {
                nearbyRenderers[i].gameObject.layer = 11;
                hiddenRenderers.Add(nearbyRenderers[i]);
            }
        }

        for (int i = 0; i < hiddenRenderers.Count; i++)
        {
            if (!nearbyRenderers.Contains(hiddenRenderers[i]) || !VisibleFromCamera(hiddenRenderers[i], mirrorCam))
            {
                hiddenRenderers[i].gameObject.layer = 0;
                hiddenRenderers.RemoveAt(i);
                i--;
            }
        }
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

    public bool PlayerIsInSameWorld()
    {
        PlayerController pc = PlayerController.player.GetComponent<PlayerController>();
        if (pc)
        {
            return pc.currentWorld == linkedMirror.currentWorld;
        }
        else
        {
            return false;
        }
    }

    void OnValidate()
    {
        //handler.rotation = Quaternion.Euler(-linkedMirror.handler.localRotation.eulerAngles.x, -linkedMirror.handler.localRotation.eulerAngles.y, 0);
    }

    public void Move()
    {
        if (!isPortal)
        {
            mirrorCam.transform.localRotation = Quaternion.Euler(
                new Vector3(playerCam.transform.rotation.eulerAngles.x,
                playerCam.transform.rotation.eulerAngles.y,
                playerCam.transform.rotation.eulerAngles.z));

            playerToMirror = (playerCam.transform.position - linkedMirror.transform.position);

            mirrorCam.transform.localPosition = playerToMirror;
        }
        else
        {
            Vector3 playerCamLookDir = playerCam.transform.forward;
            mirrorCam.transform.rotation = Quaternion.LookRotation(Vector3.Scale(playerCamLookDir, new Vector3(-1, -1, 1)), Vector3.down);
            playerToMirror = Vector3.Scale((playerCam.transform.position - linkedMirror.transform.position), new Vector3(-1,1,1));
            mirrorCam.transform.localPosition = Quaternion.AngleAxis(-90, linkedMirror.transform.right) * playerToMirror;
        }
    }

    public bool VisibleFromCamera(Renderer renderer, Camera camera)
    {
        Plane[] frustrumPlanes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(frustrumPlanes, renderer.bounds);
    }

    public void Render()
    {
        if (!VisibleFromCamera(linkedMirror.screen, playerCam) || !PlayerIsInSameWorld())
            return;

        screen.enabled = false;        

        CreateViewTexture();

        mirrorCam.projectionMatrix = playerCam.projectionMatrix;

        //var m = transform.localToWorldMatrix * linkedMirror.transform.worldToLocalMatrix * playerCam.transform.localToWorldMatrix;
        //c.Move();
        Move();
        //mirrorCam.transform.SetPositionAndRotation(m.GetColumn(3), m.rotation);

        //HideObjectsBlockingCamera();

        mirrorCam.Render();
        if (!isBlockingCamera)
            screen.enabled = true;

        linkedMirror.screen.material.SetInt("displayMask", 1);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TeleportTrigger") && canTeleport)
        {
            linkedMirror.canTeleport = false;
            canTeleport = false;
            Vector3 positionHolder1 = player.transform.parent.position;
            Vector3 positionHolder2 = player.transform.position;
            
            player.transform.parent.parent = player.transform.parent.parent == livingWorld ? deadWorld : livingWorld;
            player.transform.parent.position = linkedMirror.player.transform.parent.position;
            player.transform.position = linkedMirror.player.transform.position;

            if (player.GetComponent<PlayerController>())
                player.GetComponent<PlayerController>().currentWorld = player.GetComponent<PlayerController>().currentWorld == World.Living ? World.Dead : World.Living;
            else if (player.GetComponent<MirrorManController>())
                player.GetComponent<MirrorManController>().currentWorld = player.GetComponent<MirrorManController>().currentWorld == World.Dead ? World.Living : World.Dead;

            linkedMirror.player.transform.parent.parent = linkedMirror.player.transform.parent.parent == deadWorld ? livingWorld : deadWorld;
            linkedMirror.player.transform.parent.position = positionHolder1;
            linkedMirror.player.transform.position = positionHolder2;
            linkedMirror.isBlockingCamera = true;
            MirrorHandler.UsedPortal();
            OnTeleport?.Invoke();
        }
    }

    public void TeleportSuccess()
    {
        linkedMirror.canTeleport = true;
        canTeleport = true;
    }

    public void ReactivatePortal()
    {
        if (!canTeleport)
            canTeleport = true;
    }
}
