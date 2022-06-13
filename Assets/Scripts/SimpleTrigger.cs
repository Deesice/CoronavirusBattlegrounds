using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SimpleTrigger : MonoBehaviour
{
    public enum TriggerType { reaction, toiletZone}
    public GameObject[] targets;
    public TriggerType type;
    public UnityEvent OnEnter;
    public UnityEvent OnExit;
    public bool randomOnExit = false;
    int targetsInZone = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.name == "trigger")
        Debug.Log(targetsInZone);
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (type)
        {
            case TriggerType.reaction:
                if (FindTarget(other.gameObject))
                {
                    targetsInZone++;
                    if (targetsInZone == 1)
                    {
                        Debug.Log(gameObject.name + " triggered");
                        OnEnter.Invoke();
                    }
                }
                break;
            case TriggerType.toiletZone:
                if (other.CompareTag("enemy"))
                    GameManager.instance.GameOver();
                break;
            default:
                break;
        }
                   
    }
    private void OnDisable()
    {
        targetsInZone = 0;
    }
    private void OnTriggerExit(Collider other)
    {
        if (FindTarget(other.gameObject))
        {
            targetsInZone--;
            if (targetsInZone == 0)
                if (!randomOnExit)
                    OnExit.Invoke();
                else
                    RandomOnExit();
        }
    }

    bool FindTarget(GameObject obj)
    {
        for (int i = 0; i < targets.Length; i++)
            if (targets[i] == obj)
                return true;
        return false;
    }

    public void RandomOnExit()
    {
        var index = Random.Range(0, OnExit.GetPersistentEventCount());
        var obj = OnExit.GetPersistentTarget(index);
        var str = OnExit.GetPersistentMethodName(index);

        if (str == "SetActive")
            (obj as GameObject).SetActive(true);
        Debug.Log(obj);
        Debug.Log(str);
    }
}
