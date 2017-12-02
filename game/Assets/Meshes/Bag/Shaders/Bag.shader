// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Bag" 
{
    Properties 
    {
        _Color("Color", Color) = (1, 0, 0, 1)
        _Noise ("Noise", 2D) = "white" {}
    }
    SubShader {
        Tags 
        {
            "IgnoreProjector" = "True"
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
        }
        
        Pass 
        {
            Name "FORWARD"
            Tags {"LightMode"="ForwardBase"}
            Blend One OneMinusSrcAlpha
            
            CGPROGRAM
            
                #pragma vertex vert
                #pragma fragment frag
                #pragma target 2.0
                #include "UnityCG.cginc"
                
                uniform half4 _Color;
                uniform sampler2D _Noise;
                
                struct VertexInput 
                {
                    float4 pos : POSITION;
                    float3 normal: NORMAL;
                    float4 uv:TEXCOORD0;
                };

                struct VertexOutput 
                {
                    float4 pos : SV_POSITION;
                    float4 data : TEXCOORD0;
                    float fresnel : COLOR;
                };

                VertexOutput vert (VertexInput v) 
                {
                    VertexOutput o = (VertexOutput)0;
                    
                    v.pos.y += .02 * sin(_Time.w);
                    o.pos = UnityObjectToClipPos(v.pos);

                    float3 normalDir = UnityObjectToWorldNormal(v.normal);
                    float3 posWorld = mul(unity_ObjectToWorld, v.pos);
                    float4 objPos = mul(unity_ObjectToWorld, float4(0,0,0,1));

                    o.data.xy = posWorld.xy;
                    o.data.zw = objPos.xy;

                    float3 posCam = float3(_WorldSpaceCameraPos.x, _WorldSpaceCameraPos.y, _WorldSpaceCameraPos.z);

                    float3 viewDir = normalize(posCam - posWorld);
                    viewDir.y = 0.5;
                    o.fresnel = dot(normalDir, viewDir);
                    

                    return o;
                }
                
                half4 frag(VertexOutput i) : COLOR 
                {
                    float2 posWorld = i.data.xy;
                    float2 objPos = i.data.zw;

                    half spec = saturate(1 - pow((1.25 - posWorld.y + objPos.y) + (objPos.x - posWorld.x) , 32));
                    
                    half4 col = saturate((1 - i.fresnel) * (_Color + spec * .25));
                    
                    return col;
                }

            ENDCG
        }
    }
}
