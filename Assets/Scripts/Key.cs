using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.instance.data.firstKey)
            gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddKey(int keyNumber)
    {
        switch (keyNumber)
        {
            case 0:
                GameManager.instance.data.firstKey = true;
                break;
            case 1:
                GameManager.instance.data.secondKey = true;
                break;
            case 2:
                GameManager.instance.data.thirdKey = true;
                break;
            default:
                break;
        }
    }
}
