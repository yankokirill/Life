Shader "Custom/LifeDisplay"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _AliveColor ("Alive Color", Color) = (0,0,0,1)
        _DeadColor ("Dead Color", Color) = (1,1,1,1)
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

            sampler2D _MainTex;
            float4 _AliveColor;
            float4 _DeadColor;

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float value = tex2D(_MainTex, i.uv).r;
                return lerp(_DeadColor, _AliveColor, value);
            }
            ENDCG
        }
    }
}
