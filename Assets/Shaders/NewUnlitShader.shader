Shader "Unlit/NewUnlitShader"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _ShadowColor ("Shadow Color", Color) = (0, 0, 0, 0.5)
        _LightPos ("Light Position", Vector) = (0, 0, 0, 0)
        _OffsetStrength ("Offset Strength", Float) = 0.1
    }
    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

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
            float4 _MainTex_ST;
            float4 _ShadowColor;
            float3 _LightPos;
            float _OffsetStrength;

            v2f vert (appdata v)
            {
                v2f o;
                
                // Расчёт направления от света к игроку
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                float3 lightDir = normalize(_LightPos - worldPos);
                
                // Смещение UV
                float2 offset = lightDir.xy * _OffsetStrength;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex) + offset;
                
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                col.rgb = _ShadowColor.rgb; // Перезаписываем цвет тени
                col.a *= _ShadowColor.a;    // Умножаем альфу
                return col;
            }
            ENDCG
        }
    }
}
