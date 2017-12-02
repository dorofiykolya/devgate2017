Shader "Environment/Rays"
{
	Properties
	{
		[NoScaleOffset] _MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
	}

	SubShader
	{
        Tags 
        {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }

        Blend One One
        ZWrite Off

		Pass
		{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 pos : POSITION;
				float2 uv : TEXCOORD0;
				float4 normal : NORMAL;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float4 uv : TEXCOORD0;
				half fresnel : COLOR;
			};

			uniform sampler2D _MainTex;
			uniform half4 _Color;
			
			v2f vert (appdata v)
			{
				v2f o;

				o.pos = UnityObjectToClipPos(v.pos);

				o.uv.xy = v.uv + frac(float2(.25 * _Time.r, 0));
				o.uv.zw = v.uv * half2(2, 1) + frac(float2(-.5 * _Time.r, 0));

                float3 normalDir = UnityObjectToWorldNormal(v.normal);
                float3 posWorld = mul(unity_ObjectToWorld, v.pos);
                
				float3 viewDir = normalize(_WorldSpaceCameraPos - posWorld);
                
                o.fresnel = dot(normalDir, viewDir) * v.uv.y;

				return o;
			}
			
			half4 frag (v2f i) : SV_Target
			{
				half col_01 = tex2D(_MainTex, i.uv.xy).r;
				half col_02 = tex2D(_MainTex, i.uv.zw).r;
				half3 col = saturate(2 * col_01 * col_02 * _Color.rgb * _Color.a* i.fresnel);

				return half4(col, 1);
			}

			ENDCG
		}
	}
}
