// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Environment/Rocks" 
{
    
	Properties 
    {
        [NoScaleOffset] _MainTex ("Texture 01", 2D) = "white" {}
        [NoScaleOffset] _Caustics ("Caustics", 2D) = "black" {}
        _Color_01 ("Color 01", Color) = (0.443, 0.584, 0.161, 1)
        _Color_02 ("Color 02", Color) = (0.565, 0.565, 0.176, 1)
    }
    SubShader 
    {
        Tags {"RenderType" = "Opaque"}
        Pass 
        {
            Name "FORWARD"
            Tags {"LightMode" = "ForwardBase"}
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            uniform sampler2D _MainTex;
            uniform sampler2D _Caustics;
            uniform fixed4 _Color_01;
            uniform fixed4 _Color_02;

           	struct VertexInput 
            {
                float4 pos : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct VertexOutput 
            {
                float4 pos : SV_POSITION;
                float3 normal : NORMAL;
                float4 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
            };

            VertexOutput vert(VertexInput v) 
            {
                VertexOutput o = (VertexOutput)0;
                o.normal = v.normal;
                o.uv.xy = v.uv;
                float4 posWorld = mul(unity_ObjectToWorld, v.pos);
                o.uv.zw = posWorld.xz * 0.01;
                o.pos = UnityObjectToClipPos(v.pos);
                UNITY_TRANSFER_FOG(o, o.pos);

                
                return o;
            }
		
			fixed4 frag(VertexOutput i) : COLOR 
            {
                float4 tex = tex2D(_MainTex, i.uv.xy);
                    half4 caustics_01 = tex2D(_Caustics, 8 * i.uv.xy + _Time.rr * .5);
                    half4 caustics_02 = tex2D(_Caustics, 4 * i.uv.xy - _Time.rr * .25);

                float3 dir = normalize(float3(0, -1, -.5));
                float4 d = dot(i.normal, dir);
                float4 col = lerp(_Color_01, _Color_02, d);
                fixed4 final = saturate(col + .3 * caustics_01 * caustics_02);
                UNITY_APPLY_FOG(i.fogCoord, final);
                
                return final;
            }
            ENDCG
        }
    }
}