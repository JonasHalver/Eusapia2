using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorManController : MonoBehaviour
{
    public PlayerController player;
    public static GameObject mirrorMan;

    void Awake()
    {
        mirrorMan = gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.localPosition = player.transform.localPosition;
        transform.localRotation = player.transform.localRotation;
    }
}
