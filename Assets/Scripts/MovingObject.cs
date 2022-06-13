using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public AnimationCurve curve;
    public Transform[] positions;
    [HideInInspector]
    public int curPos;
    public float speed = 1;
    public 
    // Start is called before the first frame update
    void Start()
    {
        curPos = 0;
    }

    public void SetPositionIndex(int i)
    {
        curPos = i;
        StopAllCoroutines();
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        var start = transform.position;
        for (float i = 0; i < 1; i += Time.deltaTime / speed)
        {
            transform.position = Vector3.Lerp(start, positions[curPos].position, curve.Evaluate(i));
            yield return null;
        }
    }
}
