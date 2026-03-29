Shader "Edgar/FogOfWarURP" {
    Properties {}
    SubShader {
    	PackageRequirements
        {
            "com.unity.render-pipelines.universal": "9.0.0"
            "unity": "2022.2"
        }
        Tags { 
        	"RenderType"="Transparent"
            "Queue" = "Transparent"
        }
        LOD 100
        ZWrite Off Cull Off
        Pass {
            Name "BlitColor"

            HLSLPROGRAM
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            // The Blit.hlsl file provides the vertex shader (Vert),
            // input structure (Attributes) and output strucutre (Varyings)
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            #pragma vertex VertNoScaleBias
            #pragma fragment frag

            uniform sampler2D _VisionTex;
			uniform sampler2D _VisionTex2;
			uniform float2 _VisionTexOffset;
			uniform float2 _VisionTexSize;
			uniform float2 _CellSize;
			uniform float4 _FogColor;
			uniform float _TileGranularity;
			uniform float _FogSmoothness;
			uniform float _InitialFogTransparency;

            uniform Texture2D _CameraDepthTexture;
            uniform float4x4 _ViewProjInv;

            // Copied from Blit.hlsl, but edited to remove _BlitScaleBias to make sure result fits screen properly
            // ... why do I need to do this??
            Varyings VertNoScaleBias(Attributes input) {
                Varyings output;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

            #if SHADER_API_GLES
                float4 pos = input.positionOS;
                float2 uv  = input.uv;
            #else
                float4 pos = GetFullScreenTriangleVertexPosition(input.vertexID);
                float2 uv  = GetFullScreenTriangleTexCoord(input.vertexID);
            #endif

                output.positionCS = pos;
                output.texcoord = uv; // * _BlitScaleBias.xy + _BlitScaleBias.zw;
                return output;
            }

            float4 GetWorldPositionFromDepth( float2 uv_depth )
			{    
				float depth = SAMPLE_TEXTURE2D_X(_CameraDepthTexture, sampler_PointClamp, uv_depth).r;
				float4 H = float4(uv_depth.x*2.0-1.0, (uv_depth.y)*2.0-1.0, depth, 1.0);
				float4 D = mul(_ViewProjInv,H);
				return D/D.w;
			}

            half4 frag (Varyings input) : SV_TARGET {
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                float4 c = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_PointClamp, input.texcoord);
				half4 result = c;
				float4 worldpos = GetWorldPositionFromDepth(input.texcoord);

				// Compute world position rounded to whole tiles
				float floorX = (- _VisionTexOffset.x + worldpos.x) / _CellSize.x - 1;
				float floorY = (- _VisionTexOffset.y + worldpos.y) / _CellSize.y - 1;

				#if defined(FOG_CUSTOM_MODE)
					float p = 0.5 / _TileGranularity;
					floorX = floor((floorX) * _TileGranularity) / _TileGranularity + p;
					floorY = floor((floorY) * _TileGranularity) / _TileGranularity + p;
				#endif

				float2 floorWorldpos = float2(floorX, floorY);

				// Check if the tile is covered by the Fog of War texture
				// If it is, compute the color of the tile using the texture
				if (floorX > 0 && floorY > 0 && floorWorldpos.x < _VisionTexSize.x && floorWorldpos.y < _VisionTexSize.y) {

					float4 color = tex2D(_VisionTex, float2(floorWorldpos.x / float(_VisionTexSize.x), floorWorldpos.y / float(_VisionTexSize.y)));
					float4 colorInterpolated = tex2D(_VisionTex2, float2(floorWorldpos.x / float(_VisionTexSize.x), floorWorldpos.y / float(_VisionTexSize.y)));

					float isInterpolated = color.g;

					if (isInterpolated > 0.5) {
						#if defined(FOG_CUSTOM_MODE)
							float g = ceil(colorInterpolated.r * _FogSmoothness) / _FogSmoothness;
							result.rgba = lerp(_FogColor, c.rgba, g);
						#else
							result.rgba = lerp(_FogColor, c.rgba, colorInterpolated.r);
						#endif
					} else {
						result.rgba = lerp(_FogColor, c.rgba, color.r); 
					}

					// Uncomment for debugging purposes
					// result.rgba = lerp(colorInterpolated.rgba, c.rgba, 0.2);
				// Otherwise, show the tile as completely hidden in the fog
				} else {
					result.rgba = lerp(_FogColor, c.rgba, _InitialFogTransparency);
				}
            	
				return result;
            }
            ENDHLSL
        }
    }
}