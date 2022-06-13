using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSpatial : MonoBehaviour
{
    public float minDistanse = 2;
    public float maxDistanse = 20;
    public bool ignoredPause = false;
    float coef;
    Transform player;
    AudioSource a;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        a = GetComponent<AudioSource>();
        a.rolloffMode = AudioRolloffMode.Linear;
        if (ignoredPause)
            a.ignoreListenerPause = true;

        minDistanse *= transform.lossyScale.y;
        maxDistanse *= transform.lossyScale.y;
        a.dopplerLevel = 0;
    }

    // Update is called once per frame
    void Update()
    {
        a.maxDistance = maxDistanse;
        a.minDistance = minDistanse;
        coef = (new Vector3 (transform.position.x - player.position.x, (transform.position.y - player.position.y)*2, transform.position.z - player.position.z)).magnitude - a.minDistance;
        if (coef < 0)
            coef = 0;
        coef /= a.maxDistance - a.minDistance;
        if (coef > 1)
            coef = 1;
        a.spatialBlend = Mathf.Lerp(0, 1, coef);
    }
}
