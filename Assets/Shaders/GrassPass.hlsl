//reference: https://github.com/AJTech2002/Grass-Shader-Tutorial/blob/master/Unity%20Grass%20Tutorial%20-%20Part%201/Assets/Shaders/GrassPass.hlsl
struct Attributes
{
    float4 positionOS   : POSITION;
    float3 normalOS     : NORMAL;
    float4 tangentOS    : TANGENT;
    float2 uv           : TEXCOORD0;
    float2 uvLM         : TEXCOORD1;
    float4 color : COLOR; 
    UNITY_VERTEX_INPUT_INSTANCE_ID
};


struct Varyings
{
    float3 normalOS : NORMAL;
    float2 uv                       : TEXCOORD0;
    float2 uvLM                     : TEXCOORD1;
    float4 positionWSAndFogFactor   : TEXCOORD2; // xyz: positionWS, w: vertex fog factor
    half3  normalWS                 : TEXCOORD3;
    half3 tangentWS                 : TEXCOORD4;
    float4 positionOS : TEXCOORD5;

    float4 color : COLOR;

    #if _NORMALMAP
    half3 bitangentWS               : TEXCOORD5;
    #endif

    #ifdef _MAIN_LIGHT_SHADOWS
    float4 shadowCoord              : TEXCOORD6; // compute shadow coord per-vertex for the main light
    #endif
    float4 positionCS               : SV_POSITION;
};

//Properties
float _Height;
float _Base;
float4 _Tint;
float _ReceiveShadow;

float _LightPower;
float _TPower;

//what we require from the gru to define vertex data
//VERTEX PASS
Varyings LitPassVertex(Attributes input)
{
    Varyings output;

    output.color = input.color;

    VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS);
    VertexNormalInputs vertexNormalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);

    float fogFactor = ComputeFogFactor(vertexInput.positionCS.z);

    output.uv = TRANSFORM_TEX(input.uv, _BaseMap);

    output.uvLM = input.uvLM.xy * unity_LightmapST.xy + unity_LightmapST.zw;


    output.positionWSAndFogFactor = float4(vertexInput.positionWS, fogFactor);
    output.positionCS = vertexInput.positionCS;
    output.positionOS = input.positionOS;
    
    output.normalWS = vertexNormalInput.normalWS;
    output.tangentWS = vertexNormalInput.tangentWS;

    //bittangent means the cross direction of tangent, make sure the grass is always pointed in the outward direction
    #ifdef _NORMALMAP
        output.bitangentWS = vertexNormalInput.bitangentWS;
    #endif

    #ifdef _MAIN_LIGHT_SHADOWS
        output.shadowCoord = GetShadowCoord(vertexInput);
    #endif

    return output;
}

//rotation matrix
float3x3 RotY(float ang)
{
	return float3x3
	(
		cos(ang), 0, sin(ang),
		0,1,0,
		-sin(ang),0,cos(ang)
	);
}

//rotate the top of the grass, for the wind
float3x3 RotX(float ang)
{
	return float3x3
	(
		1,0,0,
		0,cos(ang),-sin(ang),
		0,sin(ang),cos(ang)
	);
}

float3x3 RotZ(float ang)
{
	return float3x3
	(
		cos(ang), -sin(ang),0,
		sin(ang), cos(ang),0,
		0,0,1
	);
}


float rand(float3 co)
{
	return frac(sin(dot(co.xyz, float3(12.9898, 78.233, 53.539))) * 43758.5453);
}

sampler2D _WindDistortionMap;
float3 _WindFrequency;
float _WindStrength;

float _MinHeight;
float _MaxHeight;

