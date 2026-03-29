Shader "Hidden/SRP_UI/Default (UIShiny)"
{
	Properties
	{
		[PerRendererData] _MainTex ("Main Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)

		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255

		_ColorMask ("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0

		_ParamTex ("Parameter Texture", 2D) = "white" {}
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

		Stencil
		{
			Ref [_Stencil]
			Comp [_StencilComp]
			Pass [_StencilOp]
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
		}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest [unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask [_ColorMask]
		
		HLSLINCLUDE
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		ENDHLSL

		Pass
		{
			Name "Default"

		HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0

			#pragma multi_compile __ UNITY_UI_ALPHACLIP


			#define UI_SHINY 1
			#include "SRP_UIEffect.hlsl"
			#include "SRP_UIEffectSprite.hlsl"

			half4 frag(v2f IN) : SV_Target
			{
				half4 color = (SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;
				color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);

				color = ApplyShinyEffect(color, IN.eParam);

				#ifdef UNITY_UI_ALPHACLIP
				clip (color.a - 0.001);
				#endif

				return color;
			}
		ENDHLSL
		}
	}
}
