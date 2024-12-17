using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterWaveScript : MonoBehaviour
{
    public ComputeShader waveCompute; 
    public Material waterMaterial;   
    public RenderTexture currentState; 
    public Vector2Int resolution = new Vector2Int(256, 256); 

    [Header("Wave Properties")]
    public float waveHeight = 1.0f; 
    public float waveSpeed = 1.0f;  

    [Header("Color Properties")]
    public Color waveColor = Color.blue; 

    void Start()
    {
        InitializeTexture(ref currentState);

        waterMaterial.SetTexture("_MainTex", currentState);
    }

    void InitializeTexture(ref RenderTexture tex)
    {
        tex = new RenderTexture(resolution.x, resolution.y, 0, RenderTextureFormat.ARGBFloat);
        tex.enableRandomWrite = true;
        tex.Create();
    }

    void Update()
    {

        waveCompute.SetTexture(0, "CurrentState", currentState);
        waveCompute.SetFloat("WaveHeight", waveHeight);
        waveCompute.SetFloat("WaveSpeed", waveSpeed);
        waveCompute.SetFloats("Resolution", resolution.x, resolution.y);
        waveCompute.SetFloat("Time", Time.time);

        int threadGroupsX = Mathf.CeilToInt(resolution.x / 8.0f);
        int threadGroupsY = Mathf.CeilToInt(resolution.y / 8.0f);
        waveCompute.Dispatch(0, threadGroupsX, threadGroupsY, 1);

        waterMaterial.SetColor("_WaveColor", waveColor);
    }
}
