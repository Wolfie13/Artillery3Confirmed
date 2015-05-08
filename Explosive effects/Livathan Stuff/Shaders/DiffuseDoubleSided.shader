Shader "Mine/DiffuseDoubleSided"
{
	Properties
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white"
	}
	SubShader
	{
		Pass
		{
			Material
			{
				Diffuse[_Color]
				Ambient[_Color]
			}
			Cull Off
			Lighting On
			SetTexture[_MainTex]
			{
				constantColor[_Color]
				Combine texture * primary DOUBLE, texture * constant
			}
		}
	} 
	FallBack " Diffuse", 1
}
