using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorManController : MonoBehaviour
{
    public PlayerController player;
    public static GameObject mirrorMan;
    public Mirror.World currentWorld = Mirror.World.Dead;
    public Animator anim;

    void Awake()
    { 
        mirrorMan = gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();  
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        anim.SetFloat("walking", PlayerController.PlayerInput.magnitude);
        transform.localPosition = player.transform.localPosition;
        transform.localRotation = player.transform.localRotation;
    }
}
