Shader "Moonshot/Planet"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Emission ("Emission", Color) = (0,0,0)
        _AtmosphereTint ("Atmosphere Tint", Color) = (1,1,1,1)
        _AtmosphereAlpha ("Atmosphere Alpha", Range(0,1)) = 0.5
        _AtmosphereHeight ("Atmosphere Height", Float) = 100
        _ApproachEffectOnEmission ("Approach Effect On Emission", Range(0,1)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        #include "MoonshotUtil.cginc"

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        half3 _Emission;
        fixed4 _AtmosphereTint;
        float _AtmosphereAlpha;
        float _AtmosphereHeight;
        float _ApproachEffectOnEmission;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

         void vert (inout appdata_full v, out Input o)
         {
            UNITY_INITIALIZE_OUTPUT(Input,o);
            o.worldPos = mul(unity_ObjectToWorld, v.vertex);
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            float3 cameraDistance = distance(_WorldSpaceCameraPos, IN.worldPos);
            float atmosphereApproach = saturate(remap(cameraDistance, float2(0, _AtmosphereHeight), float2(0, 1)));
            float atmosphereBlend = atmosphereApproach * _AtmosphereAlpha;
            o.Albedo = blendOverlay(c, _AtmosphereTint, atmosphereBlend);
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
            float atmosphereApproachEmission = pow(atmosphereApproach, _ApproachEffectOnEmission);
            float atmosphereBlendEmission = atmosphereApproachEmission * _AtmosphereAlpha;
            o.Emission = blendMultiply(_Emission, _AtmosphereTint, atmosphereBlendEmission);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
