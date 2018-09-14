Shader "BrokenVector/LowPolyWaterShader"
{
	Properties
	{
		_WaterColor("Water Color", Color) = (0.4926471,0.8740366,1,1)
		_WaveGuide("Wave Guide", 2D) = "white" {}
		_WaveSpeed("Wave Speed", Range( 0 , 5)) = 0
		_WaveHeight("Wave Height", Range( 0 , 5)) = 0
		_FoamColor("Foam Color", Color) = (1,1,1,0)
		_Foam("Foam", 2D) = "white" {}
		_FoamDistortion("Foam Distortion", 2D) = "white" {}
		_FoamDist("Foam Dist", Range( 0 , 1)) = 0.1
		_Smoothness("Smoothness", Range( 0 , 1)) = 0.15
		_Reflection("Reflection", Range( 0 , 1)) = 0.15
		_Opacity("Opacity", Range( 0 , 1)) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma surface surf StandardSpecular alpha:fade keepalpha noshadow vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
			float4 screenPos;
		};

		uniform float4 _WaterColor;
		uniform float4 _FoamColor;
		uniform sampler2D _Foam;
		uniform float _WaveSpeed;
		uniform float4 _Foam_ST;
		uniform sampler2D _FoamDistortion;
		uniform sampler2D _CameraDepthTexture;
		uniform float _FoamDist;
		uniform float _Reflection;
		uniform float _Smoothness;
		uniform float _Opacity;
		uniform sampler2D _WaveGuide;
		uniform float _WaveHeight;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float4 speed183 = ( _Time * _WaveSpeed );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float2 uv_TexCoord96 = v.texcoord.xy * float2( 1,1 ) + ( speed183 + (ase_vertex3Pos).y ).xy;
			float3 ase_vertexNormal = v.normal.xyz;
			float3 VertexAnimation127 = ( ( tex2Dlod( _WaveGuide, float4( uv_TexCoord96, 0, 1.0) ).r - 0.5 ) * ( ase_vertexNormal * _WaveHeight ) );
			v.vertex.xyz += VertexAnimation127;
		}

		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 normalizeResult123 = normalize( ( cross( ddx( ase_worldPos ) , ddy( ase_worldPos ) ) + float3( 1E-09,0,0 ) ) );
			float3 Normal124 = normalizeResult123;
			o.Normal = Normal124;
			float4 Albedo131 = _WaterColor;
			o.Albedo = Albedo131.rgb;
			float4 speed183 = ( _Time * _WaveSpeed );
			float2 uv_Foam = i.uv_texcoord * _Foam_ST.xy + _Foam_ST.zw;
			float2 panner177 = ( uv_Foam + speed183.x * float2( 0.5,0.5 ));
			float cos182 = cos( speed183.x );
			float sin182 = sin( speed183.x );
			float2 rotator182 = mul( panner177 - float2( 0,0 ) , float2x2( cos182 , -sin182 , sin182 , cos182 )) + float2( 0,0 );
			float clampResult181 = clamp( tex2D( _FoamDistortion, rotator182 ).r , 0.0 , 1.0 );
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth164 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(ase_screenPos))));
			float distanceDepth164 = abs( ( screenDepth164 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _FoamDist ) );
			float clampResult191 = clamp( ( clampResult181 * distanceDepth164 ) , 0.0 , 1.0 );
			float4 lerpResult157 = lerp( ( _FoamColor * tex2D( _Foam, rotator182 ) ) , float4(0,0,0,0) , clampResult191);
			float4 Emission162 = lerpResult157;
			o.Emission = Emission162.rgb;
			float3 temp_cast_4 = (_Reflection).xxx;
			o.Specular = temp_cast_4;
			o.Smoothness = _Smoothness;
			o.Alpha = _Opacity;
		}

		ENDCG
	}
}