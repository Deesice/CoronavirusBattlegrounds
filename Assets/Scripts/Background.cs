using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    Transform mainCamera;
    Vector3 zeroCameraPosition;
    Vector3 zeroSelfPosition;
    Vector3 zeroSelfScale;
    public float distant;
    // Start is called before the first frame update
    void Awake()
    {
        mainCamera = GameObject.Find("Main Camera").transform;
        zeroCameraPosition = mainCamera.position;
        zeroSelfPosition = transform.position;
        zeroSelfScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = zeroSelfPosition + Vector3.Lerp(new Vector3(0,0,0), (mainCamera.position - zeroCameraPosition) * Mathf.Sign(distant), Mathf.Abs(distant));
        transform.localScale = Vector3.Lerp(zeroSelfScale, zeroSelfScale * (Mathf.Sign(distant) == 1 ? mainCamera.GetComponent<Camera>().orthographicSize/10.8f : 10.8f/ mainCamera.GetComponent<Camera>().orthographicSize), Mathf.Abs(distant));
    }
}
