// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Anime/Anime_V1"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ShadedTex ("Shaded Texture", 2D) = "white" {}
        _ILMTex ("ILM Texture", 2D) = "white" {}
        _ShadingFactor ("Shading Factor", Range(0,1)) = 0
        _Brightness ("Brightness", Range(0,1)) = 1
        _AmbientLight ("Ambient Light", Range(0,1)) = 0
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

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 vertexNormal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 fragmentNormal : NORMAL;

            };

            sampler2D _MainTex;
            sampler2D _ShadedTex;
            sampler2D _ILMTex;
            float4 _MainTex_ST;
            float _ShadingFactor;
            float _Brightness;
            float _AmbientLight;

            float remap(float value) {
                float range = _Brightness - _AmbientLight;
                float newValue;

                if (range < 0) {
                    range = 0;
                }

                return _AmbientLight + value*range;
            }

            float calculateLightValue(v2f i) {
                float3 sunlight = _WorldSpaceLightPos0.xyz;
                float3 secondDirectionalLight = float3(0,1,0);

                float dotProduct_0 = clamp(dot(normalize(i.fragmentNormal), normalize(sunlight)), 0, 1);
                float dotProduct_1 = clamp(dot(normalize(i.fragmentNormal), normalize(secondDirectionalLight)), 0, 1);

                float totalLight = dotProduct_0 + dotProduct_1;

                totalLight = remap(totalLight);

                totalLight += _ShadingFactor;

                return totalLight;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                o.fragmentNormal = mul( unity_ObjectToWorld, v.vertexNormal);

                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the textures
                fixed4 baseColor = tex2D(_MainTex, i.uv);
                fixed4 shadedColor = tex2D(_ShadedTex, i.uv);
                fixed4 ILMColor = tex2D(_ILMTex, i.uv);
        
                fixed4 result;

                float lightValue = calculateLightValue(i);
                
                if (lightValue < 1-ILMColor.g) {
                    result = shadedColor;
                } else {
                    result = baseColor;
                }

                if (1-lightValue < ILMColor.b) {
                     result += ILMColor.r;
                 }

                return result;

               // return baseColor;
            }
            ENDCG
        }
    }
}
