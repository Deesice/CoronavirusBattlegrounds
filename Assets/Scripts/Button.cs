using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerUpHandler, IPointerExitHandler
{
    Image sp;

    public UnityEvent onClick;    
    public Sprite def;
    public Sprite onEnter;
    public Sprite onDown;
    public float latency = 0.1f;
    public int value;
    public GameObject loadingScreen;
    // Start is called before the first frame update
    void Awake()
    {
        sp = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDisable()
    {
        sp.sprite = def;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        AudioSystem.instance.PlaySound(12);
        sp.sprite = onDown;
        Invoke("Action", latency);
    }
    void Action()
    {
        onClick.Invoke();
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        sp.sprite = onEnter;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioSystem.instance.PlaySound(11);
        sp.sprite = onEnter;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        sp.sprite = def;
    }
    public void LoadScene(string scName)
    {
        AudioSystem.instance.PlaySound(13);
        var screen = Instantiate(loadingScreen);
        screen.transform.SetParent(GameObject.Find("Canvas").transform);        
        screen.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        screen.GetComponent<RectTransform>().localPosition = new Vector3(5800, 0, 0);
        screen.AddComponent<MoveTo>();
        screen.GetComponent<MoveTo>().direction = MoveTo.Direction.Left;
        screen.GetComponent<MoveTo>().speed = 3000;
        screen.GetComponent<MoveTo>().isLoadingScreen = true;

        Debug.Log("Tryyyyy");

        GameManager.instance.LoadSceneLatency(scName);
    }
    public void ChangeLang()
    {
        int a = (int)GameManager.instance.data.language;
        a += 1;
        a %= 2;
        GameManager.instance.data.language = (GameManager.Language)a;
    }
    public void AddValue(int category)
    {
        switch (category)
        {
            case 0:
                GameManager.instance.data.volSFX += value;
                GameManager.instance.CheckData();
                break;
            case 1:
                GameManager.instance.data.volMusic += value;
                GameManager.instance.CheckData();
                break;
            case 2:
                GameManager.instance.data.volVoice += value;
                GameManager.instance.CheckData();
                break;
            case 3:
                GameManager.instance.data.fontSize += value;
                GameManager.instance.CheckData();
                break;
            default: break;
        }
    }
    public void OpenURL(string link)
    {
        Application.OpenURL(link);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
