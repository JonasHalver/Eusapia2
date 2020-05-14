using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorCameraController : MonoBehaviour
{
    public Transform player;
    public Transform playerCam;
    public Transform mirror;
    public Transform linkedMirror;
    public Transform handler;

    public Vector3 playerToMirror;

    // Start is called before the first frame update
    void Start()
    {
        //player = PlayerController.instance.transform;
        playerCam = Camera.main.transform;
    }

    // Update is called once per frame
    public void Move()
    {
        transform.localRotation = Quaternion.Euler(
            new Vector3(playerCam.rotation.eulerAngles.x,
            playerCam.rotation.eulerAngles.y,
            playerCam.rotation.eulerAngles.z));

        playerToMirror = (playerCam.position - linkedMirror.position);
        transform.localPosition = playerToMirror;

        handler.rotation = Quaternion.Euler(0, -linkedMirror.localRotation.eulerAngles.y, 0);
    }
}
