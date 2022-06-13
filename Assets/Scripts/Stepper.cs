using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Stepper : MonoBehaviour
{
    public float timeToStep;
    public AudioClip[] clips;
    public AudioMixer mixer;
    float bufTime;
    AudioSource src;
    Vector3 curPos;
    // Start is called before the first frame update
    void Start()
    {
        bufTime = timeToStep;
        curPos = transform.position;
        var n = new GameObject();
        n.transform.position = gameObject.transform.position;
        n.transform.SetParent(gameObject.transform);
        n.name = "Stepper";
        
        src = n.AddComponent<AudioSource>();
        src.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
        n.AddComponent<ControlSpatial>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position != curPos)
        {
            timeToStep -= Time.deltaTime;
            curPos = transform.position;
        }

        if (timeToStep < 0)
        {
            timeToStep = bufTime;
            src.clip = clips[Random.Range(0, clips.Length)];
            src.Play();
        }
    }
}
