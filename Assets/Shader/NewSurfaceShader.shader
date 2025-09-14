Shader "UI/CosmicGradient"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        
        [Header(Cosmic Effects)]
        _AnimationSpeed ("Animation Speed", Range(0, 3)) = 0.5
        _GradientIntensity ("Gradient Intensity", Range(0, 2)) = 1.0
        _GradientSoftness ("Gradient Softness", Range(0.1, 2)) = 0.8
        _ColorFlow ("Color Flow", Range(0, 2)) = 0.3
        _GlowIntensity ("Glow Intensity", Range(0, 1)) = 0.4
        
        [Header(Colors)]
        _CenterColor ("Center Color", Color) = (0.05, 0.05, 0.08, 1)
        _EdgeColor1 ("Edge Color 1", Color) = (0.4, 0.1, 0.6, 1)
        _EdgeColor2 ("Edge Color 2", Color) = (0.6, 0.2, 0.8, 1)
        _AccentColor ("Accent Color", Color) = (0.2, 0.6, 0.9, 1)
        
        [Header(UI Settings)]
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
        
        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            Name "Default"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                float2 screenPos : TEXCOORD2;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            float4 _MainTex_ST;
            
            float _AnimationSpeed;
            float _GradientIntensity;
            float _GradientSoftness;
            float _ColorFlow;
            float _GlowIntensity;
            
            fixed4 _CenterColor;
            fixed4 _EdgeColor1;
            fixed4 _EdgeColor2;
            fixed4 _AccentColor;

            v2f vert(appdata_t v)
            {
                v2f OUT;
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
                OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                OUT.color = v.color * _Color;
                OUT.screenPos = ComputeScreenPos(OUT.vertex);
                return OUT;
            }

            // 해시 함수
            float hash(float2 p)
            {
                return frac(sin(dot(p, float2(127.1, 311.7))) * 43758.5453);
            }
            
            // 부드러운 노이즈 함수
            float smoothNoise(float2 uv)
            {
                float2 i = floor(uv);
                float2 f = frac(uv);
                f = f * f * (3.0 - 2.0 * f); // 부드러운 보간
                
                float a = hash(i);
                float b = hash(i + float2(1.0, 0.0));
                float c = hash(i + float2(0.0, 1.0));
                float d = hash(i + float2(1.0, 1.0));
                
                return lerp(lerp(a, b, f.x), lerp(c, d, f.x), f.y);
            }

            // 부드러운 그라데이션 함수
            float softGradient(float distance, float softness)
            {
                return 1.0 - smoothstep(0.0, softness, distance);
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;

                #ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif

                float2 uv = IN.texcoord;
                float2 center = float2(0.5, 0.5);
                float distanceFromCenter = length(uv - center);
                float time = _Time.y * _AnimationSpeed;
                
                // 부드러운 방사형 그라데이션
                float radialGradient = softGradient(distanceFromCenter, _GradientSoftness);
                
                // 미묘한 색상 변화
                float colorPhase = sin(time + distanceFromCenter * 3.0) * 0.5 + 0.5;
                float colorShift = sin(time * 0.7 + atan2(uv.y - 0.5, uv.x - 0.5)) * 0.3 + 0.5;
                
                // 부드러운 노이즈로 자연스러운 변화
                float noiseValue = smoothNoise(uv * 8.0 + time * 0.1) * 0.2;
                
                // 색상 믹싱
                fixed3 edge1 = lerp(_EdgeColor1.rgb, _EdgeColor2.rgb, colorPhase);
                fixed3 edge2 = lerp(_EdgeColor2.rgb, _AccentColor.rgb, colorShift);
                fixed3 edgeColor = lerp(edge1, edge2, noiseValue + 0.5);
                
                // 중앙에서 가장자리로의 부드러운 전환
                float gradientFactor = pow(radialGradient, 1.5);
                gradientFactor = saturate(gradientFactor + noiseValue * _ColorFlow);
                
                // 글로우 효과
                float glow = exp(-distanceFromCenter * 4.0) * _GlowIntensity;
                
                // 최종 색상 계산
                fixed3 finalColor = lerp(edgeColor, _CenterColor.rgb, gradientFactor);
                finalColor += glow * _AccentColor.rgb;
                
                // 원본 텍스처와 자연스럽게 블렌딩
                color.rgb = lerp(color.rgb, finalColor, _GradientIntensity);

                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif

                return color;
            }
            ENDCG
        }
    }
}