//bring vertex data
 [maxvertexcount(6)] //how many verteies we need
 void LitPassGeom(triangle Varyings input[3], inout TriangleStream<Varyings> outStream)
 {
	 //allow the character to walk through without the grass, add condition in geometry shader, to restrict where the grass is drawn
	 if (input[0].color.g < 0.1f && input[0].color.r > 0.9f)
		 return;

	 //get uv position 
	 float2 uv = (input[0].positionOS.xy * _Time.xy * _WindFrequency);

	 //get the sample from wind distortion map at uv position, multiple wind strength to affact how much it affacts
	 float4 windSample = tex2Dlod(_WindDistortionMap, float4(uv, 0, 0)) * _WindStrength;

	 //rotate normal map along the z axis first, using the sample color to control rotation along the z axis
	 float3 rotatedNormalZ = mul(input[0].normalWS, RotZ(windSample.x));

	 //rotate z first then it rotate along x axis
	 float3 rotatedNormal = mul(rotatedNormalZ, RotX(windSample.y));

	 //random height effect
	 float randomHeight = rand(input[0].positionWSAndFogFactor.xyz * 2.238293f);

     /*outStream.Append(input[0]);
     outStream.Append(input[1]);
     outStream.Append(input[2]);
     outStream.RestartStrip();*/

	 float3 basePos = (input[0].positionWSAndFogFactor.xyz + input[1].positionWSAndFogFactor.xyz + input[2].positionWSAndFogFactor.xyz) / 3;

	 //create four verteies
	 Varyings o = input[0];

	 float3 rotatedTangent = normalize(mul(o.tangentWS, RotY(rand(o.positionWSAndFogFactor.xyz) * 90)));

	 //left
	 float3 oPos = (basePos - rotatedTangent * _Base);
	 o.positionCS = TransformWorldToHClip(oPos);

	 Varyings o2 = input[0];
	 //right
	 float3 o2Pos = (basePos + rotatedTangent *_Base);
	 o2.positionCS = TransformWorldToHClip(o2Pos);

	 Varyings o3 = input[0];
	 //top right
	 float3 o3Pos = (basePos + rotatedTangent * _Base + rotatedNormal * clamp(_Height * randomHeight, _MinHeight, _MaxHeight));
	 o3.positionCS = TransformWorldToHClip(o3Pos);

	 Varyings o4 = input[0];
	 //top left
	 float3 o4Pos = (basePos - rotatedTangent * _Base + rotatedNormal * clamp(_Height * randomHeight, _MinHeight, _MaxHeight));
	 o4.positionCS = TransformWorldToHClip(o4Pos);

	 float3 newNormal = mul(rotatedTangent, RotY(PI / 2));

	 //set up uv
	 o4.uv = TRANSFORM_TEX(float2(0, 1), _BaseMap);
	 o3.uv = TRANSFORM_TEX(float2(1, 1), _BaseMap);
	 o2.uv = TRANSFORM_TEX(float2(1, 0), _BaseMap);
	 o.uv = TRANSFORM_TEX(float2(0, 0), _BaseMap);

	 //set the normal
	 o.normalWS = newNormal;
	 o2.normalWS = newNormal;
	 o3.normalWS = newNormal;
	 o4.normalWS = newNormal;

	 outStream.Append(o4);
	 outStream.Append(o3);
	 outStream.Append(o);

	 outStream.RestartStrip();

	 outStream.Append(o3);
	 outStream.Append(o2);
	 outStream.Append(o);

	 outStream.RestartStrip();




 }

// [maxvertexcount(6)] //how many verteies we need
// void LitPassGeom(triangle Varyings input[3], inout LineStream<Varyings> outStream)
// {
//     outStream.Append(input[0]);
//     outStream.Append(input[1]);
//     outStream.RestartStrip();
 
// }

// [maxvertexcount(6)] //how many verteies we need
// void LitPassGeom(triangle Varyings input[3], inout PointStream<Varyings> outStream)
// {
//     outStream.Append(input[0]);
//     outStream.Append(input[1]);
//     outStream.RestartStrip();
//       outStream.Append(input[2]);
//     outStream.RestartStrip();
 
// }

float4 TransformWorldToShadowCoords(float3 positionWS)
{
	half cascadeIndex = ComputeCascadeIndex(positionWS);
	//provide a matrix that allows to transform position world space into shadow coordinate space
	return mul(_MainLightWorldToShadow[cascadeIndex], float4(positionWS, 1.0));
}

float _ShadowPower;
float _AlphaCutoff;
float4 _Darker;

half4 LitPassFragment(Varyings input, bool vf : SV_IsFrontFace) : SV_Target
{
	//half3 to store normal world space
	half3 normalWS = input.normalWS;

	normalWS = normalize(normalWS);

	//look at back face
	if (vf == false)
	{
		//flip normal map
		normalWS = -normalWS;

	}
	//store position
	float3 positionWS = input.positionWSAndFogFactor.xyz;

	half3 color = (0, 0, 0);

	//set up light
	Light mainLight;

	//the shadow coordinates to initialize main light
	float4 shadowCoord = TransformWorldToShadowCoords(positionWS);

	mainLight = GetMainLight(shadowCoord);

	float3 normalLight = LightingLambert(mainLight.color, mainLight.direction, normalWS) * _LightPower;
	//fix the dark shadow in the back of light
	float3 inverseNormalLight = LightingLambert(mainLight.color, mainLight.direction, -normalWS) * _TPower;

	color = _Tint + normalLight + inverseNormalLight;

	//add shadow color
	color = lerp(color, _Darker, 1 - input.uv.y);

	//add shadow, blend the top color to the bottom color when there is shadow at that region
	color = lerp(_Darker, color, clamp(mainLight.shadowAttenuation + _ShadowPower, 0, 1));

	//mix fog color
	float fogFactor = input.positionWSAndFogFactor.w;

	color = MixFog(color, fogFactor);

	//bring uv in, getting color from the value
	float a = _BaseMap.Sample(sampler_BaseMap, input.uv).a;

	//if the alpha is below a certain range, don't render it, then can see what's behind it
	clip(a - _AlphaCutoff);

	//(1,1,1,1) white color, (0,0,0,0) is black
    return half4(color,1); 
}