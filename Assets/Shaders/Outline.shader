Shader "Outlines/Outline"
{
    Properties
    {

        _Thickness ("Thickness", range(0,1)) = 1 
         _Color ("Color", Color) = (0,0,0,1) 
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline"}
        Cull Front
        Name "Outline"

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 normal : NORMAL;
            };

            struct v2f
            {
                float4 positionClipSpace: SV_POSITION;
            };

            float4 _Color;
            float _Thickness;

            v2f vert (appdata v)
            {
                v2f o;
                float3 extruded = v.vertex.xyz + normalize(v.normal) * _Thickness;
                o.positionClipSpace = UnityObjectToClipPos(extruded);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return _Color;
            }
            ENDCG
        }
    }
}
