using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorHandler : MonoBehaviour
{
    public static Dictionary<Mirror, Mirror> mirrorPairs = new Dictionary<Mirror, Mirror>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void UsedPortal()
    {
        foreach(KeyValuePair<Mirror, Mirror> kvp in mirrorPairs)
        {
            GameObject holder = kvp.Key.player;
            kvp.Key.player = kvp.Value.player;
            kvp.Value.player = holder;
        }
    }
}
