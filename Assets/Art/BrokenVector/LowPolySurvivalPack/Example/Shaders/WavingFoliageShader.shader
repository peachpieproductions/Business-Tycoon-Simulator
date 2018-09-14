Shader "BrokenVector/WavingFoliageShader"
{
	Properties
	{
		_Texture("Texture", 2D) = "white" {}
		_SwingSpeed("Swing Speed", Float) = 0
		_Noise("Noise", Float) = 0
		_SwingAmplitude("Swing Amplitude", Float) = 0
		_HeightStart("Height Start", Float) = 0
		_HeightFactor("Height Factor", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Texture;
		uniform float4 _Texture_ST;
		uniform float _SwingAmplitude;
		uniform float _SwingSpeed;
		uniform float _HeightFactor;
		uniform float _HeightStart;
		uniform float _Noise;


		float3 mod289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float mulTime43 = _Time.y * 2.0;
			float3 ase_vertex3Pos = v.vertex.xyz;
			float temp_output_42_0 = ( ( mulTime43 + ase_vertex3Pos.x + ase_vertex3Pos.z ) * _SwingSpeed );
			float4 appendResult90 = (float4(sin( temp_output_42_0 ) , 0.0 , cos( temp_output_42_0 ) , 0.0));
			float temp_output_95_0 = ( _HeightFactor * ( _HeightStart + ase_vertex3Pos.y ) );
			float2 appendResult76 = (float2(ase_vertex3Pos.x , ase_vertex3Pos.y));
			float simplePerlin2D68 = snoise( ( appendResult76 + _Time.y ) );
			float2 appendResult77 = (float2(ase_vertex3Pos.z , ase_vertex3Pos.y));
			float simplePerlin2D75 = snoise( ( _Time.y + appendResult77 ) );
			float4 appendResult74 = (float4(simplePerlin2D68 , 0.0 , simplePerlin2D75 , 0.0));
			float4 VertexAnimation15 = ( ( _SwingAmplitude * appendResult90 * temp_output_95_0 ) + ( _Noise * appendResult74 * temp_output_95_0 ) );
			v.vertex.xyz += VertexAnimation15.xyz;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Texture = i.uv_texcoord * _Texture_ST.xy + _Texture_ST.zw;
			o.Albedo = tex2D( _Texture, uv_Texture ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
}