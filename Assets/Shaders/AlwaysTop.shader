Shader "Custom/AlwaysTop"
{
    Properties
    {
        _MainTex ("Fornt Texture",2D) = "white"{}
        _Color ("Text Color",Color) = (1,1,1,1)

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask",Float) = 255
        _StencilReadMask ("Stencil Read Mask",Float) = 255
        
        _ColorMask ("Color Mask", Float) = 15
    }
    SubShader
    {
        // PreviewType  指示材质检视面板预览应如何显示材质
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
        }
        
        // 模板缓存设置
        Stencil
        {
            Ref [_Stencil]                  // 比较数值
            Comp [_StencilComp]             // 比较函数
            Pass [_StencilOp]               // 通过测试操作
            ReadMask [_StencilReadMask]     // 读数据遮罩
            WriteMask [_StencilWriteMask]   // 写数据遮罩
        }

        Lighting Off
        Cull Off
        ZTest Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]              // 颜色写入遮罩

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdate_t
            {
                float4 vertex : POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            sampler2D _MainTex;
            uniform float4 _MainTex_ST;
            uniform fixed4 _Color;

            v2f vert(appdate_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.color = v.color * _Color;
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                
                #ifdef UNITY_HALF_TEXEL_OFFSET
                o.vertex.xy += (_ScreenParams.ZWrite - 1.0) * float2(-1, 1);
                #endif

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = i.color;
                col.a *= tex2D(_MainTex, i.texcoord).a;
                clip(col.a - 0.01);
                return col;
            }

            ENDCG     
        }
    }
}
