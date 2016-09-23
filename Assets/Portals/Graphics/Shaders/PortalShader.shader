Shader "Portals/CubeMap" {
	Properties {
		_Cube ("Cubemap", CUBE) = "" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		
		Stencil {
				Ref 0
				Comp Equal
			}
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		samplerCUBE _Cube;

		struct Input {
			float4 screenPos;
			float3 worldRefl;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
			
			o.Albedo = texCUBE (_Cube, float3(-IN.worldRefl.x, IN.worldRefl.y, -IN.worldRefl.z)).rgb;
			o.Alpha = texCUBE (_Cube, float3(-IN.worldRefl.x, IN.worldRefl.y, -IN.worldRefl.z)).a;
			//o.Emission = texCUBE (_Cube, IN.worldRefl).rgb;
		}
		ENDCG
	} 
}