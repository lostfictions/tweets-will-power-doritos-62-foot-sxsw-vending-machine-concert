Shader "Custom/WorldSpaceColourUnlit" {
	SubShader {
	    Pass {
	        Fog { Mode Off }
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			// vertex input: position, tangent
			struct appdata {
			    float4 vertex : POSITION;
			};

			struct v2f {
			    float4 pos : SV_POSITION;
			    fixed4 color : COLOR;
			};

			v2f vert (appdata v) {
			    v2f o;
			    o.pos = mul( UNITY_MATRIX_MVP, v.vertex );
			    o.color = sin(o.pos) * 0.4 + 0.6;
			    return o;
			}

			fixed4 frag (v2f i) : COLOR0 {
				return i.color;
			}

			ENDCG
	    }
	}
}