Shader "Custom/GlitchEffectCamera"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Intensity ("Glitch Intensity", Range(0, 1)) = 0
        _ColorIntensity ("Color Shift Intensity", Range(0, 0.1)) = 0.02
        _ScanLineJitter ("Scan Line Jitter", Range(0, 1)) = 0.1
        _VerticalJump ("Vertical Jump Amount", Range(0, 1)) = 0.1
        _HorizontalShake ("Horizontal Shake", Range(0, 1)) = 0.1
        _DisplacementMap ("Displacement Map", 2D) = "black" {}
    }

    SubShader
    {
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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _DisplacementMap;
            float _Intensity;
            float _ColorIntensity;
            float _ScanLineJitter;
            float _VerticalJump;
            float _HorizontalShake;

            // Функция случайного числа
            float nrand(float2 uv)
            {
                return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
            }

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Базовый цвет
                float2 uv = i.uv;

                // Эффект вертикального прыжка
                float jump = lerp(uv.y, frac(uv.y + _VerticalJump * _Intensity * nrand(float2(_Time.y, 2.0))), 
                    step(0.9999, nrand(floor(uv.y * 10.0 + _Time.y) * 0.5 * _Intensity)));

                // Дрожание строк
                float jitter = nrand(float2(_Time.y, uv.y)) * 2.0 - 1.0;
                jitter *= step(0.9999, nrand(float2(_Time.y, uv.y))) * _ScanLineJitter * _Intensity;

                // Горизонтальное дрожание
                float shake = (nrand(float2(_Time.y, 2.0)) - 0.5) * _HorizontalShake * _Intensity;

                // Смещение UV
                uv.y = jump;
                uv.x += jitter + shake;

                // Чтение из displacement map
                float2 disp = tex2D(_DisplacementMap, uv * 0.5 + _Time.xx * 0.5).xy;
                uv += (disp - 0.5) * 0.1 * _Intensity;

                // Разделение цветовых каналов
                fixed4 col;
                col.r = tex2D(_MainTex, uv + float2(_ColorIntensity * _Intensity, 0.0)).r;
                col.g = tex2D(_MainTex, uv).g;
                col.b = tex2D(_MainTex, uv - float2(_ColorIntensity * _Intensity, 0.0)).b;
                col.a = 1.0;

                // Добавление случайных пикселей
                if (nrand(uv + _Time.xx) > 0.998 - (0.01 * _Intensity))
                {
                    col.rgb = float3(nrand(uv), nrand(uv + 0.1), nrand(uv + 0.2));
                }

                return col;
            }
            ENDCG
        }
    }
}
