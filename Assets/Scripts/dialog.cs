using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Audio;

public class dialog : MonoBehaviour
{
    public bool stopOnExit = true;
    public bool skipOnExit = false;
    public GameObject[] persons;
    public string[] rusPhrases;
    public string[] engPhrases;
    public AudioClip[] rusSounds;
    public AudioClip[] engSounds;

    public GameObject dialogCloud;
    public int maxStrLen;
    public UnityEvent Action;
    public AudioMixer mixer;
    Vector3 offset = new Vector3(0, 5.75f, -0.5f);

    Vector2 cloudSize;
    int curPhrase = 0;
    GameObject curCloud;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var i in persons)
        {
            AudioSource audio;
            if ((audio = i.GetComponent<AudioSource>()) == null)
                audio = i.AddComponent<AudioSource>();
            audio.outputAudioMixerGroup = mixer.FindMatchingGroups("Voice")[0];
            audio.spatialBlend = 0.5f;
            audio.playOnAwake = false;
            i.AddComponent<ControlSpatial>();
        }

    }

    // Update is called once per frame
    void Update()
    {
    }
    public void ResetDialog()
    {
        curPhrase = 0;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player" && curPhrase < rusPhrases.Length && curCloud == null)
            NextPhrase();

        if (other.gameObject.name == "Player" && rusPhrases.Length == 0)
            Action.Invoke();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            if (skipOnExit)
            {
                curPhrase = rusPhrases.Length - 1;
            }
            if (stopOnExit)
            {
                StopPhrase();
                if (skipOnExit)
                    Action.Invoke();
            }
        }
    }

    public void StopPhrase()
    {
        StartCoroutine(ResetPhrase(0));
    }

    void NextPhrase()
    {
        string[] langPhrases;
        AudioClip[] langSounds;

        if (GameManager.instance != null)
        {
            langPhrases = GameManager.instance.data.language == GameManager.Language.EN ? engPhrases : rusPhrases;
            langSounds = GameManager.instance.data.language == GameManager.Language.EN ? engSounds : rusSounds;
        }
        else
        {
            langPhrases = rusPhrases;
            langSounds = rusSounds;
        }

        int curLen = maxStrLen;

        if (langPhrases[curPhrase].Length <= maxStrLen)
            curLen = langPhrases[curPhrase].Length;
        else
            langPhrases[curPhrase] = ModString(langPhrases[curPhrase], ref curLen);

        cloudSize.y = (langPhrases[curPhrase].Length - langPhrases[curPhrase].Replace("\n", "").Length) / "\n".Length + 1;
        cloudSize.y *= (1.0f / Power(cloudSize.y, 2) + 1.0f);
        cloudSize.x = curLen * 0.4f + 1;
        if (GameManager.instance != null)
            cloudSize *= GameManager.instance.data.fontSize / 72.0f;
        cloudSize *= persons[curPhrase % persons.Length].transform.lossyScale.y;

        //while (persons[curPhrase % persons.Length].GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("drinkLeft")
        //    || persons[curPhrase % persons.Length].GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("drinkRight"))
        //    yield return new WaitForEndOfFrame();

        int fontSize = GameManager.instance != null ? GameManager.instance.data.fontSize : 72;
        fontSize = (int)(fontSize * persons[curPhrase % persons.Length].transform.lossyScale.y);
        curCloud = CreateCloud(cloudSize, fontSize);
        curCloud.transform.Find("Canvas").transform.Find("Text").GetComponent<Text>().text = langPhrases[curPhrase];

        if (persons[curPhrase % persons.Length].GetComponent<Animator>())
            persons[curPhrase % persons.Length].GetComponent<Animator>().SetFloat("isTalking", 1.0f);

        StartCoroutine(ResetPhrase(langSounds[curPhrase].length));

        persons[curPhrase % persons.Length].GetComponent<AudioSource>().clip = langSounds[curPhrase];
        persons[curPhrase % persons.Length].GetComponent<AudioSource>().Play();
    }
    IEnumerator ResetPhrase(float phraseTime = 0)
    {
        if (phraseTime != 0)
            yield return new WaitForSeconds(phraseTime);

        if (curCloud != null)
        {
            Destroy(curCloud);
            foreach (var i in persons)
                if (i.GetComponent<Animator>() && i.GetComponent<Animator>().GetFloat("isTalking") == 1)
                    i.GetComponent<Animator>().SetFloat("isTalking", 0);
            persons[curPhrase % persons.Length].GetComponent<AudioSource>().Stop();
            curPhrase++;
            if (GetComponent<BoxCollider>().enabled)
            {
                GetComponent<BoxCollider>().enabled = false;
                GetComponent<BoxCollider>().enabled = true;
            }
            if (curPhrase >= rusPhrases.Length)
            {
                GetComponent<BoxCollider>().enabled = false;
                if (phraseTime > 0)
                {
                    Action.Invoke();
                    ResetDialog();
                }
            }
            StopAllCoroutines();
        }
    }

    float Power(float a, int b)
    {
        var buf = a;
        for (int i = 1; i < b; i++)
            a *= buf;

        return a;
    }

    GameObject CreateCloud(Vector2 cloudSize, int fontSize = 72)
    {
        var curCloud = Instantiate(dialogCloud, persons[curPhrase % persons.Length].transform.position + offset, dialogCloud.transform.rotation);

        curCloud.GetComponent<SpriteRenderer>().size = cloudSize;

        var canvas = curCloud.transform.Find("Canvas");
        canvas.GetComponent<RectTransform>().sizeDelta = cloudSize / canvas.GetComponent<RectTransform>().localScale.x;

        var text = canvas.transform.Find("Text");
        cloudSize.y -= 34 * canvas.GetComponent<RectTransform>().localScale.x;
        text.GetComponent<RectTransform>().sizeDelta = cloudSize / canvas.GetComponent<RectTransform>().localScale.x;
        text.GetComponent<Text>().fontSize = fontSize;

        curCloud.GetComponent<Follow>().player = persons[curPhrase % persons.Length].transform;
        curCloud.GetComponent<Follow>().offset = offset;

        return curCloud;
    }

    string ModString(string original, ref int maxChar)
    {

        List<int> indA = new List<int>();
        List<int> indB = new List<int>();
        int lastSpace = 0;
        original += ' ';
        var array = original.ToCharArray();
        int maxDiff = 0;
        int i;

        for (i = 0; i < array.Length; i++)
            if (array[i] == ' ' || array[i] == '\n')
            {
                indA.Add(i);
            }

        for (i = 0; i < indA.Count; i++)
            if (indA[i] >= lastSpace + maxChar)
            {
                indB.Add(indA[i]);
                maxDiff = Mathf.Max(maxDiff, indA[i] - lastSpace - maxChar);
                lastSpace = indA[i] + 1;
            }

        for (i = 0; i < indB.Count - 1; i++)
        {
            original = original.Remove(indB[i], 1);
            original = original.Insert(indB[i], "\n");
        }

        if (indB[i] != original.Length - 1)
        {
            original = original.Remove(indB[i], 1);
            original = original.Insert(indB[i], "\n");
        }
        else
            original = original.Remove(indB[i], 1);


        maxChar += maxDiff;

        return original;
    }
    public void DestroyObject()
    {
        StopAllCoroutines();
        foreach (var i in persons)
            if (i.GetComponent<Animator>().GetFloat("isTalking") == 1)
                i.GetComponent<Animator>().SetFloat("isTalking", 0);
        Destroy(curCloud);
        Destroy(gameObject);
    }

    public void DestroyComponent()
    {
        StopAllCoroutines();
        foreach (var i in persons)
            if (i.GetComponent<Animator>().GetFloat("isTalking") == 1)
                i.GetComponent<Animator>().SetFloat("isTalking", 0);
        Destroy(curCloud);
        Destroy(this);
    }
}
