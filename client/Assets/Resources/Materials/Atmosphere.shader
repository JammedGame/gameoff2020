Shader "Moonshot/Atmosphere"
{
    Properties
    {
        _Tint ("Tint", Color) = (1,1,1,1)
        _FrontAlpha ("FrontAlpha", Range(0,1)) = 0.5
        _FrontAlpha ("BackAlpha", Range(0,1)) = 0.5
        _CameraPower ("CameraPower", Range(0,1)) = 0.5
        _CameraMultiply ("CameraMultiply", Range(0,1)) = 0.5
        _CameraSubtract ("CameraSubtract", Range(0,1)) = 0.0
        _LightPower ("LightPower", Range(0,1)) = 0.5
        _LightMultiply ("LightMultiply", Range(0,1)) = 0.5
        _LightSubtract ("LightSubtract", Range(0,1)) = 0.0
        _EdgeFeather ("EdgeFeather", Range(0,1)) = 0.5
        _ApproachDistance ("ApproachDistance", Float) = 100.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "RenderPipeline" = "UniversalRenderPipeline" }
        LOD 100

        Pass
        {
            Tags { "LightMode"="UniversalForward" }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "MoonshotUtil.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
				float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD0;
				float3 normal : TEXCOORD1;
            };

            float4 _Tint;
            float _FrontAlpha;
            float _BackAlpha;
            float _CameraPower;
            float _CameraMultiply;
            float _CameraSubtract;
            float _LightPower;
            float _LightMultiply;
            float _LightSubtract;
            float _EdgeFeather;
            float _ApproachDistance;

            v2f vert (appdata v)
            {
                v2f o;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = normalize(v.normal);
                return o;
            }

            float4 frag (v2f i, bool isFrontFace:SV_IsFrontFace) : SV_Target
            {
                i.normal = normalize(i.normal);

                float3 viewDir = normalize(WorldSpaceViewDir(float4(i.worldPos, 1)));
                float3 featherNormal = isFrontFace ? i.normal : -i.normal;
                float featherPower = 1.0 / _EdgeFeather;
                float featherFresnel = fresnelEffect(featherNormal, viewDir, featherPower);
                float featherAlpha = isFrontFace ? _FrontAlpha : _BackAlpha;
                float edgeFeather = saturate(min(1 - featherFresnel, featherAlpha));
                edgeFeather = 1;

                float3 lightDirection = normalize(WorldSpaceLightDir(float4(i.worldPos, 1)));
                float lightAlpha = remap(dot(lightDirection, i.normal), float2(-1, 1), float2(0, 1));
                float lightEffect = atmosphereParameters(lightAlpha, _LightPower, _LightMultiply, _LightSubtract);

                float3 cameraDistance = distance(_WorldSpaceCameraPos, i.worldPos);
                float approachEffect = saturate(remap(cameraDistance, float2(0, _ApproachDistance), float2(0, 1)));

                float4 col = float4(_Tint.xyz, edgeFeather * lightEffect * approachEffect);
                return col;
            }
            ENDCG
        }
    }
}
