using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curtain : MonoBehaviour
{
    public float speed;
    GameObject plane_L;
    GameObject plane_R;
    // Start is called before the first frame update
    void Start()
    {
        var cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        transform.position = cam.transform.position + new Vector3(0,0,5);
        plane_L = transform.Find("Plane_L").gameObject;
        plane_R = transform.Find("Plane_R").gameObject;
        plane_L.GetComponent<Cloth>().enabled = false;
        plane_R.GetComponent<Cloth>().enabled = false;
        plane_L.transform.localPosition = new Vector3(-cam.orthographicSize*cam.aspect, 0, 1f);
        plane_R.transform.localPosition = new Vector3(cam.orthographicSize * cam.aspect, 0, -1f);
        plane_L.GetComponent<Cloth>().enabled = true;
        plane_R.GetComponent<Cloth>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        var dir = new Vector3(speed * Time.deltaTime, 0, 0);
        plane_L.transform.localPosition += dir;
        plane_R.transform.localPosition -= dir;

        if (plane_L.transform.localPosition.x >= -4.5f || plane_R.transform.localPosition.x <= 4.5f)
            speed = 0;
    }
}
