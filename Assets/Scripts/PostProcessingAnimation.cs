using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingAnimation : MonoBehaviour
{
    Animation anim;
    public float bloom;
    public float saturation;
    public float temperature = -35.0f;
    public float tint;
    public float vignette = 0.2f;
    public float focus = 20.0f;
    Bloom bloomLayer;
    ColorGrading colorGradingLayer;
    Vignette vignetteLayer;
    DepthOfField depthOfFieldLayer;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animation>();
        PostProcessVolume volume = gameObject.GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out bloomLayer);
        volume.profile.TryGetSettings(out colorGradingLayer);
        volume.profile.TryGetSettings(out vignetteLayer);
        volume.profile.TryGetSettings(out depthOfFieldLayer);
    }

    // Update is called once per frame
    void Update()
    {
        if (anim.isPlaying)
        {
            bloomLayer.intensity.value = bloom;
            colorGradingLayer.saturation.value = saturation;
            colorGradingLayer.temperature.value = temperature;
            colorGradingLayer.tint.value = tint;
            vignetteLayer.intensity.value = vignette;
            depthOfFieldLayer.focusDistance.value = focus;
        }
    }

    public void GameOver()
    {
        bloomLayer.active = true;
        colorGradingLayer.active = true;
        depthOfFieldLayer.active = true;
        anim.Play();
        Invoke("WastedSign", 2.25f);
    }

    void WastedSign()
    {
        var blackBox = new GameObject();
        CreateQuadFrom(blackBox, GetComponent<Camera>().orthographicSize * GetComponent<Camera>().aspect * 2, GetComponent<Camera>().orthographicSize * 0.5f);
        blackBox.transform.position = transform.position + new Vector3(-GetComponent<Camera>().orthographicSize * GetComponent<Camera>().aspect, -GetComponent<Camera>().orthographicSize * 0.25f, GetComponent<Camera>().nearClipPlane + 0.01f);
        GameObject.Find("Canvas").transform.Find("Wasted").gameObject.SetActive(true);
    }

    void CreateQuadFrom(GameObject quad, float width = 1, float height = 1)
    {
        MeshRenderer meshRenderer = quad.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));

        MeshFilter meshFilter = quad.AddComponent<MeshFilter>();

        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(0, 0, 0),
            new Vector3(width, 0, 0),
            new Vector3(0, height, 0),
            new Vector3(width, height, 0)
        };
        mesh.vertices = vertices;

        int[] tris = new int[6]
        {
            // lower left triangle
            0, 2, 1,
            // upper right triangle
            2, 3, 1
        };
        mesh.triangles = tris;

        Vector3[] normals = new Vector3[4]
        {
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward
        };
        mesh.normals = normals;

        Vector2[] uv = new Vector2[4]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };
        mesh.uv = uv;

        meshFilter.mesh = mesh;
    }
}
