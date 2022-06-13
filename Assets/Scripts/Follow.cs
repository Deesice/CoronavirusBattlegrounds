using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    public float distortionAmplitude = 0;
    public int distortionSoftness = 1;
    Vector3 origOffset;
    public float inertion;
    public float topBorder = 1000;
    IEnumerator curCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = player.position + offset * player.lossyScale.y;
        origOffset = offset;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var cam = GetComponent<Camera>();

        if (cam)
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 10.8f * player.lossyScale.y, Time.deltaTime * inertion);

        transform.position = Vector3.Lerp(transform.position, player.position + offset * player.lossyScale.y, Time.deltaTime * inertion);
        if (transform.position.y > topBorder)
            transform.position += new Vector3(0, topBorder - transform.position.y, 0);
    }

    public void TopBorder(float t)
    {
        topBorder = t;
    }

    public void Distortion(bool b)
    {
        if (b && curCoroutine == null)
        {
            curCoroutine = JuggleOffset();
            StartCoroutine(curCoroutine);
        }
        else if (!b && curCoroutine != null)
        {
            StopCoroutine(curCoroutine);
            curCoroutine = null;
            offset = origOffset;
        }
    }
    IEnumerator JuggleOffset()
    {
        while(true)
        {
            offset = new Vector3(origOffset.x + Random.Range(-distortionAmplitude, distortionAmplitude), origOffset.y + Random.Range(-distortionAmplitude, distortionAmplitude), origOffset.z);
            for (int i = 0; i < distortionSoftness; i++)
                yield return null;
        }
    }
}
