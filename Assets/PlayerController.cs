using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static GameObject player;
    public Camera playerCam;
    Rigidbody rb;
    public float movementSpeed = 2;
    public float sprintModifier = 1.5f;

    public float turnRate = 2f;

    public bool canMove = true;
    public bool isMirrored = false;
    public Vector3 camForward;

    public Vector3 PlayerInput
    {
        get { return new Vector3(Input.GetAxis("Horizontal") * (isMirrored ? -1 : 1), 0, Input.GetAxis("Vertical")); }
    }

    void Awake()
    {
        player = gameObject;
        rb = GetComponent<Rigidbody>();
        playerCam = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canMove)
        {
            //rb.position += new Vector3(PlayerInput.x * SmoothMouseLook.camForward.x, PlayerInput.y * SmoothMouseLook.camForward.y, PlayerInput.z * SmoothMouseLook.camForward.z) * movementSpeed * (Input.GetButton("Sprint") ? sprintModifier : 1) * Time.deltaTime;
            rb.position += camForward * PlayerInput.z * movementSpeed * (Input.GetButton("Sprint") ? sprintModifier : 1) * Time.deltaTime;
            rb.position += Vector3.Cross(transform.up, camForward) * PlayerInput.x * movementSpeed * Time.deltaTime;
            Vector3 lookLerp = Vector3.Lerp(transform.forward + rb.position, rb.position + camForward * PlayerInput.z, turnRate * Time.deltaTime);

            if (PlayerInput != Vector3.zero)
                transform.LookAt(rb.position + camForward * (isMirrored ? -1 : 1) * PlayerInput.z);
        }
    }
}
