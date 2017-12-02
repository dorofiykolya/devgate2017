Shader "GatePlane"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
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

		LOD 100
		Blend One One

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Color;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col_01 = tex2D(_MainTex, i.uv + _Time.gg * .25);
				fixed4 col_02 = tex2D(_MainTex, i.uv - _Time.gg * .1);
				float col = col_01.r * col_02.r;

				i.uv = (i.uv - .5) * 2;
				i.uv *= i.uv;
				float circle = i.uv.x + i.uv.y;
				circle *= .15 * col.r;
				float4 final = _Color;

				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, final);
				return final * pow(circle, .5);
			}
			ENDCG
		}
	}
}
