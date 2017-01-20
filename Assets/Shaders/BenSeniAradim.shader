Shader "Custom/BenSeniAradim" {
 Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _CutOff ("Cutoff", Range (0, 1)) = 0.5
        _CutOffTop ("Cutoff Top", Range (0, 1)) = 0
        _CutOffBottom ("Cutoff Bottom", Range (0, 1)) = 0
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

        Cull Off
        Lighting Off
        ZWrite Off
        Fog { Mode Off }
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

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
                half2 texcoord  : TEXCOORD0;
            };

            fixed4 _Color;
            fixed _CutOff;
            fixed _CutOffTop;
            fixed _CutOffBottom;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color;
                return OUT;
            }

            sampler2D _MainTex;

            fixed4 frag(v2f In) : COLOR
            {
                half2 coord = In.texcoord;
                if (coord.y > _CutOff){
                    fixed alpha = tex2D(_MainTex,coord).a;
                    if (alpha>0){
                        return In.color;
                    }
                }
                return 0;
            }
        ENDCG
        }
    }
}
