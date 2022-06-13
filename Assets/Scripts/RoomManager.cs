using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [HideInInspector]
    public bool enable;
    public float appearanceTime = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableRoom(Transform parent = null)
    {
        enable = true;
        if (parent == null)
            parent = transform;
        for (int i = 0; i < parent.childCount; i++)
        {
            EnableRoom(parent.GetChild(i));
        }
        if (parent.GetComponent<SpriteRenderer>() && !parent.gameObject.CompareTag("Outline"))
            StartCoroutine(EnableSprite(parent.GetComponent<SpriteRenderer>(), appearanceTime));
    }
    IEnumerator EnableSprite(SpriteRenderer sprite, float totalTime)
    {
        float curTime = 0;
        var white = new Color(1, 1, 1, 1);
        var transparent = sprite.color;
        //sprite.enabled = true;
        while (curTime < totalTime)
        {
            sprite.color = Color.Lerp(transparent, white, curTime / totalTime);
            
            yield return null;
            curTime += Time.deltaTime;
        }
        sprite.color = white;
    }
    public void DisableRoom(Transform parent = null)
    {
        enable = false;
        if (parent == null)
            parent = transform;
        if (parent.gameObject.GetComponent<SpriteRenderer>() && !parent.gameObject.CompareTag("Outline"))
            if (appearanceTime == 0)
            {
                parent.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
                Debug.Log(parent.gameObject.name);
            }
            else
                StartCoroutine(DisableSprite(parent.gameObject.GetComponent<SpriteRenderer>(), appearanceTime));
        for (int i = 0; i < parent.childCount; i++)
        {
            DisableRoom(parent.GetChild(i));
        }        
    }

    IEnumerator DisableSprite(SpriteRenderer sprite, float totalTime)
    {
        float curTime = 0;
        var white = sprite.color;
        var transparent = new Color(1, 1, 1, 0);
        while (curTime < totalTime)
        {
            sprite.color = Color.Lerp(white, transparent, curTime / totalTime);

            yield return null;
            curTime += Time.deltaTime;
        }
        sprite.color = transparent;
        //sprite.enabled = false;
    }
}
