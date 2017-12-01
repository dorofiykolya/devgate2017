// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "VFX/Kingdom/Ground" {
    Properties 
    {
        [NoScaleOffset] _Grass ("Grass", 2D) = "white" {}
        [NoScaleOffset] _Snow("Snow", 2D) = "white" {}
        [NoScaleOffset] _Desert ("Desert", 2D) = "white" {}
        [NoScaleOffset] _Sand ("Sand", 2D) = "white" {}
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

                #include "UnityCG.cginc"
                
                    uniform sampler2D _ClimateMap; 
                    uniform sampler2D _Snow; 
                    uniform sampler2D _Desert;
                    

                uniform sampler2D _Grass;
                uniform sampler2D _Sand;

                struct VertexInput 
                {
                    float4 vertex : POSITION;
                    float4 vertexColor : COLOR;
                };

                struct VertexOutput 
                {
                    float4 pos : SV_POSITION;
                    float2 uv : TEXCOORD0;
                    float data : TEXCOORD1;
                    float4 color : COLOR;
                };
                
                VertexOutput vert (VertexInput v) 
                {
                    VertexOutput o = (VertexOutput)0;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    float4 posWorld = mul(unity_ObjectToWorld, v.vertex);
                    

                    o.uv.xy =  posWorld.xz * 0.01;
                    o.data.y = (posWorld.z + 1000) * 0.002;

                    return o;
                }
                
                half4 frag(VertexOutput i) : COLOR 
                {
                    half4 grass = tex2D(_Grass, i.uv.zw * 0.5);
                    half4 sand = tex2D(_Sand, i.uv.zw);

                    #ifdef GROUND_CLIMATZONE_ON

                        half4 desert = tex2D(_Desert, i.uv.zw * 0.25);
                        half4 snow = tex2D(_Snow, i.uv.zw * 0.25);
                        
                        half4 grass_desert = lerp(grass, desert, desertMask);
                        half4 grass_snow = lerp(grass_desert, snow, snowMask);

                        half4 final_tex = grass_snow;




                        half3 ground = lerp(final_tex.rgb, lerp(sand.rgb, sand_snow.rgb, snowMask), saturate(2 * i.data.x));

                        half3 ground_grid = lerp(ground.rgb, lerp(1, 0.25, snowMask + desertMask), grid.rgb * _GridAlpha);

                        half3 ground_selection = lerp(ground_grid, selection, _SelectionOffset.w * selection.r);
                        
                        half4 finalColor = half4(ground_selection, 1);

                    #else

                        half4 grid = tex2D(_GridTex, i.uv.zw);
                        half4 selection = tex2D(_SelectionTex, i.data.zw);

                        half3 ground = lerp(grass.rgb, sand.rgb, saturate(2 * i.data.x));
                        half3 ground_grid = lerp(ground.rgb, 1, grid.rgb * _GridAlpha);
                        half3 ground_selection = lerp(ground_grid, selection, _SelectionOffset.w * selection.r);
                        
                        half4 finalColor = half4(ground_selection, 1);

                    #endif

                    return finalColor;
                }
                
            ENDCG
        }
    }
}
