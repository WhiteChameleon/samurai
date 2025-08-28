Shader "Custom/GlitchEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _GlitchAmount ("Glitch Amount", Range(0, 1)) = 0
        _GlitchSpeed ("Glitch Speed", Float) = 1
        _ColorShift ("Color Shift", Range(0, 0.1)) = 0.01
        _ScanLineJitter ("Scan Line Jitter", Range(0, 1)) = 0.1
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

            // ��������� ������� rand �� � �������������
            float rand(float2 co)
            {
                return frac(sin(dot(co.xy, float2(12.9898, 78.233))) * 43758.5453);
            }

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
            float _GlitchAmount;
            float _GlitchSpeed;
            float _ColorShift;
            float _ScanLineJitter;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // ������� ����
                fixed4 col = tex2D(_MainTex, i.uv);
                
                // ������ ������ ������ ����� _GlitchAmount > 0
                if (_GlitchAmount > 0)
                {
                    // ��������� �������� ��� ������� "��������"
                    float jitter = rand(float2(_Time.y, i.uv.y)) * 2 - 1;
                    jitter *= _ScanLineJitter * _GlitchAmount;
                    
                    // ����� ����� (scan line jitter)
                    float lineJitter = step(0.999, frac(i.uv.y * 10 + _Time.y * _GlitchSpeed)) * jitter;
                    
                    // ����� UV ���������
                    float2 uv = i.uv + float2(lineJitter, 0);
                    
                    // �������� ������ � ������� ����������
                    col.r = tex2D(_MainTex, uv + float2(_ColorShift * _GlitchAmount, 0)).r;
                    col.g = tex2D(_MainTex, uv).g;
                    col.b = tex2D(_MainTex, uv - float2(_ColorShift * _GlitchAmount, 0)).b;
                    
                    // ��������� ��������� �������
                    if (rand(float2(_Time.y, i.uv.x)) > 0.995 * (1 - _GlitchAmount))
                    {
                        col = fixed4(rand(i.uv + _Time.xx), rand(i.uv + _Time.yy), rand(i.uv + _Time.zz), 1);
                    }
                }
                
                return col;
            }
            ENDCG
        }
    }
}
