using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skillcheck : MonoBehaviour
{
    public float startAngle;
    public float endAngle;
    Transform cursor;
    Transform bar;
    float idealAngle;
    float curAngle;
    Vector3 offset;
    GameObject player;

    public float allowance; //from 0 to 1
    public float rotateSpeed;
    public float greatZone;
    public float goodZone;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.data.totalSkill++;
        player = GameObject.Find("Player");
        bar = transform.Find("Скиллчек_1");
        cursor = transform.Find("Скиллчек_2");
        var mainCamera = GameObject.Find("Main Camera").transform;

        offset.y = Random.Range(-mainCamera.GetComponent<Camera>().orthographicSize, mainCamera.GetComponent<Camera>().orthographicSize) * allowance;
        offset.x = Random.Range(-mainCamera.GetComponent<Camera>().orthographicSize * mainCamera.GetComponent<Camera>().aspect,
                                 mainCamera.GetComponent<Camera>().orthographicSize * mainCamera.GetComponent<Camera>().aspect) * allowance;
        
        offset.z = 1.0f;

        transform.position = mainCamera.position + offset;
        GetComponent<Follow>().enabled = true;
        GetComponent<Follow>().offset = offset;
        GetComponent<Follow>().player = mainCamera;
        

        idealAngle = Random.Range(startAngle, endAngle);
        bar.rotation *= Quaternion.AngleAxis(-idealAngle, new Vector3(0, 0, 1));
    }

    // Update is called once per frame
    void Update()
    {
        curAngle += Time.deltaTime * rotateSpeed;
        cursor.rotation = Quaternion.AngleAxis(-curAngle, new Vector3(0, 0, 1));
        if (curAngle > 360)
            Interact();
    }
    public void Interact()
    {
        if (curAngle < idealAngle)
        {
            Debug.Log("Рано");
            AddValue(-10);
            AudioSystem.instance.PlaySound(Random.Range(7,11));
        }
        else if (curAngle >= idealAngle && curAngle < idealAngle + greatZone)
        {
            Debug.Log("Отлично");
            GameManager.instance.data.greatSkill++;
            AddValue(5);
            AudioSystem.instance.PlaySound(6);
        }
        else if (curAngle >= idealAngle + greatZone && curAngle < idealAngle + greatZone + goodZone)
        {
            GameManager.instance.data.goodSkill++;
            Debug.Log("Хорошо");
            AudioSystem.instance.PlaySound(6);
        }
        else if (curAngle >= idealAngle + greatZone + goodZone)
        {
            Debug.Log("Поздно");
            AddValue(-10);
            AudioSystem.instance.PlaySound(Random.Range(7, 11));
        }
        Destroy(gameObject);
    }
    void AddValue(float value)
    {
        player.GetComponent<Timer>().timeLeft += value;
    }
}
