Shader "Hidden/MKGlowComposite" 
{
	Properties 
	{ 
		_MainTex("", 2D) = "Black" {}
		_GlowTex("", 2D) = "Black" {} 
	}
	Subshader 
	{
		ZTest off 
		Fog { Mode Off }
		Cull back
		Lighting Off
		ZWrite Off

		Pass 
		{
			Blend One Zero
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma target 2.0

			#include "UnityCG.cginc"
			
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform sampler2D _GlowTex;
			uniform float4 _MKGlowTex_ST;
			uniform float2 _GlowTex_TexelSize;

			struct Input
			{
				float2 texcoord : TEXCOORD0;
				float4 vertex : POSITION;
			};
			
			struct Output 
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				#if UNITY_SINGLE_PASS_STEREO
					UNITY_VERTEX_OUTPUT_STEREO
				#endif
			};
			
			Output vert (Input i)
			{
				Output o;
				UNITY_INITIALIZE_OUTPUT(Output,o);
				#if UNITY_SINGLE_PASS_STEREO
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				#endif
				o.pos = UnityObjectToClipPos (i.vertex);
				o.uv = TRANSFORM_TEX(i.texcoord, _MainTex);
				#if UNITY_UV_STARTS_AT_TOP
				if (_GlowTex_TexelSize.y < 0)
						o.uv.y = 1-o.uv.y;
				#endif
				
				#if UNITY_SINGLE_PASS_STEREO
					UNITY_TRANSFER_VERTEX_OUTPUT_STEREO(i,o);
				#endif
				return o;
			}

			fixed4 frag( Output i ) : SV_TARGET
			{
				#if UNITY_SINGLE_PASS_STEREO
					UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				#endif
				#if UNITY_SINGLE_PASS_STEREO
					fixed4 m = tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv.xy, _MainTex_ST));
					fixed4 g = tex2D(_GlowTex, UnityStereoScreenSpaceUVAdjust(i.uv.xy, _MKGlowTex_ST));
				#else
					fixed4 m = tex2D( _MainTex, i.uv.xy);
					fixed4 g = tex2D(_GlowTex, i.uv.xy);
				#endif

				return g+m;
			}
			ENDCG
		}
	}
	FallBack Off
}