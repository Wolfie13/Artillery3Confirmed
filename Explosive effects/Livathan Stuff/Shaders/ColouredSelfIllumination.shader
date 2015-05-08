// Upgrade NOTE: replaced 'SeperateSpecular' with 'SeparateSpecular'

Shader "Mine/ColouredSelfIllumination"
{
	Properties
	{
		_Color("Main Color", Color) = (1,1,1,0)
		_AdditiveColor("Additive Color", Color) = (1,1,1,0)
//		_SpecColor("Spec Color", Color) = (1,1,1,1)
//		_Emission("Emissive Color", Color) = (0,0,0,0)
//		_Shininess("Shininess", Range (0.01, 1)) = 0.7
		_MainTex("Base (RGB) Gloss (A)", 2D) = "white"
		_AddTex("Additive (RGB)", 2D) = "black" 
	}
	SubShader
	{
//		UsePass "Diffuse/BASE"
		Pass
		{
			Material
			{
				Diffuse[_Color]
				Ambient[_Color]
//				Shininess[_Shininess]
//				Specular[_SpecColor]
//				Emission[_Emission]        
			}
			SeparateSpecular On
			Lighting On
			SetTexture[_MainTex]
			{
				constantColor[_Color]
				Combine texture * primary DOUBLE, texture * constant
			}
		}
		Pass
		{
			Blend One One
			Fog {Mode Off}
			ZTest LEqual
			ZWrite Off
			SetTexture[_AddTex]
			{
				constantColor[_AdditiveColor]
				Combine texture * constant
			}
		}
	} 
	FallBack " Diffuse", 1
}