using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VariableLanguage : MonoBehaviour
{
    public enum DataType { Image, Text, Int}
    public DataType dataType;
    public string category;
    public Sprite rus;
    public Sprite eng;
    public string strRus;
    public string strEng;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        switch ((int)dataType)
        {
            case 0:
                if (GameManager.instance.data.language == GameManager.Language.RU)
                    GetComponent<Image>().sprite = rus;
                else
                    GetComponent<Image>().sprite = eng;
                break;
            case 1:
                if (GameManager.instance.data.language == GameManager.Language.RU)
                    GetComponent<Text>().text = strRus.Replace("\\n", "\n");
                else
                    GetComponent<Text>().text = strEng.Replace("\\n", "\n");
                break;
            case 2:

                GetComponent<Text>().text = ((int)GameManager.instance.data[category]).ToString();
                break;
            default:
                break;
        }
    }
}
