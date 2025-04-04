#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED

// @Cyanilux | https://github.com/Cyanilux/URP_ShaderGraphCustomLighting

//------------------------------------------------------------------------------------------------------
// Main Light
//------------------------------------------------------------------------------------------------------

/*
- Obtains the Direction, Color and Distance Atten for the Main Light.
- (DistanceAtten is either 0 or 1 for directional light, depending if the light is in the culling mask or not)
- If you want shadow attenutation, see MainLightShadows_float, or use MainLightFull_float instead
*/
void MainLight_float (out float3 Direction, out float3 Color, out float DistanceAtten){
	#ifdef SHADERGRAPH_PREVIEW
		Direction = normalize(float3(1,1,-0.4));
		Color = float4(1,1,1,1);
		DistanceAtten = 1;
	#else
		Light mainLight = GetMainLight();
		Direction = mainLight.direction;
		Color = mainLight.color;
		DistanceAtten = mainLight.distanceAttenuation;
	#endif
}

//------------------------------------------------------------------------------------------------------
// Main Light Shadows
//------------------------------------------------------------------------------------------------------

// requires keyword _MAIN_LIGHT_SHADOWS set to multi compile
// requires keyword _MAIN_LIGHT_SHADOWS_CASCADE set to multi compile
// requires keyword _SHADOWS_SOFT set to multi compile

void MainLightShadows_float (float3 WorldPos, out float ShadowAtten){
	#ifdef SHADERGRAPH_PREVIEW
	ShadowAtten = 0.5; // Show some shadow in preview
	#else
	// Get main light
	Light mainLight = GetMainLight();
        
	// Calculate shadow coordinates
	float4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
        
	// Try a more direct shadow sampling
	#if defined(_MAIN_LIGHT_SHADOWS) || defined(_MAIN_LIGHT_SHADOWS_CASCADE)
	// If shadow keywords are defined, use this path
	ShadowAtten = MainLightRealtimeShadow(shadowCoord);
	#else
	// Fallback path that might work without keywords
	ShadowSamplingData shadowSamplingData = GetMainLightShadowSamplingData();
	half shadowStrength = GetMainLightShadowStrength();
            
	ShadowAtten = SampleShadowmap(shadowCoord, TEXTURE2D_ARGS(_MainLightShadowmapTexture, sampler_MainLightShadowmapTexture), 
								  shadowSamplingData, shadowStrength, false);
	#endif
	#endif
}
#endif // CUSTOM_LIGHTING_INCLUDED