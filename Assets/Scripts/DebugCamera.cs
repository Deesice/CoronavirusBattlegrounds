using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Camera>().transparencySortMode = TransparencySortMode.Orthographic;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
