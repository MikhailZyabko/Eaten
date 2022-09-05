Shader "Unlit/CanalizationWater"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Scale ("Scale", Float) = 5
		_Speed ("Speed", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			
			#pragma multi_compile_fwdbase_fullshadow

            #include "UnityCG.cginc"
			#include "AutoLight.cginc"
			#include "Lighting.cginc"

            struct appdata
			{
				float4 vertex : POSITION; 
				float2 uv : TEXCOORD0;
				float4 normal:NORMAL;

			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float3 worldPos:TEXCOORD2;
				float3 screenPos:TEXCOORD3;
				float3 normal:TEXCOORD4;
				float4 _ShadowCoord :TEXCOORD5;
			};

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float _Scale;
			float _Speed;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv.y = o.uv.y * _Scale - _Time.w * _Speed;
				o.normal = normalize(mul(unity_WorldToObject, float4(v.normal.xyz, 0)).xyz);
				o.screenPos = ComputeScreenPos(o.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o._ShadowCoord = ComputeScreenPos(o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
				// Sample Shadows
				fixed atten = SHADOW_ATTENUATION(i);
				// if w == 0 then this is dir light, else point 
				float3 lightDir = normalize(_WorldSpaceLightPos0.xyz - i.worldPos.xyz *_WorldSpaceLightPos0.w);
				fixed3 diffuse = col*max(0.0, dot(i.normal, lightDir))*atten;
				fixed3 AO = (UNITY_LIGHTMODEL_AMBIENT.rgb) * 0.4;
				// Phong spec model
				
				/*float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
				float3 refl = reflect(-lightDir,normalize(i.normal));
				fixed3 spec = col*_SpecColor* pow(max(0.0, dot(refl, viewDir)), _Shininess)*atten;*/
				return fixed4(diffuse+AO,1);
            }
            ENDCG
        }
		
		Pass
		{
			Tags{"LightMode" = "ForwardAdd"}
			Blend One One

			CGPROGRAM
			#pragma multi_compile_fwdadd_fullshadows
            #pragma vertex vert
            #pragma fragment frag

			#include "Lighting.cginc"
			#include "AutoLight.cginc"
			
			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Scale;
			float _Speed;

			struct a2v
			{
				float4 vertex:POSITION;
				float3 normal:NORMAL;
				float2 uv : TEXCOORD0;
			};
			
			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 pos:SV_POSITION;
				float3 worldNormal:TEXCOORD1;
				float3 worldPos: TEXCOORD2;
				LIGHTING_COORDS(3,4)

			};
			
			v2f vert(a2v v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv.y = o.uv.y * _Scale - _Time.w * _Speed;
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldPos = mul(unity_ObjectToWorld,v.vertex).xyz;
				TRANSFER_VERTEX_TO_FRAGMENT(o);
				return o;
				
			}
			
			fixed4 frag(v2f i):SV_Target
			{
                fixed4 col = tex2D(_MainTex, i.uv);
				fixed3 worldNormal = normalize(i.worldNormal);
				fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));

				//fixed3 halfLambert = dot(worldNormal,worldLightDir) *0.5 + 0.5;
				fixed3 diffuse = _LightColor0.rgb * col * max(0,dot(worldNormal,worldLightDir));

				//fixed3 atten = LIGHT_ATTENUATION(i);
				UNITY_LIGHT_ATTENUATION(atten,i,i.worldPos);

				return fixed4((diffuse)*atten,1.0);
			}

			ENDCG
		}
    }
}
