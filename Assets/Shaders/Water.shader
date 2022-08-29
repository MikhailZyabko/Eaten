// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Water"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
		_Amplitude ("Wave Amplitude", Float) = 0.1
		_Freaquency ("Wave Freaquency", Float) = 0.5
		_Length ("Wave Length", Float) = 1
		_Dir ("Wave Direction", Vector) = (1, 0, 0, 0)
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
			Cull Back
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase

            #include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc"
			
			struct Wave
			{
				float amplitude;
			    float length;
				float freaquency;
				float2 dir;
			};

            struct appdata
            {
                float4 vertex : POSITION;
				float4 normal : NORMAL;
				float4 tangent : TANGENT;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
				float3 normal : TEXCOORD0;
				float3 worldPos : TEXCOORD1;
            };

			float4 _Color;
			float _Amplitude;
			float _Freaquency;
			float _Length;
			float4 _Dir;
			
			float find_offset(float3 t, Wave w)
			{					
				float waveLength = 1  / _Length;
			    return w.amplitude * sin( (w.length * dot(w.dir, t.xz) + w.freaquency * _Time.y) * waveLength);
			}
			
			float find_offsetdx(float3 t, Wave w)
			{					
				float waveLength = 1  / _Length;
			    return w.amplitude * cos( (w.length * dot(w.dir, t.xz) + w.freaquency * _Time.y) * waveLength) * w.dir.x * w.length;
			}
			
			float find_offsetdz(float3 t, Wave w)
			{					
				float waveLength = 1  / _Length;
			    return w.amplitude * cos( (w.length * dot(w.dir, t.xz) + w.freaquency * _Time.y) * waveLength) * w.dir.y * w.length;
			}

            v2f vert (appdata v)
            {
				Wave waves[5] = {
					{_Amplitude, 1, _Freaquency, _Dir.xz},
					{1.5 * _Amplitude, 1.5, 0.5 * _Freaquency, _Dir.zx},
					{2.5 * _Amplitude, 0.5, 0.1 * _Freaquency, float2(0.5, 0.5)},
					{0.5 * _Amplitude, 2.5, 1.5 * _Freaquency, float2(-0.4, -0.6)},
					{0.2 * _Amplitude, 0.75, 5 * _Freaquency, float2(-0.2, 0.8)}
				};
				
                v2f o;
				float4 t = mul(unity_ObjectToWorld, v.vertex);
				
				for(int i = 0; i < 5; i++)
				{
					t.y += find_offset(t, waves[i]);
				}
				
				//o.vertex = UnityObjectToClipPos(t);
				o.vertex = mul(UNITY_MATRIX_VP, t);
				o.worldPos = t;
				o.normal = v.normal;
				//o.normal = normal;
				
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float dx = 0;
				float dz = 0;
				
				Wave waves[5] = {
					{_Amplitude, 1, _Freaquency, _Dir.xz},
					{1.5 * _Amplitude, 1.5, 0.5 * _Freaquency, _Dir.zx},
					{2.5 * _Amplitude, 0.5, 0.1 * _Freaquency, float2(0.5, 0.5)},
					{0.5 * _Amplitude, 2.5, 1.5 * _Freaquency, float2(-0.4, -0.6)},
					{0.2 * _Amplitude, 0.75, 5 * _Freaquency, float2(-0.2, 0.8)}
				};
				
				for(int j = 0; j < 5; j++)
				{
					dx += find_offsetdx(i.worldPos, waves[j]);
					dz += find_offsetdz(i.worldPos, waves[j]);
				}
			
				float3 n = mul(unity_ObjectToWorld, normalize(float3(-dx, 1, -dz)));
				float3 l = normalize(_WorldSpaceLightPos0);
				float3 v = normalize(_WorldSpaceCameraPos - i.worldPos);
				float3 r = reflect(-v, n);
				fixed3 diff = _Color * max(0.6, dot(l, n)) * _LightColor0;
				fixed4 spec = pow(max(0, dot(r, l)), 20) * _LightColor0;
                return fixed4(diff + spec, 1);
				return fixed4(i.normal, 1);
				return _Color;
            }
            ENDCG
        }
    }
}
