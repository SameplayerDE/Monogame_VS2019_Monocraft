#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

matrix WorldViewProjection;
matrix World;

float3 LightPosition = float3(-5, -5, 5);
struct VertexShaderInput
{
	float4 Position : POSITION0;
	float3 Normal : NORMAL0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float3 Normal : NORMAL0;
	float3 LightDirection : TEXCOORD1;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

	output.Position = mul(input.Position, WorldViewProjection);
	output.Color =  float4(1, 1, 1, 1);
	output.Normal = mul(input.Normal, World);
	output.LightDirection = LightPosition - mul(input.Position, World);

	return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float4 Color = input.Color;

	float3 N = normalize(input.Normal);
	float3 L = normalize(input.LightDirection);

	float A = saturate(dot(N, L));
	A = clamp(A, 0.1f, 1);

	return (Color * A);
}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		ZEnable = true;
		ZWriteEnable = true;
		CullMode = CCW;
		AlphaBlendEnable = false;
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};