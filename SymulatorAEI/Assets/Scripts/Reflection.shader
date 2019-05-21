Shader "Custom/Reflection"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Cube("Cube", Cube) = "" {}

		_Inten("Inten1", Range(0.01, 1)) = 0.5
		_Inten2("Inten2", Range(0.01, 1)) = 0.5
		_Color("Main Color", Color) = (1,1,1)
		_Color2("Secound Color", Color) = (0.5,0.5,1)
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 100

			//ZWrite Off
			//Blend SrcAlpha OneMinusSrcAlpha
			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				// make fog work
				#pragma multi_compile_fog

				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{

					half3 worldNormal : TEXCOORD2;
					float2 uv : TEXCOORD0;
					float3 Reflection : TEXCOORD3;
					//  UNITY_FOG_COORDS(1)
					  float4 vertex : SV_POSITION;
				  };

				  sampler2D _MainTex;
				  float4 _MainTex_ST;

				  v2f vert(appdata v, float3 normal : NORMAL)
				  {
					  v2f o;
					  o.vertex = UnityObjectToClipPos(v.vertex);
					  o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					  o.worldNormal = UnityObjectToWorldNormal(normal);
					  //o.VSNormal = COMPUTE_VIEW_NORMAL;
					  UNITY_TRANSFER_FOG(o,o.vertex);
					  float3 viewDir = UNITY_MATRIX_IT_MV[2].xyz;

					  o.Reflection = reflect(-normalize(viewDir), normalize(o.worldNormal));

					  return o;
				  }


				  samplerCUBE _Cube;
				  float _Inten;
				  float3 _Color;
				  float _Inten2;
				  float3 _Color2;

				  fixed4 frag(v2f i) : SV_Target
				  {

					  //to co na laborkach było(cieniowanie)
					  float4 dirVec = _WorldSpaceLightPos0;
					  float3 norVec = i.worldNormal;

					  float4 t = dot(normalize(dirVec), normalize(norVec));
					  t = max(t,0);

					  float3 swiatlo;
					  swiatlo = _Inten * _Color;
					  swiatlo += _Inten2 * t *_Color2;

					  swiatlo = min(swiatlo,1);

					  fixed4 col = tex2D(_MainTex, i.uv);//nadanie koloru z tekstury(domyslnie bialy)
					  col.rgb *= texCUBE(_Cube,normalize(i.Reflection)); //dodanie odbicia 

					  col.rgb *= swiatlo;
					  col.a = 0.5;
					  // UNITY_APPLY_FOG(i.fogCoord, col);
					   return col;
				   }
				   ENDCG
			   }
		}
}
