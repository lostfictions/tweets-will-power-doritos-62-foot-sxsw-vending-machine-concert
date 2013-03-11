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
			o.Albedo = fmod(abs(IN.worldPos - 0.5), 1.0);
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
