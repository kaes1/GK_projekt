Shader "Custom/Refrakcja"
{
	Properties
	{
		_MainTex("Texture", 2D) = "" {}
		_Cube("Cube", Cube) = "" {}

		_indeks("indeks", Range(0.01, 3)) = 1.5
	}
		SubShader
		{
			Tags {"Queue" = "Transparent" "RenderType" = "Transparent" }
			LOD 100

			//ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			Pass
			{
				CGPROGRAM
				#pragma vertex vert alpha
				#pragma fragment frag alpha
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

					float2 uv : TEXCOORD0;
					float3 Refraction : TEXCOORD1;
					//  UNITY_FOG_COORDS(1)
					  float4 vertex : SV_POSITION;
				  };

				  sampler2D _MainTex;
				  float4 _MainTex_ST;
				  float _indeks;

				  v2f vert(appdata v, float3 normal : NORMAL)
				  {
					  v2f o;
					  o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					 // output.normalDir = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
					  float3 viewDir = mul(unity_ObjectToWorld , v.vertex).xyz -_WorldSpaceCameraPos;
					  float3 normalDirection = normalize( mul(float4(normal,0.0),unity_WorldToObject).xyz );
					  //vec3(modelMatrix * gl_Vertex - vec4(_WorldSpaceCameraPos, 1.0));
					  
					  o.Refraction = refract(normalize(viewDir), normalize(normalDirection), _indeks);
					  
					  o.vertex = UnityObjectToClipPos(v.vertex);
					  return o;
				  }


				  samplerCUBE _Cube;

				  fixed4 frag(v2f i) : SV_Target
				  {
				  fixed4 col = texCUBE(_Cube,i.Refraction) * tex2D(_MainTex,i.uv);
				  return col;
				   }
				   ENDCG
			   }
		}
}
