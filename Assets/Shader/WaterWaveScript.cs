using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterWaveScript : MonoBehaviour
{
    public ComputeShader waveCompute; // Shader de calcul
    public Material waterMaterial;   // Matériau pour appliquer la texture à un plane
    public RenderTexture currentState; // Texture pour représenter l'état actuel
    public Vector2Int resolution = new Vector2Int(256, 256); // Résolution de la texture

    [Header("Wave Properties")]
    public float waveHeight = 1.0f; // Hauteur des vagues
    public float waveSpeed = 1.0f;  // Vitesse de propagation

    [Header("Color Properties")]
    public Color waveColor = Color.blue; // Couleur des vagues

    void Start()
    {
        InitializeTexture(ref currentState);

        // Appliquer la texture au matériau
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
        // Envoyer les paramètres au shader
        waveCompute.SetTexture(0, "CurrentState", currentState);
        waveCompute.SetFloat("WaveHeight", waveHeight);
        waveCompute.SetFloat("WaveSpeed", waveSpeed);
        waveCompute.SetFloats("Resolution", resolution.x, resolution.y);
        waveCompute.SetFloat("Time", Time.time);

        // Lancer le calcul
        int threadGroupsX = Mathf.CeilToInt(resolution.x / 8.0f);
        int threadGroupsY = Mathf.CeilToInt(resolution.y / 8.0f);
        waveCompute.Dispatch(0, threadGroupsX, threadGroupsY, 1);

        // Appliquer la couleur au matériau
        waterMaterial.SetColor("_WaveColor", waveColor);
    }
}
