using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToiletBar : MonoBehaviour
{
    GameObject player;
    Transform bar;
    Transform cursor;
    [HideInInspector]
    public Vector3 backup;
    [HideInInspector]
    public BoxCollider zone;
    public Vector3 offset;
    public float speed;
    public float sense;
    public float border = 2.35f;
    public float poopEfficiency = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        bar = transform.Find("Шкала_1");
        cursor = transform.Find("Шкала_2");
        player = GameObject.Find("Player");
        transform.position = offset + player.transform.position;
        player.GetComponent<PlayerController>().freeze = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
            cursor.position += new Vector3(0, sense, 0);

        cursor.position += new Vector3(0, Time.deltaTime * speed, 0);
        if (cursor.localPosition.y > border)
            cursor.localPosition = new Vector3(0, border, -0.01f);
        else if (cursor.localPosition.y < -border)
        {            
            player.GetComponent<Animator>().SetBool("poop", false);
            player.GetComponent<Timer>().freeze = false;
            player.transform.position = backup;
            GameObject.Find("Main Camera").GetComponent<Follow>().player = player.transform;
            Destroy(zone.gameObject);
            player.GetComponent<PlayerController>().freeze = false;
            Destroy(gameObject);
        }

        player.GetComponent<Animator>().SetFloat("speed", (cursor.localPosition.y + border)/(2*border));

        if (cursor.localPosition.y > border / 2)
            AddValue(poopEfficiency * -2);
        else if (cursor.localPosition.y > 0)
            AddValue(poopEfficiency * -1.5f);
        else
            AddValue(poopEfficiency * -1);

    }

    void AddValue(float value)
    {
        player.GetComponent<PlayerController>().curPoopValue += value * Time.deltaTime;
    }
}
