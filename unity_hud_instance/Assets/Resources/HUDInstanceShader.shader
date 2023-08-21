Shader "HUD/Instance"
{
	Properties
	{
		_FontTex("FontTexture", 2DArray) = "white" {}
		_Color("Color", Color) = (1, 1, 1, 1)
	}

	SubShader
	{
		Tags { "RenderType" = "Transparent" }
		LOD 100
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing	//激活Instance
			#pragma enable_d3d11_debug_symbols	//开启调试
			#pragma require 2darray				//需要2d array
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 uv : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 color : COLOR;
				float2 uv : TEXCOORD0;
				float4 parm : TEXCOORD1;
			};

			UNITY_DECLARE_TEX2DARRAY(_FontTex);
			float4 _FontTex_ST;
			float4 _Parms[511];

			v2f vert(appdata v, uint instanceID : SV_InstanceID)
			{
				v2f o;
				
				UNITY_SETUP_INSTANCE_ID(v);
				
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _FontTex);
				o.color = v.color;
				o.parm = _Parms[instanceID];

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 color = fixed4(0,0,0,0);
				if (i.color.a > 1.0)
				{
					clip(i.parm.x - i.uv.x);//通过uv计算进度，uv从0-1
					color = i.color;
				}
				else
				{
					float3 uv = float3(i.uv.xy, i.parm.y);
					color = UNITY_SAMPLE_TEX2DARRAY(_FontTex, uv);
				}
				return color;
			}
			ENDCG
		}
	}
}