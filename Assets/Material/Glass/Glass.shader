Shader "Custom/Glass"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_GrassSpeed("Speed", Range(0,50)) = 2  // �Զ�ҡ���ٶ�
		_GrassWind("Bend amount", Range(0,50)) = 10  // ������
		[Space]
        [Toggle] _MANUALWIND("Manually animated?", Int) = 0  // �Ƿ��ֶ�ҡ�� , 0Ϊ�Զ�������Ϊ�ֶ�
		_GrassManualAnim("Manual Anim Value", Range(-1,1)) = 1   // ҡ�ڷ���

		[HideInInspector] _RandomSeed("_MaxYUV", Range(0, 10000)) = 0.0
    }
    SubShader
    {
		Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}

        Pass
        {
            ZWrite Off   // ͸���Ȼ����Ҫ�ر����д��,�رպ��͸�����岻���޸���Ȼ��壬���ܻᵼ�´�������
            Blend SrcAlpha OneMinusSrcAlpha  // ���û��ģʽ

            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma shader_feature _MANUALWIND_ON

                #include "UnityCG.cginc"

                half _GrassSpeed;
                half _GrassWind;
                half _GrassManualAnim;
                half _GrassRadialBend;
                float _GrassManualToggle;
                sampler2D _MainTex;
                float4 _MainTex_ST;

                struct a2v{
                    float4 vertex : POSITION;
                    float4 texcoord : TEXCOORD0;
                };

                struct v2f{
                    float4 pos : SV_POSITION;
                    float2 uv : TEXCOORD2;
                };

            UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_DEFINE_INSTANCED_PROP(float, _RandomSeed)
            UNITY_INSTANCING_BUFFER_END(Props)

            v2f vert (a2v v)
            {
                v2f o;         	
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target{
                half randomSeed = UNITY_ACCESS_INSTANCED_PROP(Props, _RandomSeed);
			    half windOffset = sin((_Time + randomSeed) * _GrassSpeed * 10);
			    half2 windCenter = half2(0.5, 0.1);
                float2 uvRect = i.uv;

			    #if !_MANUALWIND_ON
			        i.uv.x = fmod(abs(lerp(i.uv.x, i.uv.x + (_GrassWind * 0.01 * windOffset), uvRect.y)), 1);
			    #else
			        i.uv.x = fmod(abs(lerp(i.uv.x, i.uv.x + (_GrassWind * 0.01 * _GrassManualAnim), uvRect.y)), 1);
			        windOffset = _GrassManualAnim;
			    #endif

			    half2 delta = i.uv - windCenter;
			    half delta2 = dot(delta.xy, delta.xy);
			    half2 delta_offset = delta2 * windOffset;
			    i.uv = i.uv + half2(delta.y, -delta.x) * delta_offset * _GrassRadialBend;

				half4 col = tex2D(_MainTex, i.uv);
                return tex2D(_MainTex, i.uv);
            }

            ENDCG
        }
    }
}
