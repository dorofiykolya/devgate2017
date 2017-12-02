Shader "DissolveObjects"
{
	Properties
	{
		_HeightMap ("Height Map", 2D) = "white" {}
		_Dissolve ("Dissolve", Range(0, 1)) = 0
		_Color ("Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

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
				float4 pos : POSITION;
				float2 uv : TEXCOORD0;
				float4 normal : NORMAL;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
				UNITY_FOG_COORDS(1)
			};

			sampler2D _HeightMap;
			float4 _Color;
			float _Dissolve;

			
			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.pos);
				o.uv = v.uv;
				o.normal = v.normal;
				UNITY_TRANSFER_FOG(o, o.pos);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture

				float3 dir = normalize(float3(0, 1, .5));
				float shade = dot(i.normal, dir);

				fixed4 height = tex2D(_HeightMap, i.uv);
				float c = height.r - _Dissolve;

				float4 col = _Color * shade;

				clip(c);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
