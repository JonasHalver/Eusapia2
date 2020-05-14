using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MirrorCamFollow : MonoBehaviour
{
    public MirrorManController mirrorPlayer;
    public SmoothMouseLook otherCam;
    public Transform child;
    public Vector3 otherRot;
    public Vector3 adjustedRotation;
    public float angle;
    public float newY;
    void LateUpdate()
    {
        otherRot = otherCam.transform.rotation.eulerAngles;
        Vector2 r2 = new Vector2(otherRot.x, otherRot.z);
        angle = otherRot.y;
        if (angle > 180)
        {
            newY = Mathf.Abs((angle-90) - 90);
        }
        else if (angle < 180)
        {
            newY = Mathf.Abs((angle - 270) - 270);
        }
        else
        {
            newY = 0;
        }
        adjustedRotation = new Vector3(otherCam.transform.root.eulerAngles.x, -angle -180, 0);

        //adjustedRotation = new Vector3(otherCam.transform.rotation.eulerAngles.x - 180, otherCam.transform.rotation.eulerAngles.y - 180, otherCam.transform.rotation.eulerAngles.z - 180);
        //Quaternion newRot = Quaternion.LookRotation(-otherCam.transform.forward, otherCam.transform.up);
        transform.SetPositionAndRotation(mirrorPlayer.transform.position, Quaternion.Euler(adjustedRotation));
        Vector3 fakePoint = new Vector3(child.position.x, mirrorPlayer.transform.position.y, child.position.z);
        //mirrorPlayer.camForward = (mirrorPlayer.transform.position - fakePoint).normalized;
    }
}
