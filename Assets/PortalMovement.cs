using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalMovement : MonoBehaviour
{
    public AnimationCurve curve;
    PlayerController pc;
    float t = 0;
    bool moving = false;
    public float speed;
    float modifiedSpeed;
    bool startedMove;
    public Quaternion rotHolder;


    // Start is called before the first frame update
    void Start()
    {
        pc = PlayerController.player.GetComponent<PlayerController>();
        Mirror.portal1.OnTeleport += TeleportSuccess;
        Mirror.portal2.OnTeleport += TeleportSuccess;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            moving = true;
            t = 0;
        }

        if (moving)
            Move();
    }

    public void Move()
    {
        if (!startedMove)
        {
            startedMove = true;
            rotHolder = pc.transform.rotation;
        }
        modifiedSpeed = curve.Evaluate(t) * speed;
        pc.canMove = false;
        PlayerController.player.GetComponent<Collider>().enabled = false;
        PlayerController.player.GetComponent<Rigidbody>().isKinematic = true;

        float angleToRotate = 180 * (Time.deltaTime * speed);

        pc.transform.RotateAround(-pc.transform.up + pc.transform.position, pc.transform.right, angleToRotate);

        t += speed * Time.deltaTime;
        if (t >= 1)
        {
            moving = false; pc.canMove = true;
            PlayerController.player.GetComponent<Collider>().enabled = true;
            PlayerController.player.GetComponent<Rigidbody>().isKinematic = false;
            startedMove = false;
        }
    }

    public void TeleportSuccess()
    {
        pc.transform.Rotate(new Vector3(0, 180, 180));
    }
}
