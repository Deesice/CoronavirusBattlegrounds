using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Reflection;
public class ReflectableClass
{
    public object this[string key]
    {
        get
        {
            return this.GetType().GetField(key, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).GetValue(this);
        }
        set
        {
            this.GetType().GetField(key, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).SetValue(this, value);
        }
    }

}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Serializable]
    public enum Language { EN, RU };
    [Serializable]
    public class Data : ReflectableClass
    {
        public Language language = Language.EN;
        public int volSFX = 5;
        public int volMusic = 5;
        public int volVoice = 5;
        public float firstTime = 0;
        public float secondTime = 0;
        public float thirdTime = 0;
        public float curTime = 0;
        public int totalSkill = 0;
        public int greatSkill = 0;
        public int goodSkill = 0;
        public bool[] firstGoals = { false, false, false, false, false };
        public bool[] secondGoals = { false, false, false, false, false };
        public bool[] thirdGoals = { false, false, false, false, false };
        public bool[] curGoals = { false, false, false, false, false };
        public bool firstKey = false;
        public bool secondKey = false;
        public bool thirdKey = false;
        public int fontSize = 72;
    }
    public GameObject firstLogo;
    public GameObject eng;
    public GameObject rus;
    private bool firstLaunch = false;
    public Data data = new Data();
    string path;
    public AudioMixer mixer;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);

#if UNITY_ANDROID && !UNITY_EDITOR
        path = Path.Combine(Application.persistentDataPath, "Save.poop");
#else
        path = Path.Combine(Application.dataPath, "Save.poop");
#endif
        if (File.Exists(path))
        {
            data = JsonUtility.FromJson<Data>(File.ReadAllText(path));
            firstLogo.SetActive(true);
        }
        else
        {
            firstLaunch = true;
            eng.SetActive(true);
            rus.SetActive(true);
        }

        CheckData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeLang()
    {
        int a = (int)data.language;
        a += 1;
        a %= 2;
        data.language = (Language)a;
    }
    public void LoadSceneLatency(string name)
    {
        ResetVariable();
        StartCoroutine(loool(name));
    }
    public void LoadScene(string name)
    {
        ResetVariable();
        SceneManager.LoadScene(name);
    }
    static IEnumerator loool(string scName)
    {
        yield return new WaitForSeconds(1);
        Debug.Log("Loading" + scName);
        if (scName == "_current")
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        else
            SceneManager.LoadScene(scName);
    }
    public void GameOver()
    {
        AudioSystem.instance.PlaySound(4);
        var player = GameObject.Find("Player");
        foreach (var i in GameObject.FindGameObjectsWithTag("enemy"))
            i.GetComponent<NPCController>().GameOver(player.transform.position);

        player.GetComponent<PlayerController>().GameOver();
        GameObject.Find("Main Camera").GetComponent<PostProcessingAnimation>().GameOver();
        Destroy(GameObject.Find("Прогресс_туалета(Clone)"));

        var ui = GameObject.Find("Canvas").GetComponent<PauseMenu>();
        ui.SetClockColor(new Color(0, 0, 0, 0));
        ui.state = PauseMenu.FlowState.GameOver;
    }

    public void Complete()
    {
        AudioSystem.instance.PlaySound(5);
        SaveResults();
        GameObject.Find("Main Camera").transform.Find("Curtain").gameObject.SetActive(true);
        var ui = GameObject.Find("Canvas").GetComponent<PauseMenu>();
        ui.SetClockColor(new Color(0, 0, 0, 0));
        ui.state = PauseMenu.FlowState.Complete;
    }
    public void CheckData()
    {
        if (data.volSFX < 0)
            data.volSFX = 0;
        if (data.volSFX > 10)
            data.volSFX = 10;

        mixer.SetFloat("SFX", Mathf.Lerp(-50, 0, data.volSFX / 10.0f));

        if (data.volMusic < 0)
            data.volMusic = 0;
        if (data.volMusic > 10)
            data.volMusic = 10;

        mixer.SetFloat("Music", Mathf.Lerp(-50, 0, data.volMusic / 10.0f));

        if (data.volVoice < 0)
            data.volVoice = 0;
        if (data.volVoice > 10)
            data.volVoice = 10;

        mixer.SetFloat("Voice", Mathf.Lerp(-50, 0, data.volVoice / 10.0f));
    }

    void ResetVariable()
    {
        data.curTime = 0;
        data.greatSkill = 0;
        data.goodSkill = 0;
        data.totalSkill = 0;
        for (int i = 0; i < 5; i++)
            data.curGoals[i] = false;
    }
    void SaveResults()
    {
        data.curTime += data.greatSkill * 5 + (data.totalSkill - data.greatSkill - data.goodSkill) * (-10);
        switch(SceneManager.GetActiveScene().name)
        {
            case "Level1":
                if (data.curTime < data.firstTime || data.firstTime == 0)
                    data.firstTime = data.curTime;
                for (int i = 0; i < 5; i++)
                    data.firstGoals[i] = data.firstGoals[i] || data.curGoals[i];
                break;
            case "Level2":
                if (data.curTime < data.secondTime || data.secondTime == 0)
                    data.secondTime = data.curTime;
                for (int i = 0; i < 5; i++)
                    data.secondGoals[i] = data.secondGoals[i] || data.curGoals[i];
                break;
            case "Level3":
                if (data.curTime < data.thirdTime || data.secondTime == 0)
                    data.thirdTime = data.curTime;
                for (int i = 0; i < 5; i++)
                    data.thirdGoals[i] = data.thirdGoals[i] || data.curGoals[i];
                break;
        }
    }
#if UNITY_ANDROID && !UNITY_EDITOR
    private void OnApplicationPause(bool pause)
    {
        if (pause) File.WriteAllText(path, JsonUtility.ToJson(data));
    }
#endif
    private void OnApplicationQuit()
    {
        File.WriteAllText(path, JsonUtility.ToJson(data));
    }
    public void SaveData()
    {
        File.WriteAllText(path, JsonUtility.ToJson(data));
    }

    public delegate void FloatFunc(float a);
    public delegate void BoolFunc(bool a);
    public void Invoke(FloatFunc f, float value, float time)
    {
        StartCoroutine(StartFloat(f, value, time));
    }
    IEnumerator StartFloat(FloatFunc f, float value, float time)
    {
        yield return new WaitForSeconds(time);
        f(value);
        Destroy(gameObject);
    }
    public void Invoke(BoolFunc f, bool value, float time)
    {
        StartCoroutine(StartBool(f, value, time));
    }
    IEnumerator StartBool(BoolFunc f, bool value, float time)
    {
        yield return new WaitForSeconds(time);
        f(value);
    }
}
