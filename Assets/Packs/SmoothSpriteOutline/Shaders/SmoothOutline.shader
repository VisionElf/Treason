Shader "2D Outlines/Sprite Outline"
{
	Properties
	{
 		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}

		[KeywordEnum(Simple, SimpleWithDiagonals, Smooth)]
		_OutlineQuality("Outline Quality", Float) = 0

		_OutlineColor("Outline Color", Color) = (1,1,1,1)
		_OutlineSize("Outline Size", Float) = 1
		_OutlineMultiplier("Outline Power", Range(0, 20)) = 1
		[PowerSlider(3.0)]_TransparencyCutout("Transparency Tolerance", Range(0, 10)) = 1

		_Size("Rect Size", Range(0, 3)) = 1

        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		[PerRendererData] _EnableExternalAlpha("Enable External Alpha", Float) = 0

		[HideInInspector]_PivotCorrection("Pivot Correction", Vector) = (0,0,0,0)
		[HideInInspector]_SpriteUVs("Sprite Uvs", Vector) = (0,0,0,0)
	}

	SubShader
	{
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		// OUTLINE PASS
		Pass
		{
		CGPROGRAM
			#pragma vertex vert
		    #pragma fragment frag
		    #pragma target 2.5
		    #pragma multi_compile_instancing
		    #pragma multi_compile _ PIXELSNAP_ON

		    #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			#pragma shader_feature _OUTLINEQUALITY_SIMPLE _OUTLINEQUALITY_SIMPLEWITHDIAGONALS _OUTLINEQUALITY_SMOOTH
           	#include "UnitySprites.cginc"

			struct vInput
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct vOutput
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_OUTPUT_STEREO
			};


			float4 _MainTex_TexelSize;
			float4 _OutlineColor;
			fixed _OutlineSize;
			fixed _OutlineMultiplier;
			fixed _TransparencyCutout;
			fixed _Size;

			fixed4 _SpriteUVs;
			fixed4 _PivotCorrection;


			fixed4 SampleSpriteTex(float2 uv)
			{
                //uv = saturate(uv);
				if (uv.x < _SpriteUVs.x) return fixed4(0, 0, 0, 0);
				if (uv.x > _SpriteUVs.z) return fixed4(0, 0, 0, 0);
				if (uv.y < _SpriteUVs.y) return fixed4(0, 0, 0, 0);
				if (uv.y > _SpriteUVs.w) return fixed4(0, 0, 0, 0);

				fixed4 color = tex2D(_MainTex, uv);

			#if ETC1_EXTERNAL_ALPHA
				fixed4 alpha = tex2D(_AlphaTex, uv);
				color.a = lerp(color.a, alpha.r, _EnableExternalAlpha);
			#endif

				return color;
			}


			// BEGIN BLUR VARIANTS
			fixed4 HQBlur(float2 uv, float4 texelSize, float size)
			{
				fixed4 color = fixed4(0, 0, 0, 0);

				// blur LEFT RIGHT
				color += SampleSpriteTex(float2(uv.x - texelSize.x * 3.0 * size, uv.y)) * 0.045;
				color += SampleSpriteTex(float2(uv.x - texelSize.x * 2.0 * size, uv.y)) * 0.06;
				color += SampleSpriteTex(float2(uv.x - texelSize.x * size, uv.y)) * 0.075;
				color += SampleSpriteTex(float2(uv.x, uv.y)) * 0.09;
				color += SampleSpriteTex(float2(uv.x + texelSize.x * size, uv.y)) * 0.075;
				color += SampleSpriteTex(float2(uv.x + texelSize.x * 2.0 * size, uv.y)) * 0.06;
				color += SampleSpriteTex(float2(uv.x + texelSize.x * 3.0 * size, uv.y)) * 0.045;

				// // blur TOP BOT
				color += SampleSpriteTex(float2(uv.x, uv.y - texelSize.y * 3.0 * size)) * 0.045;
				color += SampleSpriteTex(float2(uv.x, uv.y - texelSize.y * 2.0 * size)) * 0.06;
				color += SampleSpriteTex(float2(uv.x, uv.y - texelSize.y * size)) * 0.075;
				color += SampleSpriteTex(float2(uv.x, uv.y)) * 0.09;
				color += SampleSpriteTex(float2(uv.x, uv.y + texelSize.y * size)) * 0.075;
				color += SampleSpriteTex(float2(uv.x, uv.y + texelSize.y * 2.0 * size)) * 0.06;
				color += SampleSpriteTex(float2(uv.x, uv.y + texelSize.y * 3.0 * size)) * 0.045;

				// Simplified diagonals
				color += SampleSpriteTex(float2(uv.x + texelSize.x * 2 * size, uv.y + texelSize.x * 2 * size)) * 0.1;
				color += SampleSpriteTex(float2(uv.x + texelSize.x * 2 * size, uv.y - texelSize.x * 2 * size)) * 0.1;
				color += SampleSpriteTex(float2(uv.x - texelSize.x * 2 * size, uv.y + texelSize.x * 2 * size)) * 0.1;
				color += SampleSpriteTex(float2(uv.x - texelSize.x * 2 * size, uv.y - texelSize.x * 2 * size)) * 0.1;

				color += SampleSpriteTex(float2(uv.x + texelSize.x * 3 * size, uv.y + texelSize.x * 3 * size)) * 0.025;
				color += SampleSpriteTex(float2(uv.x + texelSize.x * 3 * size, uv.y - texelSize.x * 3 * size)) * 0.025;
				color += SampleSpriteTex(float2(uv.x - texelSize.x * 3 * size, uv.y + texelSize.x * 3 * size)) * 0.025;
				color += SampleSpriteTex(float2(uv.x - texelSize.x * 3 * size, uv.y - texelSize.x * 3 * size)) * 0.025;

				return color / 1.4;
			}

			fixed4 LQBlur(float2 uv, float4 texelSize, float size)
			{
				fixed4 color = fixed4(0, 0, 0, 0);

				// blur LEFT RIGHT
				color += SampleSpriteTex(float2(uv.x - texelSize.x * size, uv.y));
				color += SampleSpriteTex(float2(uv.x + texelSize.x * size, uv.y));

				// // blur TOP BOT
				color += SampleSpriteTex(float2(uv.x, uv.y - texelSize.y * size));
				color += SampleSpriteTex(float2(uv.x, uv.y + texelSize.y * size));

				return color * 0.25;
			}

			fixed4 LQWithDiagBlur(float2 uv, float4 texelSize, float size)
			{
				fixed4 color = fixed4(0, 0, 0, 0);

				// blur LEFT RIGHT
				color += SampleSpriteTex(float2(uv.x - texelSize.x * size, uv.y)) * 0.125;
				color += SampleSpriteTex(float2(uv.x + texelSize.x * size, uv.y)) * 0.125;

				// // blur TOP BOT
				color += SampleSpriteTex(float2(uv.x, uv.y - texelSize.y * size)) * 0.125;
				color += SampleSpriteTex(float2(uv.x, uv.y + texelSize.y * size)) * 0.125;

				// Simplified diagonals
				color += SampleSpriteTex(float2(uv.x + texelSize.x * size, uv.y + texelSize.x * size)) * 0.125;
				color += SampleSpriteTex(float2(uv.x + texelSize.x * size, uv.y - texelSize.x * size)) * 0.125;
				color += SampleSpriteTex(float2(uv.x - texelSize.x * size, uv.y + texelSize.x * size)) * 0.125;
				color += SampleSpriteTex(float2(uv.x - texelSize.x * size, uv.y - texelSize.x * size)) * 0.125;

				return color;
			}
			// END BLUR VARIANTS 

			vOutput vert(vInput IN)
			{
				vOutput OUT;

				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

				#ifdef UNITY_INSTANCING_ENABLED
				IN.vertex.xy *= _Flip.xy;
				#endif
				
				//fixed4 vert = IN.vertex;
				IN.vertex.xy *= _Size;

				// CORRECT SPRITE PIVOT
				IN.vertex.xy += _PivotCorrection.xy;

				// -----------

				OUT.vertex = UnityObjectToClipPos(IN.vertex);

				// CORRECT SPRITE-SHEET UVs
				fixed2 length = _SpriteUVs.xy + _SpriteUVs.zw;
				OUT.texcoord = ((IN.texcoord) * _Size) - (_Size - 1) *  length * 0.5;
				// -----

				OUT.color = IN.color * _Color * _RendererColor;

				return OUT;
			}

			fixed4 frag(vOutput IN) : SV_Target
			{
				fixed2 texCoords = IN.texcoord;

				fixed4 spriteCol = SampleSpriteTex(float2(texCoords.x, texCoords.y));

				fixed4 cBlur;

				#ifdef _OUTLINEQUALITY_SIMPLE
				cBlur = LQBlur(texCoords, _MainTex_TexelSize, _OutlineSize);
				#elif _OUTLINEQUALITY_SIMPLEWITHDIAGONALS
				cBlur = LQWithDiagBlur(texCoords, _MainTex_TexelSize, _OutlineSize);
				#else
				cBlur = HQBlur(texCoords, _MainTex_TexelSize, _OutlineSize);
				#endif	

				cBlur.a *= saturate(1 - spriteCol.a * _TransparencyCutout);

				cBlur.rgb = _OutlineColor.rgb * _OutlineMultiplier;
				cBlur.a *= _OutlineColor.a;
				cBlur.rgb *= cBlur.a;

				return cBlur;
			}

		ENDCG
		}

		// DRAW SPRITE PASS - Would break batching, therefore it's better to draw the sprite as a separate gameObject!

		//Pass
		//{
		//CGPROGRAM
		//	#pragma vertex SpriteVert
		//	#pragma fragment SpriteFrag
		//	#pragma target 2.0
		//	#pragma multi_compile_instancing
		//	#pragma multi_compile _ PIXELSNAP_ON
		//	#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
		//	#include "UnitySprites.cginc"
		//ENDCG
		//}
	}
}
