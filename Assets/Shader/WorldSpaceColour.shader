Shader "Custom/WorldSpaceColour"
{
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		struct Input
		{
		    float3 worldPos;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			o.Albedo = sin(IN.worldPos) * 0.4 + 0.6;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
