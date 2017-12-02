// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Environment/Bottom" {
    Properties 
    {
        _Color_01 ("Color 01", Color) = (1, 1, 1, 1)
        _Color_02 ("Color 02", Color) = (1, 1, 1, 1)
        _Color_03 ("Color 03", Color) = (1, 1, 1, 1)
        [NoScaleOffset] _Caustics ("Caustics", 2D) = "black" {}
    }

    SubShader 
    {
        Tags { "RenderType" = "Opaque" }
        Pass 
        {
            CGPROGRAM

                #pragma vertex vert
                #pragma fragment frag
                #pragma target 2.0
                #pragma multi_compile_fog

                #include "UnityCG.cginc"
                
                float4 _Color_01; 
                float4 _Color_02;
                float4 _Color_03;
                sampler2D _Caustics;

                struct VertexInput 
                {
                    float4 pos : POSITION;
                    float4 color : COLOR;
                };

                struct VertexOutput 
                {
                    float4 pos : SV_POSITION;
                    float2 uv : TEXCOORD0;
                    float4 color : COLOR;
                    UNITY_FOG_COORDS(1)
                };
                
                VertexOutput vert (VertexInput v) 
                {
                    VertexOutput o = (VertexOutput)0;
                    o.pos = UnityObjectToClipPos(v.pos);
                    float4 posWorld = mul(unity_ObjectToWorld, v.pos);

                    o.uv =  posWorld.xz * 0.01;
                    UNITY_TRANSFER_FOG(o, o.pos);

                    o.color = v.color;

                    return o;
                }
                
                half4 frag(VertexOutput i) : COLOR 
                {
                    /*
                    half4 tex01 = tex2D(_Tex01, i.uv);
                    half4 tex02 = tex2D(_Tex02, i.uv);
                    half4 tex03 = tex2D(_Tex03, i.uv);
                    */

                    half4 tex_01 = _Color_01;
                    half4 tex_02 = _Color_02;
                    half4 tex_03 = _Color_03;

                    half4 caustics_01 = tex2D(_Caustics, 4 * i.uv + _Time.rr * .5);
                    half4 caustics_02 = tex2D(_Caustics, 2 * i.uv - _Time.rr * .25);
                    
                    float4 iter01 = lerp(tex_01, tex_02, i.color.r);
                    float4 iter02 = lerp(iter01, tex_03, i.color.g);

                    float4 final = saturate(iter02 + (1 - i.color.r) * .3 * caustics_01 * caustics_02) * i.color.a;
                    UNITY_APPLY_FOG(i.fogCoord, final);
                    return final;
                }
                
            ENDCG
        }
    }
}
