Shader "Hidden/MKGlowRender"
{
	SubShader 
	{
		Tags { "RenderType"="MKGlow" "Queue"="Transparent"}
		Blend SrcAlpha OneMinusSrcAlpha
		Pass 
		{
			ZTest LEqual  
			Fog { Mode Off }
			Cull Back
			Lighting Off
			ZWrite On

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma target 2.0

			#include "UnityCG.cginc"
			
			uniform sampler2D _MKGlowTex;
			uniform float4 _MKGlowTex_ST;
			uniform fixed4 _MKGlowColor;
			uniform half _MKGlowPower;
			uniform half _MKGlowTexPower;
			uniform fixed4 _Color;
			
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
				o.uv = TRANSFORM_TEX(i.texcoord, _MKGlowTex);
				#if UNITY_SINGLE_PASS_STEREO
					UNITY_TRANSFER_VERTEX_OUTPUT_STEREO(i,o);
				#endif
				return o;
			}

			fixed4 frag (Output i) : SV_TARGET
			{
				#if UNITY_SINGLE_PASS_STEREO
					UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				#endif
				#if UNITY_SINGLE_PASS_STEREO
					fixed4 glow = tex2D(_MKGlowTex, UnityStereoScreenSpaceUVAdjust(i.uv.xy, _MKGlowTex_ST));
				#else
					fixed4 glow = tex2D(_MKGlowTex, i.uv.xy);
				#endif
				glow.rgb *= (_MKGlowColor * _MKGlowPower);
				glow.a = _Color.a;
				return glow;
			}
			ENDCG
		}
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		Pass 
		{
			Fog { Mode Off }
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma target 2.0
			
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
				o.uv = i.texcoord;
				#if UNITY_SINGLE_PASS_STEREO
					UNITY_TRANSFER_VERTEX_OUTPUT_STEREO(i,o);
				#endif
				return o;
			}

			fixed4 frag (Output i) : SV_TARGET
			{
				#if UNITY_SINGLE_PASS_STEREO
					UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				#endif
				return fixed4(0,0,0,0);
			}
			
			ENDCG
		}
	}
	SubShader 
	{
		Tags { "RenderType"="Transparent" }
		Pass 
		{
			Fog { Mode Off }
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma target 2.0
			
			uniform fixed4 _Color;

			struct Input
			{
				float2 texcoord : TEXCOORD0;
				float4 vertex : POSITION;
			};
			
			struct Output 
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR0;
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
				o.uv = i.texcoord;
				o.color = _Color;
				#if UNITY_SINGLE_PASS_STEREO
					UNITY_TRANSFER_VERTEX_OUTPUT_STEREO(i,o);
				#endif
				return o;
			}

			fixed4 frag (Output i) : SV_TARGET
			{
				#if UNITY_SINGLE_PASS_STEREO
					UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				#endif
				return fixed4(0,0,0,i.color.a);
			}
			
			ENDCG
		}
	} 
	
	SubShader 
	{
		Tags { "RenderType"="TransparentCutout" }
		Pass 
		{
			Fog { Mode Off }
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma target 2.0
			
			uniform fixed4 _Color;

			struct Input
			{
				float2 texcoord : TEXCOORD0;
				float4 vertex : POSITION;
			};
			
			struct Output 
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR0;
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
				o.uv = i.texcoord;
				#if UNITY_SINGLE_PASS_STEREO
					UNITY_TRANSFER_VERTEX_OUTPUT_STEREO(i,o);
				#endif
				return o;
			}

			fixed4 frag (Output i) : SV_TARGET
			{
				#if UNITY_SINGLE_PASS_STEREO
					UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				#endif
				return fixed4(0,0,0,i.color.a);
			}
			
			ENDCG
		}
	} 
} 

