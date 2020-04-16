Shader "ScreenSpace" {
	Properties{
		_Color("Tint", Color) = (0,0,0,1)
		_MainTex("Texture", 2D) = "white" {}
	}

	Subshader {
		Tags {
				"RenderType" = "Opaque"
				"Queue" = "Geometry"
		}
		Cull Off
		Pass {
			CGPROGRAM
			#include "UnityCG.cginc"

			#pragma vertex vert
			#pragma fragment frag

			fixed4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;

			struct appdata {
				float4 vertex: POSITION;
				float2 uv: TEXCOORD0;
			};

			struct v2f { //vertex to fragment
				float4 position: SV_POSITION;
				float4 screenPos: TEXCOORD0;
			};


			v2f vert(appdata v) { //rasterizer - convert data to pixels
				v2f o;
				o.position = UnityObjectToClipPos(v.vertex);
				o.screenPos = ComputeScreenPos(o.position);
				return o;
			}

			fixed4 frag(v2f i) : SV_TARGET{
				float2 screenSpaceUV = i.screenPos.xy / i.screenPos.w;
				return tex2D(_MainTex, screenSpaceUV);
			}

			ENDCG
		}
	}
}