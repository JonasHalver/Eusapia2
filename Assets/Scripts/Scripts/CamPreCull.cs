using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPreCull : MonoBehaviour
{
    Mirror[] mirrors;

    void Awake()
    {
        mirrors = FindObjectsOfType<Mirror>();
    }

    void LateUpdate()
    {
        for (int i = 0; i < mirrors.Length; i++)
        {
            //mirrors[i].Render();
        }
    }
}
