
Shader "myshaders/Terrainshader"
{
	Properties
	{
		_MainTex ("_MainTex", 2D) = "white" {}
		_Diffuse("_Diffuse",Color)=(1,1,1,1)
		_HalfLambert("_HalfLambert",Range(0.0,1.0))=0.386
		_Specular("_Specular",Color)=(1,1,1,1)
		_Gloss("_Gloss",Range(8.0,256))=20
		_BumpMap("_BumpMap",2D)="white"{}
		_BumpScale("_BumpScale",Float) = 1.0

	}
		SubShader
		{
			Tags { "LightMode" = "ForwardBase" }
			Pass
			{
				Name "GRASSHIGH"
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "Lighting.cginc"
				#include "AutoLight.cginc"
				#pragma multi_compile_fwdbase
				sampler2D _MainTex;
				float4 _MainTex_ST;
				float4 _Diffuse;
				fixed _HalfLambert;
				float4 _Specular;
				float _Gloss;
				sampler2D _BumpMap;
				float4 _BumpMap_ST;
				fixed _BumpScale;

			struct v2a
			{
				float4 vertex : POSITION;
				float3 normal:NORMAL;
				float4 tangent:TANGENT;//切线方向
				float4 texcoord:TEXCOORD0;
			};

			struct v2f
			{
				float4 pos:SV_POSITION;
				float4 uv:TEXCOORD0;
				float3 lightDir:TEXCOORD1;
				float3 viewDir:TEXCOORD2;
				float3 worldpos:TEXCOORD3;
				//SHADOW_COORDS(5)
			};
			v2f vert (v2a v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv.xy = v.texcoord.xy*_MainTex_ST.xy + _MainTex_ST.zw;
				o.uv.zw = v.texcoord.xy*_BumpMap_ST.xy + _BumpMap_ST.zw;
				o.uv.xy/=2;
				o.uv.zw/=2;
				TANGENT_SPACE_ROTATION; //这个内置宏可以把切线坐标变换矩阵放到rotation中
				o.lightDir = mul(rotation, ObjSpaceLightDir(v.vertex)).xyz;
				o.viewDir = mul(rotation, ObjSpaceViewDir(v.vertex)).xyz;
				o.worldpos=mul(unity_ObjectToWorld,v.vertex);
				//TRANSFER_SHADOW(o);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
			fixed3 tangentlightdir = normalize(i.lightDir);
			fixed3 tangentviewdir = normalize(i.viewDir);
			fixed4 packenormal = tex2D(_BumpMap, i.uv.zw);
			fixed3 tangentnormal = UnpackNormal(packenormal);
			tangentnormal.xy *= _BumpScale;
			tangentnormal.z = sqrt(1.0 - saturate(dot(tangentnormal.xy, tangentnormal.xy)));
			fixed3 albedo = tex2D(_MainTex, i.uv.xy).rgb*_Diffuse.rgb;
			fixed3 diffuse = _LightColor0.rgb*albedo*(dot(tangentnormal, tangentlightdir)*_HalfLambert + _HalfLambert);
			fixed3 halfDir = normalize(tangentlightdir + tangentviewdir);
			//UNITY_LIGHT_ATTENUATION(atten, i, i.worldpos);
			fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz*albedo;
			//fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz*(albedo*lerp(0.2,1,atten));
			fixed3 specular = _LightColor0.rgb*_Specular.rgb*pow(max(0, dot(tangentnormal, halfDir)), _Gloss);
			return fixed4(ambient+diffuse + specular, 1.0);
			}
			ENDCG
		}
	}
	//FallBack "Specular"
}
