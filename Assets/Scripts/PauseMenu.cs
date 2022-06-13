using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    public enum FlowState {Default, GameOver, Complete}
    [HideInInspector]
    public FlowState state = FlowState.Default;
    bool isPaused = false;
    GameObject panel;
    GameObject settings;
    Image gut;
    Image clock;
    GameObject stats;
    public float latency = 2;
    
    // Start is called before the first frame update
    void Start()
    {
        stats = transform.Find("Stats").gameObject;
        panel = transform.Find("Panel").gameObject;
        settings = panel.transform.Find("======SETTINGS======").gameObject;
        gut = transform.Find("Gut").Find("Gut_full").GetComponent<Image>();
        clock = transform.Find("Clock").Find("Clock_full").GetComponent<Image>();
    }

    public void SetClock(float value)
    {
        clock.fillAmount = value;
    }

    public void SetGut(float value)
    {
        gut.fillAmount = value;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case FlowState.Default:
                if (Input.GetButtonDown("Cancel"))
                    if (settings.activeSelf)
                        settings.transform.Find("BackB").GetComponent<Button>().onClick.Invoke();
                    else if (isPaused)
                        Resume();
                    else
                        Pause();
                break;
            case FlowState.GameOver:                
                break;
            case FlowState.Complete:
                latency -= Time.deltaTime;
                if (latency <= 0)
                {
                    stats.SetActive(true);
                    if (Input.anyKeyDown)
                        transform.Find("Panel").transform.Find("MenuB").GetComponent<Button>().LoadScene("MainMenu");
                }
                break;
        }
    }

    public void Resume()
    {
        Time.timeScale = 1;
        panel.SetActive(false);
        isPaused = false;

        if (state == FlowState.Default)
            SetClockColor(Color.white);

        AudioListener.pause = false;
    }

    public void Pause()
    {
        Time.timeScale = 0;
        panel.SetActive(true);
        isPaused = true;
        SetClockColor(Color.gray);

        AudioListener.pause = true;
    }
    public void SetClockColor(Color color)
    {
        gut.color = color;
        clock.color = color;
        gut.transform.parent.GetComponent<Image>().color = color;
        clock.transform.parent.GetComponent<Image>().color = color;
    }
}
