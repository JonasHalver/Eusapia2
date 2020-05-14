using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothMouseLook : MonoBehaviour
{
    public Camera cam;
    public PlayerController player;

    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;

    public float minimumX = -360F;
    public float maximumX = 360F;

    public float minimumY = -60F;
    public float maximumY = 60F;

    public float minimumCameraDistance = 1;
    public float maximumCameraDistance = 5;

    public float rotationX = 0F;
    public float rotationY = 0F;

    private List<float> rotArrayX = new List<float>();
    float rotAverageX = 0F;

    private List<float> rotArrayY = new List<float>();
    public float rotAverageY = 0F;

    public float frameCounter = 20;

    Quaternion originalRotation;

    private float y;
    private bool sliding;
    private float dist;
    RaycastHit hit;

    public LayerMask mirrorMask;
    private Mirror hiddenMirror;

    void Update()
    {
        if (!player.isMirrored)
        {
            Vector3 resetPos = transform.TransformPoint(new Vector3(0, cam.transform.localPosition.y, cam.transform.localPosition.z));
            Vector3 fakePoint = new Vector3(resetPos.x, player.transform.position.y, resetPos.z);
            player.camForward = transform.forward; //(player.transform.position - fakePoint).normalized;
            Debug.DrawLine(player.transform.position, player.transform.position + transform.forward * 3, Color.red);
        }

        if (Physics.Raycast(cam.transform.position, Vector3.down, out hit, Mathf.Infinity))
        {
            dist = Vector3.Distance(cam.transform.position, hit.point);
        }

        if (axes == RotationAxes.MouseXAndY)
        {
            rotAverageY = 0f;
            rotAverageX = 0f;

            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationX += Input.GetAxis("Mouse X") * sensitivityX;

            rotationY = Mathf.Clamp(rotationY, -50, 20);

            rotArrayY.Add(rotationY);
            rotArrayX.Add(rotationX);

            if (rotArrayY.Count >= frameCounter)
            {
                rotArrayY.RemoveAt(0);
            }
            if (rotArrayX.Count >= frameCounter)
            {
                rotArrayX.RemoveAt(0);
            }

            for (int j = 0; j < rotArrayY.Count; j++)
            {
                rotAverageY += rotArrayY[j];
            }
            for (int i = 0; i < rotArrayX.Count; i++)
            {
                rotAverageX += rotArrayX[i];
            }

            rotAverageY /= rotArrayY.Count;
            rotAverageX /= rotArrayX.Count;

            rotAverageY = ClampAngle(rotAverageY, minimumY, maximumY);
            rotAverageX = ClampAngle(rotAverageX, minimumX, maximumX);

            Quaternion yQuaternion = Quaternion.AngleAxis(rotAverageY, Vector3.left);
            Quaternion xQuaternion = Quaternion.AngleAxis(rotAverageX, Vector3.up);

            if (dist < cam.transform.localPosition.y - 0.1f)
            {
                if (!sliding)
                {
                    sliding = true;
                    y = rotAverageY;                    
                }
                else
                {
                    if (rotAverageY > y)
                    {
                        float p = Mathf.Clamp((rotAverageY - y) / (60 - y), 0, 0.35f);
                        if (p < 0)
                        {
                            cam.transform.localPosition = new Vector3(1, cam.transform.localPosition.y, -maximumCameraDistance);
                            y = 0;
                            sliding = false;
                        }
                        else
                        {
                            //print(p);
                            cam.transform.localPosition = new Vector3(1, cam.transform.localPosition.y, Mathf.Lerp(-maximumCameraDistance, -minimumCameraDistance, p * 2.5f));
                        }
                    }
                    else
                    {
                        cam.transform.localPosition = new Vector3(1, cam.transform.localPosition.y, -maximumCameraDistance);
                        y = 0;
                        sliding = false;
                    }
                }
            }
            else
            {
                cam.transform.localPosition = new Vector3(1, cam.transform.localPosition.y, -maximumCameraDistance);
                y = 0;
                sliding = false;
            }

            transform.localRotation = originalRotation * xQuaternion * yQuaternion;
        }
        else if (axes == RotationAxes.MouseX)
        {
            rotAverageX = 0f;

            rotationX += Input.GetAxis("Mouse X") * sensitivityX;

            rotArrayX.Add(rotationX);

            if (rotArrayX.Count >= frameCounter)
            {
                rotArrayX.RemoveAt(0);
            }
            for (int i = 0; i < rotArrayX.Count; i++)
            {
                rotAverageX += rotArrayX[i];
            }
            rotAverageX /= rotArrayX.Count;

            rotAverageX = ClampAngle(rotAverageX, minimumX, maximumX);

            Quaternion xQuaternion = Quaternion.AngleAxis(rotAverageX, Vector3.up);
            transform.localRotation = originalRotation * xQuaternion;
        }
        else
        {
            rotAverageY = 0f;

            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;

            rotArrayY.Add(rotationY);

            if (rotArrayY.Count >= frameCounter)
            {
                rotArrayY.RemoveAt(0);
            }
            for (int j = 0; j < rotArrayY.Count; j++)
            {
                rotAverageY += rotArrayY[j];
            }
            rotAverageY /= rotArrayY.Count;

            rotAverageY = ClampAngle(rotAverageY, minimumY, maximumY);

            Quaternion yQuaternion = Quaternion.AngleAxis(rotAverageY, Vector3.left);
            transform.localRotation = originalRotation * yQuaternion;
        }

        CheckIfMirrorBlockingView();
    }

    void LateUpdate()
    {
        transform.position = player.transform.position;
    }

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb)
            rb.freezeRotation = true;
        originalRotation = transform.localRotation;
    }

    public void CheckIfMirrorBlockingView()
    {
        RaycastHit hit1;
        Vector3 dir = (player.transform.position - cam.transform.position).normalized;
        float dst = Vector3.Distance(player.transform.position, cam.transform.position);
        if (Physics.Raycast(cam.transform.position, dir, out hit1, dst, mirrorMask))
        {
            Mirror m = hit1.collider.GetComponent<Mirror>();
            if (m)
            {
                m.isBlockingCamera = true;
                hiddenMirror = m;
            }
        }
        else
        {
            if (hiddenMirror)
            {
                hiddenMirror.isBlockingCamera = false;
                hiddenMirror.ReactivatePortal();
            }
        }
        Debug.DrawRay(cam.transform.position, dir * dist, Color.cyan);
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        angle = angle % 360;
        if ((angle >= -360F) && (angle <= 360F))
        {
            if (angle < -360F)
            {
                angle += 360F;
            }
            if (angle > 360F)
            {
                angle -= 360F;
            }
        }
        return Mathf.Clamp(angle, min, max);
    }
}
