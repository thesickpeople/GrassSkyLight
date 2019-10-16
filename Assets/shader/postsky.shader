Shader "myshaders/postsky"
{
	Properties
	{
		_NoiseTex("_NoiseTex", 2D) = "white" {}
		_Skycolor("_Skycolor",color)=(1,1,1,1)
		_Skycolor2("_Skycolor2",color)=(1,1,1,1)
		_Sunpos("_Sunpos",vector)=(-0.8,0.4,-0.3,0)
		_Cloudcolor("_Cloudcolor",color)=(1.0,0.95,1.0)
		_Cloudspd("_Cloudspd",float)=1
		_Layer("_Layer",float)=3
	}
	SubShader
	{
		Pass
		{
			ZTest Always Cull Off ZWrite Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 FCray:TEXCOORD1;
			};

			sampler2D _NoiseTex;
			float4 _NoiseTex_TexelSize;
			float4x4 _FrustumCornersRay;
			float4x4 _UnityMatVP;
			//float4 _Windway;
			sampler2D _CameraDepthTexture;
			float4 _Skycolor;
			float4 _Skycolor2;
			float4 _Sunpos;
			float4 _Cloudcolor;
			float _Cloudspd;
			float _Layer;
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				#if UNITY_UV_STARTS_AT_TOP
				if(_NoiseTex_TexelSize.y<0)
					o.uv.y=1-o.uv.y;
				#endif
				int index =0;
				if(v.uv.x<0.5&&v.uv.y<0.5){
					index=0;
				}else if(v.uv.x>0.5&&v.uv.y<0.5){
					index=1;
				}else if(v.uv.x>0.5&&v.uv.y>0.5){
					index=2;
				}else{
					index=3;
				}
				#if UNITY_UV_STARTS_AT_TOP
				if(_NoiseTex_TexelSize.y<0)
					index=3-index;
				#endif
				o.FCray=_FrustumCornersRay[index];
				return o;
			}
			float FBM(float2 p,float t){
				float2 f=0.0;
				float s=0.5;
				for(int i=0;i<5;i++){
					p+=t;//时间偏移
					t*=1.5;//速度不同
					f+=s*tex2D(_NoiseTex,p/256).x;
					p*=2.0;
					s*=0.5;
				}
				return f;
			}
			fixed4 frag (v2f i) : SV_Target
			{
				//float linearDepth=LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture,i.uv));
				//float3 worldPos=_WorldSpaceCameraPos+linearDepth*i.FCray.xyz;
				i.FCray=normalize(i.FCray);
				//-----天空
				//距离越远颜色越深,*1.1是为了让颜色不要太深
				float3 col = _Skycolor*1.1-i.FCray.y*i.FCray.y*0.5;
				//为了把下面的颜色剔除，给一个制定的颜色
				col=lerp(col,_Skycolor2,pow(1.0-max(i.FCray.y,0.0),3.0));
				//-----/天空
				//-----太阳
				//通过向量点乘，确定太阳位置A.*b>0说明夹角小于90°
				float sundot=clamp(dot(i.FCray.xyz,normalize(_Sunpos.xyz)),0.0,1.0);
				//感性计算，位置越正，rgb越趋近于1
				col+=0.25*float3(1.0,0.7,0.4)*pow(sundot,5.0);
				col+=0.25*float3(1.0,0.8,0.6)*pow(sundot,64.0);
				col+=0.2*float3(1.0,0.8,0.6)*pow(sundot,512.0);
				//-----/太阳
				//-----云
				float3 campos=_WorldSpaceCameraPos;
				float time=_Time.y*0.05*_Cloudspd;
				float3 c=i.FCray.xyz;
				for(int i=0;i<_Layer;i++){
					//给不同的层不同的高度
					float2 sc=campos.xz+c.xz*((i+3)*40000.0-campos.y)/c.y;
					//噪声插值混色
					col=lerp(col,_Cloudcolor,0.5*smoothstep(0.399,0.69,FBM(0.000039*sc,time*(i+3))));
				}
				//把下面的云剔除
				col=lerp(col,_Skycolor2,pow(1.0-max(c.y,0.0),16.0));
				//-----/云
				return float4(col,1);
			}
			ENDCG
		}
	}
}
