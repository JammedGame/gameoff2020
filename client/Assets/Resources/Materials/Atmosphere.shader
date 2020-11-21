Shader "Moonshot/Atmosphere"
{
    Properties
    {
        _Tint ("Tint", Color) = (1,1,1,1)
        _FrontAlpha ("FrontAlpha", Range(0,1)) = 0.5
        _BackAlpha ("BackAlpha", Range(0,1)) = 0.5
        _LightPower ("LightPower", Range(0,1)) = 0.5
        _LightMultiply ("LightMultiply", Range(0,1)) = 0.5
        _LightSubtract ("LightSubtract", Range(0,1)) = 0.0
        _EdgeFeather ("EdgeFeather", Float) = 1.0
        _ApproachDistance ("ApproachDistance", Float) = 100.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Tags { "LightMode"="ForwardBase" }
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
                float3 worldNormal : TEXCOORD1;
                float3 viewDir : TEXCOORD2;
                float3 lightDir : TEXCOORD3;
            };

            float4 _Tint;
            float _FrontAlpha;
            float _BackAlpha;
            float _LightPower;
            float _LightMultiply;
            float _LightSubtract;
            float _EdgeFeather;
            float _ApproachDistance;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.worldNormal = normalize(mul(unity_ObjectToWorld, float4(v.normal, 0.0)));
                o.viewDir = normalize(WorldSpaceViewDir(v.vertex));
                o.lightDir = normalize(WorldSpaceLightDir(float4(o.worldPos, 0.0)));
                return o;
            }

            float4 frag (v2f i, bool isFrontFace:SV_IsFrontFace) : SV_Target
            {
                float3 featherNormal = isFrontFace ? i.worldNormal : -i.worldNormal;
                float featherPower = 1.0 / _EdgeFeather;
                float featherFresnel = fresnelEffect(featherNormal, i.viewDir, featherPower);
                float featherAlpha = isFrontFace ? _FrontAlpha : _BackAlpha;
                float edgeFeather = saturate(min(1.0 - featherFresnel, featherAlpha));

                float lightAlpha = remap(dot(i.lightDir, i.worldNormal), float2(-1.0, 1.0), float2(0.0, 1.0));
                float lightEffect = atmosphereParameters(lightAlpha, _LightPower, _LightMultiply, _LightSubtract);

                float3 cameraDistance = distance(_WorldSpaceCameraPos, i.worldPos);
                float approachEffect = saturate(remap(cameraDistance, float2(0.0, _ApproachDistance), float2(0.0, 1.0)));

                float4 col = float4(_Tint.xyz, edgeFeather * lightEffect * approachEffect);
                return col;
            }
            ENDCG
        }
    }
}
