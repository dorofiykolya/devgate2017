Shader "VFX/CameraUnderwater"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_NormalTex("Normal", 2D) = "white" {}
		_Sphere("Sphere", 2D) = "white" {}
		_Effect("Effect", Float) = 1
		_SphereEffect("SphereEffect", Range(0, 1)) = 1 
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _NormalTex;
			sampler2D _Sphere;
			float _Effect;
			float _SphereEffect;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 normalTex = tex2D(_NormalTex, i.uv + _Time.rr);
				fixed4 sphereTex = tex2D(_Sphere, i.uv);
				fixed4 col = tex2D(_MainTex, normalTex.rg * _Effect + lerp(sphereTex.rg, i.uv, _SphereEffect));
				
				return col;
			}
			ENDCG
		}
	}
}
