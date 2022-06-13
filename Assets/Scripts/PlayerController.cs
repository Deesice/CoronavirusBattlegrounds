using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed;
    public GameObject focusedItem;
    public GameObject skillcheck;
    public GameObject toiletBar;
    public float startSkillTime;
    public float endSkillTime;
    public float curPoopValue;
    Vector3 dir;
    Animator animator;
    [HideInInspector]
    public float lastInteractTime;
    [HideInInspector]
    public bool blockLeftMovement;
    [HideInInspector]
    public bool blockRightMovement;
    [HideInInspector]
    public bool freeze;
    PauseMenu ui;
    float startPoopValue;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnSkillchecks());
        lastInteractTime = -0.75f;
        animator = GetComponent<Animator>();
        ui = GameObject.Find("Canvas").GetComponent<PauseMenu>();
        startPoopValue = curPoopValue;
    }
    IEnumerator SpawnSkillchecks()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(startSkillTime, endSkillTime));
            if (!animator.GetBool("poop"))
                AudioSystem.instance.PlaySound(0);
            yield return new WaitForSeconds(1);
            yield return new WaitForSeconds(0.75f - (Time.time - lastInteractTime) > 0 ? 0.75f - (Time.time - lastInteractTime) : 0);
            if (!animator.GetBool("poop"))
                Instantiate(skillcheck, skillcheck.transform.position, skillcheck.transform.rotation);
        }
    }
    // Update is called once per frame
    void Update()
    {
        RefreshGut();
        Interact();

        var input = freeze ? 0 : Input.GetAxis("Horizontal");
        //StepControl(input);
        if (input < 0 && blockLeftMovement)
        {
            input = 0;
            animator.SetTrigger("turnLeft");
        }
        else if (input > 0 && blockRightMovement)
        {
            input = 0;
            animator.SetTrigger("turnRight");
        }
        else
        {
            animator.ResetTrigger("turnLeft");
            animator.ResetTrigger("turnRight");
        }

        if (!freeze && GetComponent<NPCController>().lookAt == null)
            animator.SetFloat("speed", input);
        dir.x = input * Time.deltaTime * walkSpeed * transform.lossyScale.y;
        transform.position += dir;       
    }

    public void Cycle(float time)
    {
        Debug.Log(Time.time);
        GameManager.instance.Invoke(Cycle,time,time);
    }
    public void Freeze(float time)
    {
        freeze = true;
        Invoke("Unfreeze", time);
    }
    void Unfreeze()
    {
        freeze = false;
    }

    public void GameOver()
    {
        animator.SetBool("gameOver", true);
    }

    public void PreGameOver()
    {
        AudioSystem.instance.PlaySound(3);
        animator.SetBool("gameOver", true);
        freeze = true;
        StopAllCoroutines();
        Invoke("SendGameManager", 1.5f);
        GameManager.instance.GameOver();
    }
    void SendGameManager()
    {
        GameManager.instance.GameOver();
    }

    void Interact()
    {
        if (Input.GetButtonDown("Jump") && focusedItem != null && (Time.time - lastInteractTime) > 0.75f)
        {
            if (focusedItem.GetComponent<InteractObject>().type == InteractObject.TypeOfInteract.Other && !focusedItem.GetComponent<InteractObject>().block)
            {
                if (focusedItem.GetComponent<InteractObject>().interactSound == null)
                    AudioSystem.instance.PlaySound(1);
                lastInteractTime = Time.time;
                Freeze(0.75f);
                animator.SetTrigger("action");
                focusedItem.GetComponent<InteractObject>().Action(0.375f);
            }
            else
            {
                if (focusedItem.GetComponent<InteractObject>().block)
                    AudioSystem.instance.PlaySound(2);
                focusedItem.GetComponent<InteractObject>().Action();
            }
        }
    }

    void RefreshGut()
    {
        ui.SetGut(curPoopValue / startPoopValue);

        if (curPoopValue <= 0 && !animator.GetBool("poop"))
        {
            animator.SetBool("victory", true);            
            freeze = true;
            StopAllCoroutines();
            GetComponent<Timer>().freeze = true;
            GameManager.instance.data.curTime = GetComponent<Timer>().startTime - GetComponent<Timer>().timeLeft;
            GameManager.instance.Complete();
            curPoopValue = 0.0001f;
        }
    }

    void StepControl(float input)
    {
        var stepper = GetComponent<Stepper>();
        if (!stepper)
            return;

        if (input != 0)
            stepper.timeToStep -= Time.deltaTime;
    }

    public void DropItem(GameObject itemOnFloor)
    {
        var curSprite = transform.Find("Тело").Find("Бутылка").GetComponent<SpriteRenderer>().sprite;
        if (itemOnFloor.GetComponent<SpriteRenderer>().sprite == curSprite)
        {
            itemOnFloor.SetActive(true);
            itemOnFloor.GetComponent<BoxCollider>().enabled = true;
        }
    }

    public void ItemActive(bool b)
    {
        transform.Find("Тело").Find("Бутылка").gameObject.SetActive(b);
        transform.Find("Рука").Find("Бутылка (1)").gameObject.SetActive(b);
    }
}
