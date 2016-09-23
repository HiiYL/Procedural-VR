Shader "Portals/render textures Unlit" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		
		Stencil {
				Ref 0
				Comp Equal
			}
		
		CGPROGRAM
		#pragma surface surf Unlit
		
		fixed4 LightingUnlit(SurfaceOutput s, fixed3 lightDir, fixed atten) {
			fixed4 c;
			c.rgb = s.Albedo*0.5f;
			//c.a = s.Alpha;
			return c;
		}

		sampler2D _MainTex;

		struct Input {
			float4 screenPos;
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
			o.Albedo = tex2D (_MainTex, screenUV).rgb;
		}
		ENDCG
	} 
}