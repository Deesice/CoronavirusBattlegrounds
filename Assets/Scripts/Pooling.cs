using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.Events;

public class Pooling : MonoBehaviour
{
    public enum PoolType {Time, Goals, Key, Bool, Coefficient };
    public PoolType poolType;
    public string dataName;
    
    // Start is called before the first frame update
    void Start()
    {
        switch((int)poolType)
        {
            case 0:
                PoolTime();
                break;
            case 1:
                PoolGoals();
                break;
            case 2:
                PoolKey();
                break;
            case 3:
                PoolBool();
                break;
            case 4:
                PoolCoefficient();
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    public void PoolTime()
    {
        var f = (float)GameManager.instance.data[dataName];
        int seconds = (int)f;
        f -= seconds;
        f *= 100;
        int milliseconds = (int)f;
        int minutes = seconds / 60;
        seconds %= 60;

        if (seconds + minutes + milliseconds == 0)
            GetComponent<Text>().text = "--/--/--";
        else
            GetComponent<Text>().text =
            (minutes < 10 ? "0" : "") +
            minutes.ToString() + ":" +
            (seconds < 10 ? "0" : "") +
            seconds.ToString() + ":" +
            (milliseconds < 10 ? "0" : "") +
            milliseconds.ToString();
    }

    public void PoolGoals()
    {
        var b = (bool[])GameManager.instance.data[dataName];
        int counter = 0;
        foreach (var i in b)
            if (i)
                counter++;

        GetComponent<Text>().text = counter.ToString() + "/5";
    }
    public void PoolKey()
    {
        var b = (bool)GameManager.instance.data[dataName];

        if (b)
            GetComponent<Image>().enabled = true;
    }
    public void PoolBool()
    {
        var yes = GameObject.Find("Canvas").transform.Find("Галочка").GetComponent<Image>().sprite;
        var no = GameObject.Find("Canvas").transform.Find("Крестик").GetComponent<Image>().sprite;
        if (GameManager.instance.data.curGoals[int.Parse(dataName)])
            GetComponent<Image>().sprite = yes;
        else
            GetComponent<Image>().sprite = no;
    }

    public void PoolCoefficient()
    {
        GetComponent<Text>().text = ((GameManager.instance.data.goodSkill + GameManager.instance.data.greatSkill) * 100 / GameManager.instance.data.totalSkill).ToString();
    }
}
