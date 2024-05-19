// Made with Amplify Shader Editor v1.9.3.3
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Impostors/Examples/StandardCrossfade"
{
	Properties
	{
		_BaseColor("BaseColor", Color) = (1,1,1,0)
		_MainTex("Main Tex", 2D) = "white" {}
		[NoScaleOffset]_Normal("Normal", 2D) = "bump" {}
		[NoScaleOffset]_Occlusion("Occlusion", 2D) = "white" {}
		_OcclusionAmount("Occlusion Amount", Range( 0 , 1)) = 1
		[NoScaleOffset]_SpecularSmoothness("Specular & Smoothness", 2D) = "white" {}
		_SpecSmoothTint("Spec Smooth Tint", Color) = (0.5019608,0.5019608,0.5019608,0.1176471)
		_DetailAlbedo("Detail Albedo", 2D) = "white" {}
		[NoScaleOffset]_DetailMask("Detail Mask", 2D) = "white" {}
		[NoScaleOffset]_DetailNormal("Detail Normal", 2D) = "bump" {}
		[Toggle(_USEDETAILMAPS_ON)] _UseDetailMaps("Use Detail Maps", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#pragma target 3.0
		#pragma multi_compile_instancing
		#pragma shader_feature_local _USEDETAILMAPS_ON
		#pragma surface surf StandardSpecular keepalpha addshadow fullforwardshadows dithercrossfade 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Normal;
		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		uniform sampler2D _DetailNormal;
		uniform sampler2D _DetailAlbedo;
		uniform float4 _DetailAlbedo_ST;
		uniform sampler2D _DetailMask;
		uniform float4 _BaseColor;
		uniform sampler2D _SpecularSmoothness;
		uniform float4 _SpecSmoothTint;
		uniform sampler2D _Occlusion;
		uniform float _OcclusionAmount;

		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			float2 uv_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			float3 tex2DNode6 = UnpackNormal( tex2D( _Normal, uv_MainTex ) );
			float2 uv_DetailAlbedo = i.uv_texcoord * _DetailAlbedo_ST.xy + _DetailAlbedo_ST.zw;
			float3 lerpResult20 = lerp( tex2DNode6 , BlendNormals( UnpackNormal( tex2D( _DetailNormal, uv_DetailAlbedo ) ) , tex2DNode6 ) , tex2D( _DetailMask, uv_MainTex ).r);
			#ifdef _USEDETAILMAPS_ON
				float3 staticSwitch29 = lerpResult20;
			#else
				float3 staticSwitch29 = tex2DNode6;
			#endif
			float3 Normal44 = staticSwitch29;
			o.Normal = Normal44;
			float4 temp_output_31_0 = ( _BaseColor * tex2D( _MainTex, uv_MainTex ) );
			#ifdef _USEDETAILMAPS_ON
				float4 staticSwitch30 = ( temp_output_31_0 * ( tex2D( _DetailAlbedo, uv_DetailAlbedo ) * float4( (unity_ColorSpaceDouble).rgb , 0.0 ) ) );
			#else
				float4 staticSwitch30 = temp_output_31_0;
			#endif
			float4 BaseColor43 = staticSwitch30;
			o.Albedo = BaseColor43.rgb;
			float Smoothness45 = (( tex2D( _SpecularSmoothness, uv_MainTex ) * _SpecSmoothTint )).a;
			o.Smoothness = Smoothness45;
			float AO46 = ( 1.0 - ( ( 1.0 - tex2D( _Occlusion, uv_MainTex ).r ) * _OcclusionAmount ) );
			o.Occlusion = AO46;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
}
/*ASEBEGIN
Version=19303
Node;AmplifyShaderEditor.CommentaryNode;52;-314.6347,-776.8505;Inherit;False;400.0192;232.2975;UNITY_COLORSPACE_GAMMA;2;54;53;;1,0.920137,0,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;28;-691.7921,-249.6511;Inherit;False;0;1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorSpaceDouble;54;-299.7346,-721.9835;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;15;-430.5605,-275.4195;Inherit;True;Property;_DetailMask;Detail Mask;8;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SwizzleNode;53;-74.42896,-726.6515;Inherit;False;FLOAT3;0;1;2;3;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;32;-42.49635,-185.4875;Float;False;Property;_BaseColor;BaseColor;0;0;Create;True;0;0;0;False;0;False;1,1,1,0;1,1,1,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-433.049,-50.78706;Inherit;True;Property;_MainTex;Main Tex;1;0;Create;False;0;0;0;False;0;False;-1;None;488be4821ece012498ce423c38511544;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;40;-133.2533,-171.156;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;17;-1069.108,-468.9992;Inherit;False;0;12;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;213.0271,-69.4889;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;55;116.6123,-665.2183;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WireNode;37;-125.4624,233.9708;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;4;-435.3029,677.8322;Inherit;True;Property;_Occlusion;Occlusion;3;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;40ce0b7000ac5df449255a5e0d17119f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;6;-427.6892,185.3968;Inherit;True;Property;_Normal;Normal;2;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;096e8b491913f204ca1bd73901acfcec;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;16;-813.7731,110.9839;Inherit;True;Property;_DetailNormal;Detail Normal;9;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;36;350.2331,-72.03726;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;12;-434.0295,-494.7381;Inherit;True;Property;_DetailAlbedo;Detail Albedo;7;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;57;118.0737,-483.2153;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;7;-427.2519,406.044;Inherit;True;Property;_SpecularSmoothness;Specular & Smoothness;5;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;a167f1cf54425724a8f890313aefc4d4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendNormalsNode;19;-102.7332,114.5559;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WireNode;38;-119.8974,264.0212;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WireNode;39;-110.9936,300.7497;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;25;-97.14062,494.2301;Float;False;Property;_SpecSmoothTint;Spec Smooth Tint;6;0;Create;True;0;0;0;False;0;False;0.5019608,0.5019608,0.5019608,0.1176471;1,1,1,0.141;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;10;-121.6772,704.4097;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-135.2775,801.3902;Float;False;Property;_OcclusionAmount;Occlusion Amount;4;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;33;522.7457,-96.5229;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;35;339.0717,-438.241;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;182.9125,-493.0108;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;20;156.9806,239.5123;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;144.1018,412.61;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;166.6263,701.2;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;388.9872,-516.7487;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;34;540.5221,-464.9526;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;29;344.6661,174.6349;Float;False;Property;_UseDetailMaps;Use Detail Maps;10;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT3;0,0,0;False;0;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT3;0,0,0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;11;386.9463,702.0863;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;27;382.8415,404.7944;Inherit;False;False;False;False;True;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;30;596.7079,-547.6957;Float;False;Property;_UseDetailMaps;Use Detail Maps;9;0;Create;True;0;0;0;False;0;False;0;0;0;False;_USEDETAILMAPS_ON;Toggle;2;Key0;Key1;Fetch;True;True;All;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;44;607.5042,171.757;Inherit;False;Normal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;45;605.55,399.9478;Inherit;False;Smoothness;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;46;573.9866,692.0032;Inherit;False;AO;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;43;862.9294,-547.6921;Inherit;False;BaseColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;47;1266.457,-552.9471;Inherit;False;43;BaseColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;48;1270.613,-471.2178;Inherit;False;44;Normal;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;49;1251.219,-390.8739;Inherit;False;45;Smoothness;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;51;1260.916,-310.53;Inherit;False;46;AO;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1538.979,-546.9989;Float;False;True;-1;2;;0;0;StandardSpecular;Impostors/Examples/StandardCrossfade;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;True;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;17;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;16;FLOAT4;0,0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;15;1;28;0
WireConnection;53;0;54;0
WireConnection;1;1;28;0
WireConnection;40;0;15;1
WireConnection;31;0;32;0
WireConnection;31;1;1;0
WireConnection;55;0;53;0
WireConnection;37;0;40;0
WireConnection;4;1;28;0
WireConnection;6;1;28;0
WireConnection;16;1;17;0
WireConnection;36;0;31;0
WireConnection;12;1;17;0
WireConnection;57;0;55;0
WireConnection;7;1;28;0
WireConnection;19;0;16;0
WireConnection;19;1;6;0
WireConnection;38;0;6;0
WireConnection;39;0;37;0
WireConnection;10;0;4;1
WireConnection;33;0;31;0
WireConnection;35;0;36;0
WireConnection;56;0;12;0
WireConnection;56;1;57;0
WireConnection;20;0;38;0
WireConnection;20;1;19;0
WireConnection;20;2;39;0
WireConnection;26;0;7;0
WireConnection;26;1;25;0
WireConnection;8;0;10;0
WireConnection;8;1;9;0
WireConnection;14;0;35;0
WireConnection;14;1;56;0
WireConnection;34;0;33;0
WireConnection;29;1;6;0
WireConnection;29;0;20;0
WireConnection;11;0;8;0
WireConnection;27;0;26;0
WireConnection;30;1;34;0
WireConnection;30;0;14;0
WireConnection;44;0;29;0
WireConnection;45;0;27;0
WireConnection;46;0;11;0
WireConnection;43;0;30;0
WireConnection;0;0;47;0
WireConnection;0;1;48;0
WireConnection;0;4;49;0
WireConnection;0;5;51;0
ASEEND*/
//CHKSM=D26FC5436335CC546D11435CDCFA588295C1F784