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

Texture2D Texture : register(t0);
sampler TextureSampler : register(s0)
{
	Texture = (Texture);
	MinFilter = Point; // Minification Filter
	MagFilter = Point; // Magnification Filter
	MipFilter = Point; // Mip-mapping
	AddressU = Wrap; // Address Mode for U Coordinates
	AddressV = Wrap; // Address Mode for V Coordinates
};

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float2 TextureUVs : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureUVs : TEXCOORD0;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

	output.Position = mul(input.Position, WorldViewProjection);
	output.Color = float4(1, 1, 1, 1);
	//output.Normal = mul(input.Normal, World);
	//output.LightDirection = LightPosition - mul(input.Position, World);
	output.TextureUVs = input.TextureUVs;

	return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float2 calculatedUVs = input.TextureUVs;
	float4 Color = input.Color * tex2D(TextureSampler, calculatedUVs);

	//float3 N = normalize(input.Normal);
	//float3 L = normalize(input.LightDirection);

	//float A = saturate(dot(N, L));
	//A = clamp(A, 0.1f, 1);

	return (Color);
}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		CullMode = CCW;
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};