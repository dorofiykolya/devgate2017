// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Environment/Grass" 
{
    
	Properties 
    {
        _Color_01 ("Color 01", Color) = (0.443, 0.584, 0.161, 1)
        _Color_02 ("Color 02", Color) = (0.565, 0.565, 0.176, 1)
        [NoScaleOffset] _Caustics ("Caustics", 2D) = "black" {}
        _PrimaryFreq ("Primary Freqency", Float) = 2
        _primaryMotion ("Primary Motion Amp", Range(0, 500)) = 250
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

            uniform sampler2D _Caustics;
            uniform fixed4 _Color_01;
            uniform fixed4 _Color_02;
            uniform float _PrimaryFreq;
            uniform float _primaryMotion;


           
			struct VertexInput 
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct VertexOutput 
            {
                float4 pos : SV_POSITION;
                float4 uv : TEXCOORD0;
                float2 data : TEXCOORD1;
                float4 color : COLOR;
                UNITY_FOG_COORDS(1)
            };

            VertexOutput vert(VertexInput v) 
            {
                VertexOutput o = (VertexOutput)0;
                o.uv.xy = v.uv;
                float4 vertpos = mul(unity_ObjectToWorld, v.pos);
                
                // Animation
                    float pi = 3.14159;
                    float freq = _Time.r * _PrimaryFreq;
                    float phase;

                    phase = (pi * v.color.g)/2; 

                    fixed amp = v.uv.y * v.color.r * .01 * _primaryMotion;
                    v.pos.x += amp * sin(freq + phase);             //top rocking X  
                    v.pos.z += amp * cos(1.5 * (freq + phase));     //top rocking Y
                // end

                o.uv.z = frac(vertpos.x * 4 + vertpos.z * 4) * frac(vertpos.z);
                o.uv.w = frac(vertpos.x * 2 - vertpos.z * 2) * frac(vertpos.x);

                float4 posWorld = mul(unity_ObjectToWorld, v.pos);

                o.data =  posWorld.xz * 0.01;
                
                o.color = v.color;
                o.pos = UnityObjectToClipPos(v.pos);
                UNITY_TRANSFER_FOG(o, o.pos);

                
                return o;
            }
		
			fixed4 frag(VertexOutput i) : COLOR 
            {
                fixed4 col = lerp(_Color_01, _Color_02, i.uv.z + i.uv.w);
                fixed shade = saturate(i.uv.y + .5);

                    half4 caustics_01 = tex2D(_Caustics, 4 * i.data + _Time.rr * .5);
                    half4 caustics_02 = tex2D(_Caustics, 2 * i.data - _Time.rr * .25);
                    


                fixed4 final = col * shade + .3 * caustics_01 * caustics_02;
                UNITY_APPLY_FOG(i.fogCoord, final);
                
                return final;
            }
            ENDCG
        }
    }
}