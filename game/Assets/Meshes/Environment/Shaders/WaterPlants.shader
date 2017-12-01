// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Environment/WaterPlants" 
{
    
	Properties 
    {
        _Color_01 ("Color 01", Color) = (0.443, 0.584, 0.161, 1)
        _Color_02 ("Color 02", Color) = (0.565, 0.565, 0.176, 1)
        _PrimaryFreq ("Primary Freqency", Float) = 2
        _SecondaryFreq ("Secondary Freqency", Float) = 2
        _primaryMotion ("Primary Motion Amp", Range(0, 500)) = 250
        _secondaryMotion ("Secondary Motion Amp", Range(0, 200)) = 50
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
            uniform fixed4 _Color_01;
            uniform fixed4 _Color_02;
            uniform float _PrimaryFreq;
            uniform float _SecondaryFreq;
            uniform float _primaryMotion;
            uniform float _secondaryMotion;
           
			struct VertexInput 
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct VertexOutput 
            {
                float4 pos : SV_POSITION;
                float4 uv : TEXCOORD0;
                float4 color : COLOR;
                UNITY_FOG_COORDS(1)
            };

            VertexOutput vert(VertexInput v) 
            {
                VertexOutput o = (VertexOutput)0;
                o.uv.xy = v.uv;
                float4 vertpos = mul(unity_ObjectToWorld, v.vertex);
                
                // Animation
                    float pi = 3.14159;
                    float freq = _Time.r * _PrimaryFreq;
                    float phase;

                    phase = (pi * v.color.r)/2; 

                    fixed amp = v.color.g * .01 * _primaryMotion;
                    v.vertex.x += amp * sin(freq + phase);             //top rocking X  
                    v.vertex.z += amp * cos(1.5 * (freq + phase));     //top rocking Y
                    float secMotion =  v.color.b * sin(_Time.g * _SecondaryFreq + v.vertex.y) * _secondaryMotion;	    	//branches wiggle
                    v.vertex.y += secMotion;
                // end

                o.uv.z = frac(vertpos.x * 10 + vertpos.z * 10) * frac(vertpos.z);
                o.uv.w = frac(vertpos.x * 5 - vertpos.z * 5) * frac(vertpos.x);
                
                o.color = v.color;
                o.pos = UnityObjectToClipPos(v.vertex);
                UNITY_TRANSFER_FOG(o, o.pos);

                
                return o;
            }
		
			fixed4 frag(VertexOutput i) : COLOR 
            {
                fixed4 col = lerp(_Color_01, _Color_02, i.uv.z + i.uv.w);
                fixed shade = i.color.g;
                fixed4 final = col * shade;
                UNITY_APPLY_FOG(i.fogCoord, final);
                
                return final;
            }
            ENDCG
        }
    }
}