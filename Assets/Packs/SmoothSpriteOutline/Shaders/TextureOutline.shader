Shader "2D Outlines/Texture Outline"
{
	Properties
	{
 		[NoScaleOffset]_MainTex ("Texture", 2D) = "white" {}

		[KeywordEnum(Simple, SimpleWithDiagonals, Smooth)]
		_OutlineQuality("Outline Quality", Float) = 0

		_OutlineColor("Outline Color", Color) = (1,1,1,1)
		_OutlineSize("Outline Size", Float) = 1
		_OutlineMultiplier("Outline Power", Range(0, 20)) = 1
		[PowerSlider(3.0)]_TransparencyCutout("Transparency Tolerance", Range(0, 10)) = 1

		_Size("Rect Size", Range(0, 3)) = 1
	}

	SubShader
	{
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
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
			#pragma shader_feature _OUTLINEQUALITY_SIMPLE _OUTLINEQUALITY_SIMPLEWITHDIAGONALS _OUTLINEQUALITY_SMOOTH

			struct vInput
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct vOutput
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};


			sampler2D _MainTex;
			float4 _MainTex_TexelSize;
			float4 _OutlineColor;
			fixed _OutlineSize;
			fixed _OutlineMultiplier;
			fixed _TransparencyCutout;
			fixed _Size;


			fixed4 SampleTex(float2 uv)
			{
				if (uv.x < 0) return fixed4(0, 0, 0, 0);
				if (uv.x > 1) return fixed4(0, 0, 0, 0);
				if (uv.y < 0) return fixed4(0, 0, 0, 0);
				if (uv.y > 1) return fixed4(0, 0, 0, 0);

				return tex2D(_MainTex, uv);
			}


			// BEGIN BLUR VARIANTS
			fixed4 HQBlur(float2 uv, float4 texelSize, float size)
			{
				fixed4 color = fixed4(0, 0, 0, 0);

				// blur LEFT RIGHT
				color += SampleTex(float2(uv.x - texelSize.x * 3.0 * size, uv.y)) * 0.045;
				color += SampleTex(float2(uv.x - texelSize.x * 2.0 * size, uv.y)) * 0.06;
				color += SampleTex(float2(uv.x - texelSize.x * size, uv.y)) * 0.075;
				color += SampleTex(float2(uv.x, uv.y)) * 0.09;
				color += SampleTex(float2(uv.x + texelSize.x * size, uv.y)) * 0.075;
				color += SampleTex(float2(uv.x + texelSize.x * 2.0 * size, uv.y)) * 0.06;
				color += SampleTex(float2(uv.x + texelSize.x * 3.0 * size, uv.y)) * 0.045;

				// // blur TOP BOT
				color += SampleTex(float2(uv.x, uv.y - texelSize.y * 3.0 * size)) * 0.045;
				color += SampleTex(float2(uv.x, uv.y - texelSize.y * 2.0 * size)) * 0.06;
				color += SampleTex(float2(uv.x, uv.y - texelSize.y * size)) * 0.075;
				color += SampleTex(float2(uv.x, uv.y)) * 0.09;
				color += SampleTex(float2(uv.x, uv.y + texelSize.y * size)) * 0.075;
				color += SampleTex(float2(uv.x, uv.y + texelSize.y * 2.0 * size)) * 0.06;
				color += SampleTex(float2(uv.x, uv.y + texelSize.y * 3.0 * size)) * 0.045;

				// Simplified diagonals
				color += SampleTex(float2(uv.x + texelSize.x * 2 * size, uv.y + texelSize.x * 2 * size)) * 0.1;
				color += SampleTex(float2(uv.x + texelSize.x * 2 * size, uv.y - texelSize.x * 2 * size)) * 0.1;
				color += SampleTex(float2(uv.x - texelSize.x * 2 * size, uv.y + texelSize.x * 2 * size)) * 0.1;
				color += SampleTex(float2(uv.x - texelSize.x * 2 * size, uv.y - texelSize.x * 2 * size)) * 0.1;

				color += SampleTex(float2(uv.x + texelSize.x * 3 * size, uv.y + texelSize.x * 3 * size)) * 0.025;
				color += SampleTex(float2(uv.x + texelSize.x * 3 * size, uv.y - texelSize.x * 3 * size)) * 0.025;
				color += SampleTex(float2(uv.x - texelSize.x * 3 * size, uv.y + texelSize.x * 3 * size)) * 0.025;
				color += SampleTex(float2(uv.x - texelSize.x * 3 * size, uv.y - texelSize.x * 3 * size)) * 0.025;

				return color / 1.4;
			}

			fixed4 LQBlur(float2 uv, float4 texelSize, float size)
			{
				fixed4 color = fixed4(0, 0, 0, 0);

				// blur LEFT RIGHT
				color += SampleTex(float2(uv.x - texelSize.x * size, uv.y)) * 0.25;
				color += SampleTex(float2(uv.x + texelSize.x * size, uv.y)) * 0.25;

				// // blur TOP BOT
				color += SampleTex(float2(uv.x, uv.y - texelSize.y * size)) * 0.25;
				color += SampleTex(float2(uv.x, uv.y + texelSize.y * size)) * 0.25;

				return color;
			}

			fixed4 LQWithDiagBlur(float2 uv, float4 texelSize, float size)
			{
				fixed4 color = fixed4(0, 0, 0, 0);

				// blur LEFT RIGHT
				color += SampleTex(float2(uv.x - texelSize.x * size, uv.y)) * 0.125;
				color += SampleTex(float2(uv.x + texelSize.x * size, uv.y)) * 0.125;

				// // blur TOP BOT
				color += SampleTex(float2(uv.x, uv.y - texelSize.y * size)) * 0.125;
				color += SampleTex(float2(uv.x, uv.y + texelSize.y * size)) * 0.125;

				// Simplified diagonals
				color += SampleTex(float2(uv.x + texelSize.x * size, uv.y + texelSize.x * size)) * 0.125;
				color += SampleTex(float2(uv.x + texelSize.x * size, uv.y - texelSize.x * size)) * 0.125;
				color += SampleTex(float2(uv.x - texelSize.x * size, uv.y + texelSize.x * size)) * 0.125;
				color += SampleTex(float2(uv.x - texelSize.x * size, uv.y - texelSize.x * size)) * 0.125;

				return color;
			}
			// END BLUR VARIANTS 

			vOutput vert(vInput IN)
			{
				vOutput OUT;

				fixed4 vert = IN.vertex;
				vert.xy *= _Size;
				OUT.vertex = UnityObjectToClipPos(vert);

				OUT.texcoord = ((IN.texcoord) * _Size) - (_Size - 1) * 0.5;

				OUT.color = IN.color;

				return OUT;
			}

			fixed4 frag(vOutput IN) : SV_Target
			{
				fixed2 texCoords = IN.texcoord;

				fixed4 texCol = SampleTex(float2(texCoords.x, texCoords.y));

				fixed4 cBlur;

				#ifdef _OUTLINEQUALITY_SIMPLE
				cBlur = LQBlur(texCoords, _MainTex_TexelSize, _OutlineSize);
				#elif _OUTLINEQUALITY_SIMPLEWITHDIAGONALS
				cBlur = LQWithDiagBlur(texCoords, _MainTex_TexelSize, _OutlineSize);
				#else
				cBlur = HQBlur(texCoords, _MainTex_TexelSize, _OutlineSize);
				#endif	

				cBlur.a *= saturate(1 - texCol.a * _TransparencyCutout);

				cBlur.rgb = _OutlineColor.rgb * _OutlineMultiplier;
				cBlur.a *= _OutlineColor.a;
				cBlur.rgb *= cBlur.a;

				return cBlur;
			}

		ENDCG
		}
	}
}
