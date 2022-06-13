using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheaterScript : MonoBehaviour
{
    public float[] durations;
    int curStage;
    public bool pause = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!pause)
        {
            durations[curStage] -= Time.deltaTime;
            if (durations[curStage] <= 0)
            {
                curStage++;
                GetComponent<Animator>().SetInteger("stage", curStage);
                if (curStage >= durations.Length)
                    pause = true;
            }
        }
    }

    public void Play()
    {
        pause = false;
    }
}
