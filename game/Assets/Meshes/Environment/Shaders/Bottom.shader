// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Environment/Bottom" {
    Properties 
    {
        [NoScaleOffset] _Tex01 ("Texture 01", 2D) = "white" {}
        [NoScaleOffset] _Tex02("Texture 02", 2D) = "white" {}
        [NoScaleOffset] _Tex03 ("Texture 02", 2D) = "white" {}
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
                
                sampler2D _Tex01; 
                sampler2D _Tex02; 
                sampler2D _Tex03; 

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
                    half4 tex01 = tex2D(_Tex01, i.uv.xy);
                    half4 tex02 = tex2D(_Tex02, i.uv.xy);
                    half4 tex03 = tex2D(_Tex03, i.uv.xy);
                    
                    float4 iter01 = lerp(tex01, tex02, i.color.r);
                    float4 iter02 = lerp(iter01, tex03, i.color.g);

                    float4 final = iter02;
                    UNITY_APPLY_FOG(i.fogCoord, final);
                    return final;
                }
                
            ENDCG
        }
    }
}
