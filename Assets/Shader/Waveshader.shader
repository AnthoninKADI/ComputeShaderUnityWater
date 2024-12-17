Shader "Unlit/Waveshader"
{
        Properties
            {
                _MainTex ("Wave Texture", 2D) = "white" {}
                _WaveHeight ("Wave Height Multiplier", Float) = 0.5
        
                // Couleurs exposées dans l'éditeur Unity
                _ColorLowest ("Color at Lowest Point", Color) = (0, 0, 0.5, 1) // Bleu foncé
                _ColorMid ("Color at Mid Point", Color) = (0, 0.5, 1, 1)     // Bleu clair
                _ColorPeak ("Color at Highest Point", Color) = (1, 1, 1, 1)   // Blanc
            }
            SubShader
            {
                Tags { "RenderType"="Opaque" }
                LOD 100
        
                Pass
                {
                    CGPROGRAM
                    #pragma vertex vert
                    #pragma fragment frag
        
                    sampler2D _MainTex;
                    float _WaveHeight;
                    
                    fixed4 _ColorLowest;  // Couleur la plus foncée
                    fixed4 _ColorMid;     // Couleur intermédiaire
                    fixed4 _ColorPeak;    // Couleur la plus claire
        
                    struct appdata_t
                    {
                        float4 vertex : POSITION;
                        float2 uv : TEXCOORD0;
                    };
        
                    struct v2f
                    {
                        float2 uv : TEXCOORD0;
                        float4 vertex : SV_POSITION;
                    };
        
                    v2f vert (appdata_t v)
                    {
                        v2f o;
                        o.vertex = UnityObjectToClipPos(v.vertex);
                        o.uv = v.uv;
        
                        float waveHeight = tex2Dlod(_MainTex, float4(v.uv, 0, 0)).r * _WaveHeight;
                        o.vertex.y += waveHeight;
                        return o;
                    }
        
                    fixed4 frag (v2f i) : SV_Target
                    {
                        float waveHeight = tex2D(_MainTex, i.uv).r;
                        float invertedHeight = 1.0 - waveHeight;
        
                        fixed4 finalColor = lerp(_ColorLowest, _ColorMid, invertedHeight);
                        finalColor = lerp(finalColor, _ColorPeak, invertedHeight);
        
                        return finalColor;
                    }
                    ENDCG
                }
            }
}
