Shader "Fabsolute/Cutout/Detailed" {
 Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _CutOut ("Cutout", Range (0, 1)) = 0
        _CutOutTop ("Cutout Top", Range (0, 1)) = 0
        _CutOutTopStart ("Cutout Top Start", Range (0, 1)) = 0
        _CutOutBottom ("Cutout Bottom", Range (0, 1)) = 0
        _CutOutBottomStart ("Cutout Bottom Start", Range (0, 1)) = 0
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
            fixed _CutOut;
            fixed _CutOutTop;
            fixed _CutOutBottom;
            fixed _CutOutBottomStart;
            fixed _CutOutTopStart;

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
                fixed alpha = tex2D(_MainTex,coord).a;
                if (alpha > _CutOut){
                        if (coord.y<(_CutOutBottom/2) && 1-coord.x > _CutOutBottomStart){
                            return 0;
                        }
                        else if (coord.y >(1-(_CutOutTop/2) )&& 1-coord.x > _CutOutTopStart)
                        {
                            return 0;
                        }
                        return In.color;
                }
                return 0;
            }
        ENDCG
        }
    }
}
