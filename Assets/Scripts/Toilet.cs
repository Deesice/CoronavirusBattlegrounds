using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toilet : MonoBehaviour
{
    public Transform anchor;
    public int goalNumber;
    Transform player;
    public GameObject toiletBar;
    public BoxCollider zone;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate()
    {
        var camera = GameObject.Find("Main Camera").GetComponent<Follow>();
        camera.player = new GameObject().transform;
        camera.player.localScale = player.transform.lossyScale;
        camera.player.position = player.position;

        var bar = Instantiate(toiletBar, anchor.position, toiletBar.transform.rotation);
        bar.GetComponent<ToiletBar>().backup = player.position;
        player.position = anchor.position;
        player.GetComponent<Animator>().SetBool("poop", true);
        player.GetComponent<Timer>().freeze = true;

        bar.GetComponent<ToiletBar>().zone = zone;
        zone.gameObject.SetActive(true);
        zone.enabled = true;
        camera.player.position = new Vector3(anchor.position.x, camera.player.position.y, camera.player.position.z);

        GameManager.instance.data.curGoals[goalNumber] = true;

        Destroy(GetComponent<SpriteRenderer>());
    }
}
